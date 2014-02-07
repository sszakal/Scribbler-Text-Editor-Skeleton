//-----------------------------------------------------------------------
// <copyright file="IEffect.cs" company="University of Hull">
//     Copyright (c) University of Hull. All rights reserved.
// 
// This code remains the property of University of Hull. No portion of this 
// code may be reproduced without the expression written permission of 
// University of Hull.
// </copyright>
//-----------------------------------------------------------------------

namespace Scribbler.Types
{
    #region Using directives
    using System;
    using System.Windows.Forms;
    #endregion

    /// <summary>
    /// Interface which defines the capabilities of a Scribbler text Operation 
    /// </summary>
    public interface IOperation
    {
        /// <summary>
        /// Gets the name of the Operation which can be rendered on the menus
        /// </summary>
        /// <value>The effect name.</value>
        string Name { get; }

        /// <summary>
        /// Applies the Operation to the supplied text.
        /// </summary>
        /// <param name="image">A <see cref="String"/> instance containing the
        /// text displayed in the Scribbler word processor. The effect is applied to this text</param>
        /// <param name="ownerWindow">An <see cref="IWin32Window"/> instance that 
        /// represents the top-level window which will own any dialog box that the 
        /// Operation chooses to display</param>
        /// <returns>The modified text</returns>
        String Execute(String text, IWin32Window ownerWindow);
    }
}
