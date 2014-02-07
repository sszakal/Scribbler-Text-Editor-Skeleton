//-----------------------------------------------------------------------
// <copyright file="OperationsManager.cs" company="University of Hull">
//     Copyright (c) University of Hull. All rights reserved.
// 
// This code remains the property of University of Hull. No portion of this 
// code may be reproduced without the expression written permission of 
// University of Hull.
// </copyright>
//-----------------------------------------------------------------------

using System.IO;
using System.Reflection;
using Scribbler.Types;
using System.Linq;

namespace Scribbler
{
    #region Using directives
    using System;
using System.Collections.Generic;
    using System.Windows.Forms;
    using System.Runtime.Remoting.Lifetime;
    #endregion
    /// <summary>
    /// Manages all the dynamically loadable Operations which can be loaded
    /// into the Scribbler application
    /// </summary>
    internal class OperationManager
    {
        #region Nested Types

        /// <summary>
        /// Represents a set of plugins from an appdomain, and has all the data necessary to comunicate with the remote objects.
        /// Also, it automaticaly loads and unloads appdomains.
        /// </summary>
        private sealed class PlugInsAppDomain
        {
            /// <summary>
            /// AppDomain object which contains the plugin objects.
            /// </summary>
            private AppDomain appDom;

            /// <summary>
            /// Reference to the .NET DLL file. If the appdomains is unloaded, this is used to reload it.
            /// </summary>
            private AssemblyName assemblyReference;

            /// <summary>
            /// Total number of plugins available in the plugin's appdomain.
            /// </summary>
            public uint numberOfPlugIns { get; private set; }

            /// <summary>
            /// Total number of plugins which are used (active) by the menu.
            /// </summary>
            public uint activePlugIns { get; private set; }

            /// <summary>
            /// Array of object references to the plugins available in the AppDomain.
            /// </summary>
            public RemotePlugInController[] plugInControllers { get; private set; }

            /// <summary>
            /// Parameter-less constructor. NOT AVAILABLE! DO NOT USE.
            /// </summary>
            private PlugInsAppDomain()
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            public PlugInsAppDomain(AssemblyName assemblyReference)
            {
                this.assemblyReference = assemblyReference;
                AppDomainSetup setupAppDomain = new AppDomainSetup();
                setupAppDomain.ApplicationName = assemblyReference.Name;
                setupAppDomain.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
                setupAppDomain.PrivateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "operations");
                setupAppDomain.ShadowCopyFiles = "true";
                setupAppDomain.CachePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cache-" + assemblyReference.Name);
                setupAppDomain.ShadowCopyDirectories = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cache-" + assemblyReference.Name);
                this.appDom = AppDomain.CreateDomain(assemblyReference.Name, null, setupAppDomain);

                PlugInsLoader loader = (PlugInsLoader)this.appDom.CreateInstanceAndUnwrap(typeof(PlugInsLoader).Assembly.FullName, typeof(PlugInsLoader).FullName);
                PlugInWrapper[] plugInWrapers = loader.LoadPlugins(assemblyReference);
                List<RemotePlugInController> plugInControllers = new List<RemotePlugInController>();
                foreach (PlugInWrapper plugInWrapper in plugInWrapers)
                {
                    RemotePlugInController newPlugInRemote = new RemotePlugInController(plugInWrapper);
                    plugInControllers.Add(newPlugInRemote);
                    newPlugInRemote.loadEvent += this.loadHandler;
                    newPlugInRemote.unLoadEvent += this.unLoadHandler;
                }
                this.plugInControllers = plugInControllers.ToArray();
                this.activePlugIns = (uint)this.plugInControllers.Length;
            }


