//-----------------------------------------------------------------------
// <copyright file="MenuManager.cs" company="University of Hull">
//     Copyright (c) University of Hull. All rights reserved.
// 
// This code remains the property of University of Hull. No portion of this 
// code may be reproduced without the expression written permission of 
// University of Hull.
// </copyright>
//-----------------------------------------------------------------------

using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Scribbler
{
    #region Using directives
    using System;
    #endregion
    /// <summary>
    /// Provides handlers for all of the events associated with menu items in the
    /// Scribbler application
    /// </summary>
    /// 
    internal class MenuManager
    {
        #region Constants

        /// <summary>
        /// The name of the Edit menu
        /// </summary>
        private const string EditMenuName = "editToolStripMenuItem";
        #endregion

        #region Fields
        /// <summary>
        /// the main form.
        /// </summary>
        private MainForm parent;

        /// <summary>
        /// Plugin Manager
        /// </summary>
        private OperationManager OperationManager;

        /// <summary>
        /// List of plugin menus.
        /// </summary>
        private LinkedList<PlugInMenu> plugInMenus;
        #endregion

        #region Nested Types

        /// <summary>
        /// Remote Controll wrapper, which implements the menu event handling and the manu item.
        /// </summary>
        internal class PlugInMenu
        {
            /// <summary>
            /// The item from the plugins menu.
            /// </summary>
            private ToolStripMenuItem plugInMenuInPlugins;

            /// <summary>
            /// The plugin menu item.
            /// </summary>
            private ToolStripMenuItem plugInMenu;

            /// <summary>
            /// the remote control for the plugin
            /// </summary>
            private OperationManager.RemotePlugInController plugIn;

            /// <summary>
            /// The main form, where the plugin is added.
            /// </summary>
            private MainForm appForm;

            /// <summary>
            /// Constructor.
            /// </summary>
            internal PlugInMenu(OperationManager.RemotePlugInController plugIn, MainForm appForm)
            {
                this.plugIn = plugIn;
                this.appForm = appForm;

                this.plugInMenu = new ToolStripMenuItem(plugIn.plugInName);
                this.plugInMenu.Click += this.handlerPlugIn;
                this.plugInMenu.ToolTipText = plugIn.plugInDesc;

                this.plugInMenuInPlugins = new ToolStripMenuItem();
                this.plugInMenuInPlugins.Click += this.handlerPlugInManager;
                if (plugIn.active)
                {
                    this.plugInMenuInPlugins.Text = "Disable " + plugIn.plugInName;
                }
                else
                {
                    this.plugInMenuInPlugins.Text = "Enable " + plugIn.plugInName;
                }
                ToolStripMenuItem searchPlugInManagerMenu = null;
                ToolStripMenuItem searchPlugInMenu = null;
                foreach (ToolStripMenuItem item in appForm.Menu.Items)
                {
                    if(item.Text == "&PlugIns")
                    {
                        searchPlugInManagerMenu = item;
                    }
                    else if ((item.Text == plugIn.plugInMenuName))
                    {
                        searchPlugInMenu = item;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (searchPlugInManagerMenu != null)
                {
                    searchPlugInManagerMenu.DropDownItems.Add(plugInMenuInPlugins);
                }
                else
                {
                    throw new Exception("PlugIn Menu Manager Not FOUND!");
                }
                if (searchPlugInMenu != null)
                {
                    searchPlugInMenu.DropDownItems.Add(plugInMenu);
                }
                else
                {
                    searchPlugInMenu = new ToolStripMenuItem(plugIn.plugInMenuName);
                    appForm.Menu.Items.Add(searchPlugInMenu);
                    searchPlugInMenu.DropDownItems.Add(plugInMenu);
                }
            }

            /// <summary>
            /// Handler for the plugin manager click (Enable/Disable Plugin).
            /// </summary>
            private void handlerPlugInManager(object o, EventArgs args)
            {
                try
                {
                    if (this.plugIn.active)
                    {
                        this.plugInMenu.Enabled = false;
                        this.plugIn.active = false;
                        this.plugInMenuInPlugins.Text = "Enable " + plugIn.plugInName;
                    }
                    else
                    {
                        this.plugInMenu.Enabled = true;
                        this.plugIn.active = true;
                        this.plugInMenuInPlugins.Text = "Disable " + plugIn.plugInName;
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            /// <summary>
            /// Handler for the plugin menu item click (executes the plugin functionality).
            /// </summary>
            private void handlerPlugIn(object o, EventArgs args)
            {
                this.plugIn.Execute(this.appForm.EditableText, this.appForm);
            }
        }
        #endregion Nested Types

        #region Constructors
        internal MenuManager(MainForm appForm)
        {
            try
            {
                parent = appForm;
                OperationManager = new OperationManager(this);
                plugInMenus = new LinkedList<PlugInMenu>();

                foreach (OperationManager.RemotePlugInController plugIn in OperationManager.plugInControllers)
                {
                    this.plugInMenus.AddLast(new PlugInMenu(plugIn, appForm));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the main form of the application
        /// </summary>
        /// <value>The parent.</value>
        internal MainForm Parent
        {
            get
            {
                return parent;
            }
        }
        #endregion

        #region Public methods
        #endregion

        #region Non-public methods
        #endregion

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

        #region Menu event handlers
        /// <summary>
        /// Handles the Click event of the File->Open menu item
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        internal void OpenFileMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                parent.EditableText = TextManager.OpenTextFile();
            }
            catch (FileNotFoundException)
            {
                // Ignore the fact that we can't open the text file
            }
        }

        /// <summary>
        /// Handles the Click event of the File->New menu item
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        internal void NewFileMenuItem_Click(object sender, EventArgs e)
        {
            parent.EditableText = String.Empty;
        }

        /// <summary>
        /// Handles the Click event of the File->Save menu item
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        internal void SaveFileMenuItem_Click(object sender, EventArgs e)
        {
            bool success = TextManager.SaveTextFile(parent.EditableText);
            if (success)
            {
                parent.Status = "File saved";
            }
            else
            {
                parent.Status = "Error - file could not be saved";
            }
        }
        /// <summary>
        /// Handles the Click event of the File->Exit menu item
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        internal void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Handles the Click event of the PLugIns->Give Me Loaded Assemblies menu item
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        internal void GiveMeLoadedAssembliesMenuItem_Click(object sender, EventArgs e)
        {
            System.Reflection.Assembly[] asmList = AppDomain.CurrentDomain.GetAssemblies();
            string list = "";
            foreach (System.Reflection.Assembly asm in asmList)
            {
                list += (asm.FullName + "\n");
            }
            MessageBox.Show(list);
        }

        #endregion

    }
}
