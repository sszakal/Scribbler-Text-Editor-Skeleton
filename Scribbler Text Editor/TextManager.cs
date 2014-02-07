//-----------------------------------------------------------------------
// <copyright file="ImageManager.cs" company="University of Hull">
//     Copyright (c) University of Hull. All rights reserved.
// 
// This code remains the property of University of Hull. No portion of this 
// code may be reproduced without the expression written permission of 
// University of Hull.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Scribbler.Types;

namespace Scribbler
{
    #region Using directives
    using System;
    #endregion
    /// <summary>
    /// Manages the image manipulated by the Scribbler application
    /// </summary>
    internal class TextManager
    {
        #region Constants
        /// <summary>
        /// The file Operation used to restrict which files are displayed in the Open File dialog
        /// </summary>
        private const string FileOperation = "TXT files (*.txt)|*.txt|All files (*.*)|*.*";
        #endregion

        #region Fields
        #endregion

        #region Constructors
        private TextManager()
        {
            // Private constructor to prevent instantiation
        }
        #endregion

        #region Properties
        #endregion

        #region Private methods
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
        /// <summary>
        /// Displays an open file dialog to allow the user to select the text
        /// file to manipulate
        /// </summary>
        /// <returns>A <see cref="String"/> containing the text</returns>
        internal static String OpenTextFile()
        {
            string sourceText = string.Empty; 
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = FileOperation;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamReader reader = new StreamReader(File.Open(dlg.FileName, FileMode.Open)))
                    {
                        sourceText = reader.ReadToEnd();
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Error: " + ex.Message);
                    throw new FileNotFoundException();
                }
            }

            return sourceText;
        }

        /// <summary>
        /// Displays a save file dialog to allow the user to save the text currently 
        /// in the editor to a file 
        /// </summary>
        /// <returns><c>true</c> if the file was saved successfully, otherwise <c>false</c></returns>
        internal static bool SaveTextFile(string sourceText)
        {
            bool success = false;
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = FileOperation;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(File.Create(dlg.FileName)))
                    {
                        writer.Write(sourceText);
                        writer.Close();
                        success = true;
                    }
                }
                catch
                {
                }
            }

            return success;
        }
        
        #endregion

    }
}