            /// <summary>
            /// Handler for load and unloading appdomains.
            /// If all the plugins are disable in the menu, this will cause the appdomain to be unloaded.
            /// If one of the plugins gets enabled in the menu, this will cause the appdomain to be reloaded, 
            /// and all its remote objects to be relinked to their remote controls.
            /// </summary>
            private void loadHandler(object o, EventArgs args)
            {
                if (this.activePlugIns == 0)
                {
                    //reload appdomain
                    AppDomainSetup setupAppDomain = new AppDomainSetup();
                    setupAppDomain.ApplicationName = this.assemblyReference.Name;
                    setupAppDomain.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
                    setupAppDomain.PrivateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "operations");
                    setupAppDomain.ShadowCopyFiles = "true";
                    setupAppDomain.CachePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cache-" + this.assemblyReference.Name);
                    setupAppDomain.ShadowCopyDirectories = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cache-" + this.assemblyReference.Name);
                    this.appDom = AppDomain.CreateDomain(this.assemblyReference.Name, null, setupAppDomain);

                    PlugInsLoader loader = (PlugInsLoader)this.appDom.CreateInstanceAndUnwrap(typeof(PlugInsLoader).Assembly.FullName, typeof(PlugInsLoader).FullName);
                    PlugInWrapper[] plugInWrapers = loader.LoadPlugins(assemblyReference);

                    if (plugInWrapers.Length != this.plugInControllers.Length)
                    {
                        throw new Exception("PlugInsAppDomain: Assembly file was changed!");
                    }

                    foreach (RemotePlugInController controller in this.plugInControllers)
                    {
                        PlugInWrapper temp = null;
                        foreach (PlugInWrapper wraper in plugInWrapers)
                        {
                            if ((controller.plugInName == wraper.getPlugInName()) && (controller.plugInMenuName == wraper.getPlugInMenuName()))
                            {
                                temp = wraper;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        if (temp != null)
                        {
                            controller.plugIn = temp;
                        }
                        else
                        {
                            throw new Exception("PlugInsAppDomain: Cannot re-find plugin in assembly");
                        }
                    }
                }
                this.activePlugIns++;
            }

            /// <summary>
            /// Handler for load and unloading appdomains.
            /// If all the plugins are disable in the menu, this will cause the appdomain to be unloaded.
            /// If one of the plugins gets enabled in the menu, this will cause the appdomain to be reloaded, 
            /// and all its remote objects to be relinked to their remote controls.
            /// </summary>
            private void unLoadHandler(object o, EventArgs args)
            {
                this.activePlugIns--;
                if (this.activePlugIns == 0)
                {
                    //unload appdomain
                    AppDomain.Unload(appDom);
                    this.appDom = null;
                }
            }
        }

        /// <summary>
        /// Represents a remote control, which controls a plugin from another appdomain.
        /// </summary>
        public class RemotePlugInController
        {
            /// <summary>
            /// The name of the menu where the plugin is inserted.
            /// </summary>
            public readonly string plugInMenuName;

            /// <summary>
            /// The name of the plugin.
            /// </summary>
            public readonly string plugInName;

            /// <summary>
            /// The description for the plugin.
            /// </summary>
            public readonly string plugInDesc;

            /// <summary>
            /// The remoted object.
            /// </summary>
            public PlugInWrapper plugIn;

            /// <summary>
            /// Indicates if the plugin is active or not (used by the menu).
            /// </summary>
            private bool _active;

            /// <summary>
            /// Getter and Setter for the _active variable. Triggers load and unload events.
            /// </summary>
            public bool active
            {
                get
                {
                    return this._active;
                }
                set
                {
                    if (this._active != value)
                    {
                        this._active = value;
                        if (value)
                        {
                            this.loadEvent(this, null);
                        }
                        else
                        {
                            this.unLoadEvent(this, null);
                        }
                    }
                }
            }

            /// <summary>
            /// Load Event, triggered which the plugin is activated.
            /// </summary>
            public event EventHandler loadEvent;

            /// <summary>
            /// UnLoad Event, triggered which the plugin is deactivated.
            /// </summary>
            public event EventHandler unLoadEvent;

            /// <summary>
            /// Parameter-less constructor. NOT AVAILABLE! DO NOT USE.
            /// </summary>
            private RemotePlugInController()
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            public RemotePlugInController(PlugInWrapper plugIn)
            {
                this._active = true;
                this.plugIn = plugIn;
                this.plugInName = plugIn.getPlugInName();
                this.plugInDesc = plugIn.getPlugInDescription();
                this.plugInMenuName = plugIn.getPlugInMenuName();
            }

            /// <summary>
            /// Executes the command on the plugin AppDomain using the remoted object.
            /// </summary>
            public String Execute(String text, IWin32Window ownerWindow)
            {
                return plugIn.execute(text, ownerWindow);
            }
        }

