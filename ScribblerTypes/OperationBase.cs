//-----------------------------------------------------------------------
// <copyright file="Effect.cs" company="University of Hull">
//     Copyright (c) University of Hull. All rights reserved.
// 
// This code remains the property of University of Hull. No portion of this 
// code may be reproduced without the expression written permission of 
// University of Hull.
// </copyright>
//-----------------------------------------------------------------------

namespace Scribbler
{
    #region Using directives
    using System.Windows.Forms;
    using Scribbler.Types;
    #endregion
    /// <summary>
    /// Base class from which all effects can be derived
    /// </summary>
    public class OperationBase : IOperation
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

        #region IEffect Members

        /// <summary>
        /// Gets the name of the Operation which can be rendered on the menus
        /// </summary>
        /// <value>The effect name.</value>
        public virtual string Name
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Applies the Operation to the supplied text.
        /// </summary>
        /// <param name="image">A <see cref="String"/> instance containing the
        /// text displayed in the Scribbler word processor. The effect is applied to this text</param>
        /// <param name="ownerWindow">An <see cref="IWin32Window"/> instance that 
        /// represents the top-level window which will own any dialog box that the 
        /// Operation chooses to display</param>
        /// <returns>The modified text</returns>
        public virtual string Execute(string text, IWin32Window ownerWindow)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
