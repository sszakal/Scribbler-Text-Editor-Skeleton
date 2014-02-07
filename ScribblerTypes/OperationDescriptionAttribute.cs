//-----------------------------------------------------------------------
// <copyright file="EffectDescriptionAttribute.cs" company="University of Hull">
//     Copyright (c) Codefounders Ltd. All rights reserved.
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
    #endregion
    /// <summary>
    /// Metadata attribute which can be used to add descriptive metadata
    /// to a Scribbler text Operation
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class OperationDescriptionAttribute: System.Attribute
    {
        #region Constants
        #endregion

        #region Fields
        /// <summary>
        /// The description
        /// </summary>
        private string description;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationDescriptionAttribute"/> class.
        /// </summary>
        /// <param name="description">The description of the effect.</param>
        public OperationDescriptionAttribute(string description)
        {
            this.description = description;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public String Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the menu group under which the 
        /// Operation command should be displayed
        /// </summary>
        public String MenuName { get; set; }

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
    }
}