        /// <summary>
        /// Class used to load the assembly and the plugins in the remoted appdomain.
        /// </summary>
        private sealed class PlugInsLoader : MarshalByRefObject
        {
            /// <summary>
            /// Returns an array of remoted objects.
            /// </summary>
            public PlugInWrapper[] LoadPlugins(AssemblyName assemblyReference)
            {
                List<PlugInWrapper> plugInWrappers = new List<PlugInWrapper>();

                Assembly assembly = Assembly.Load(assemblyReference);
                Type[] types = assembly.GetExportedTypes();

                foreach (Type type in types)
                {
                    PlugInWrapper newPlugIn = PlugInWrapper.CreatePlugInType(type);
                    if (newPlugIn != null)
                    {
                        plugInWrappers.Add(newPlugIn);
                    }
                }

                return plugInWrappers.ToArray();
            }

            /// <summary>
            /// Required to set the life time of the object in the remoted appdomain.
            /// </summary>
            public override Object InitializeLifetimeService()
            {
                ILease lease = (ILease)base.InitializeLifetimeService();
                if (lease.CurrentState == LeaseState.Initial)
                {
                    lease.InitialLeaseTime = TimeSpan.FromMinutes(1);
                    lease.SponsorshipTimeout = TimeSpan.FromMinutes(2);
                    lease.RenewOnCallTime = TimeSpan.FromSeconds(2);
                }
                return lease;
            }
        }

        /// <summary>
        /// Represents a remoted plugin.
        /// </summary>
        public sealed class PlugInWrapper : MarshalByRefObject
        {
            /// <summary>
            /// The plugin Type.
            /// </summary>
            private Type typeRef;

            /// <summary>
            /// The interface used to execute code.
            /// </summary>
            private IOperation typeInstance;

            /// <summary>
            /// Parameter-less constructor. NOT AVAILABLE! DO NOT USE.
            /// </summary>
            private PlugInWrapper()
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            private PlugInWrapper(Type typeRef)
            {
                this.typeRef = typeRef;
                this.typeInstance = (IOperation)Activator.CreateInstance(typeRef);
            }

            #region Public Methods
            /// <summary>
            /// Returns the plugin name.
            /// </summary>
            public string getPlugInName()
            {
                return this.typeInstance.Name;
            }
            /// <summary>
            /// Returns the plugin menu name.
            /// </summary>
            public string getPlugInMenuName()
            {
                OperationDescriptionAttribute plugInInfo = typeRef.GetCustomAttributes(false).Where<object>(new Func<object, bool>(o => o is OperationDescriptionAttribute)).Cast<OperationDescriptionAttribute>().First<OperationDescriptionAttribute>();
                if (plugInInfo != null)
                {
                    return plugInInfo.MenuName;
                }
                else
                {
                    return "Edit";
                }
            }
            /// <summary>
            /// Returns the plugin description.
            /// </summary>
            public string getPlugInDescription()
            {
                OperationDescriptionAttribute plugInInfo = typeRef.GetCustomAttributes(false).Where<object>(new Func<object, bool>(o => o is OperationDescriptionAttribute)).Cast<OperationDescriptionAttribute>().First<OperationDescriptionAttribute>();
                if (plugInInfo != null)
                {
                    return plugInInfo.Description; ;
                }
                else
                {
                    return "";
                }
                
            }
            /// <summary>
            /// Executes the plugin functionality.
            /// </summary>
            public string execute(string text, IWin32Window ownerWindow)
            {
                return this.typeInstance.Execute(text, ownerWindow);
            }
            #endregion Public Methods

            #region Static Methods
            public static PlugInWrapper CreatePlugInType(Type type)
            {
                if (PlugInWrapper.isPlugInCompliant(type))
                {
                    return new PlugInWrapper(type);
                }
                else
                {
                    return (PlugInWrapper)null;
                }
            }

            private static bool isPlugInCompliant(Type plugIn)
            {
                if (plugIn.IsClass && typeof(IOperation).IsAssignableFrom(plugIn))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            #endregion Static Methods

            #region Overrides
            /// <summary>
            /// Extends the life of the object. (otherwise is distroyed after 5 minutes.
            /// </summary>
            public override Object InitializeLifetimeService()
            {
                return null;
            }
            #endregion Overrides

        }

       

