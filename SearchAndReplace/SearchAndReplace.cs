//-----------------------------------------------------------------------
// <copyright file="MonochromeEffect.cs" company="University of Hull">
//     Copyright (c) Codefounders Ltd. All rights reserved.
// 
// This code remains the property of University of Hull. No portion of this 
// code may be reproduced without the expression written permission of 
// University of Hull.
// </copyright>
//-----------------------------------------------------------------------

namespace Operations.Search
{
    #region Using directives
    using System;
    using System.Windows.Forms;
    using Scribbler;
    using Scribbler.Types;
    #endregion
    /// <summary>
    /// Implements an Operation which performs search and replace on the text
    /// </summary>
    [OperationDescription("Replaces all instances of the specified text")]
    public class SearchAndReplaceOperation: OperationBase
    {
        #region Constants
        #endregion

        #region Fields
        #endregion

        #region Constructors
        #endregion

        #region Properties
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

        #region OperationBase overrides
        /// <summary>
        /// Gets the name of the Operation which can be rendered on the menus
        /// </summary>
        /// <value>The effect name.</value>
        public override string Name
        {
            get
            {
                return "Replace";
            }
        }

        /// <summary>
        /// Applies the Operation to the specified text.
        /// </summary>
        /// <param name="sourceText">A <see cref="String"/> instance containing the
        /// text displayed in Scribbler. The Operation is applied to this image</param>
        /// <param name="ownerWindow">An <see cref="IWin32Window"/> instance that 
        /// represents the top-level window which will own any dialog box that the 
        /// Operation chooses to display</param>
        public override string Execute(string sourceText, IWin32Window ownerWindow)
        {
            string editedText = sourceText;
            // Show the search and replace dialog
            ReplaceDialog dlg = new ReplaceDialog();
            DialogResult rslt = dlg.ShowDialog(ownerWindow); ;

            if (rslt == DialogResult.OK && !String.IsNullOrEmpty(dlg.SearchTerm))
            {
                // Perform the search and replace
                editedText = sourceText.Replace(dlg.SearchTerm, dlg.ReplaceTerm);
            }

            return editedText;
        }
        #endregion
    }
}
