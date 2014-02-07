//-----------------------------------------------------------------------
// <copyright file="IPlugin.cs" company="University of Hull">
//     Copyright (c) University of Hull. All rights reserved.
// 
// This code remains the property of University of Hull. No portion of this 
// code may be reproduced without the expression written permission of 
// University of Hull.
// </copyright>
//-----------------------------------------------------------------------

using System.IO;

namespace Scribbler
{
    #region Using directives
    using System.Drawing;
    using System.Windows.Forms;
    #endregion
    /// <summary>
    /// The main window of the Scribbler application
    /// </summary>
    public partial class MainForm : Form
    {
        #region Constants
        #endregion

        #region Fields
        /// <summary>
        /// The MenuManager instance which handles all menu operations for the application
        /// </summary>
        private MenuManager menuManager;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            ConnectEventHandlers();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the <see cref="T:System.Windows.Forms.MainMenu"/> that is displayed in the form.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// A <see cref="T:System.Windows.Forms.MainMenu"/> that represents the menu to display in the form.
        /// </returns>
        /// <PermissionSet>
        /// 	<IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
        /// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
        /// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/>
        /// 	<IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
        /// </PermissionSet>
        internal new MenuStrip Menu
        {
            get
            {
                return menuBar;
            }
        }

        internal ToolStripMenuItem editMenuItem
        {
            get
            {
                return this.editToolStripMenuItem;
            }
        }

        /// <summary>
        /// Gets or sets the text manipulated by Scribbler.
        /// </summary>
        /// <value>A <see cref="String"/> instance containing the text.</value>
        internal string EditableText
        {
            get
            {
                return editTextBox.Text;
            }
            set
            {
                editTextBox.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the text displayed in the status bar
        /// </summary>
        /// <value>The status text</value>
        internal string Status
        {
            get
            {
                return statusBar.Text;
            }

            set
            {
                statusBar.Text = value;
                statusBar.Refresh();
            }
        }
        #endregion

        #region Internal Methods
        #endregion
        #region Private Methods
        /// <summary>
        /// Connects the event handlers used to process menu commands
        /// </summary>
        private void ConnectEventHandlers()
        {
            menuManager = new MenuManager(this);

            openFileMenuItem.Click += menuManager.OpenFileMenuItem_Click;
            exitToolStripMenuItem.Click += menuManager.ExitMenuItem_Click;
            newToolStripMenuItem.Click += menuManager.NewFileMenuItem_Click;
            saveToolStripMenuItem.Click += menuManager.SaveFileMenuItem_Click;
            giveMeLoadedAssembliesToolStripMenuItem.Click += menuManager.GiveMeLoadedAssembliesMenuItem_Click;
        }
        #endregion
    }
}