        #endregion Nested Types

        #region Constants
        /// <summary>
        /// The subdirectory of the application installation directory in
        /// which the DLL files that (potentially) contain Operations are
        /// assumed to be located. This is the directory which should be 
        /// searched when looking for Operations.
        /// </summary>
        private const string OperationsSubDirectory = @"\Operations";

        /// <summary>
        /// Search Operation to find only DLL files
        /// </summary>
        private const string DllFiles = "*.dll";

        /// <summary>
        /// The interface which defines Scribbler text Operations
        /// </summary>
        private const string OperationInterfaceType = "Scribbler.Types.IOperation";
        #endregion

        #region Fields
        /// <summary>
        /// The full path to the directory which contains Operations DLL's
        /// </summary>
        private string OperationsDirectory = Environment.CurrentDirectory + OperationsSubDirectory;

        /// <summary>
        /// List of Remoted AppDomains
        /// </summary>
        private PlugInsAppDomain[] remoteAssemblies;

        /// <summary>
        /// List of all remoted controls for all plugins from all appdomains.
        /// </summary>
        public RemotePlugInController[] plugInControllers;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationManager"/> class.
        /// </summary>
        /// <param name="menuManager">The menu manager.</param>
        internal OperationManager(MenuManager menuManager)
        {
            LoadOperations(menuManager);
        }
        #endregion

        #region Properties
        #endregion

        #region Private methods
        /// <summary>
        /// Loads any available Operations from the Operations subdirectory of the
        /// application installation directory
        /// </summary>
        /// <param name="menuManager">The menu manager.</param>
        private void LoadOperations(MenuManager menuManager)
        {

            if (Directory.Exists(this.OperationsDirectory))
            {
                MainForm mainForm = menuManager.Parent;
                DirectoryInfo operationsDir = new DirectoryInfo(this.OperationsDirectory);
                FileInfo[] files = operationsDir.GetFiles(OperationManager.DllFiles, SearchOption.TopDirectoryOnly);

                List<PlugInsAppDomain> remoteAssemblies = new List<PlugInsAppDomain>();
                List<RemotePlugInController> plugInControllers = new List<RemotePlugInController>();

                foreach (FileInfo file in files)
                {
                    AssemblyName assblyRef;
                    if (OperationManager.IsAssembly(file, out assblyRef))
                    {
                        PlugInsAppDomain newPlugInsAppDomain = new PlugInsAppDomain(assblyRef);
                        plugInControllers.AddRange(newPlugInsAppDomain.plugInControllers);
                        remoteAssemblies.Add(newPlugInsAppDomain);
                    }
                    else
                    {
                        continue;
                    }
                }
                this.remoteAssemblies = remoteAssemblies.ToArray();
                this.plugInControllers = plugInControllers.ToArray();
            }

        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Checks if a DLL/EXE file is a .NET DLL/EXE file.
        /// </summary>
        /// <param name="file">The <see cref="System.IO.FileInfo"/>which has to be checked.</param>
        /// <returns>Returns true if is a .NET DLL/EXE , false otherwise.</returns>
        private static bool IsAssembly(FileInfo file, out AssemblyName assblyRef)
        {
            try
            {
                assblyRef = AssemblyName.GetAssemblyName(file.FullName);
                return true;
            }
            catch
            {
                assblyRef = null;
                return false;
            }
        }
        #endregion StaticMethods

        #region System.Object overrides
        /// <summary>
        /// Determines whether the specified <see cref="System.Object">Object</see> is equal to the current <see cref="System.Object">Object</see>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object">Object</see> to compare with the current <see cref="System.Object">Object</see>.</param>
        /// <returns><b>true</b> if the specified <see cref="System.Object">Object</see> is equal to the current <see cref="System.Object">Object</see>; otherwise, <b>false</b>.</returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Serves as a hash function for this type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="System.Object">Object</see>.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String">String</see> that represents the current <see cref="System.Object">Object</see>.
        /// </summary>
        /// <returns>A <see cref="System.String">String</see> that represents the current <see cref="System.Object">Object</see>.</returns>
        public override string ToString()
        {
            return base.ToString();
        }
        #endregion

        #region Internal methods
        #endregion

    }
}
