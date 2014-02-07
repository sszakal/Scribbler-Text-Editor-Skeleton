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
    using Scribbler;
    using Google.API.Translate;
    #endregion
    /// <summary>
    /// Base class from which all translation operations are derived.
    /// This class provides functionality to keep track of the language
    /// in which the text of the document is currently expressed (the 
    /// Google Translate API requires us to specify the source language 
    /// from which we are translating)
    /// </summary>
    public abstract class TranslationOperation: OperationBase
    {
        #region Constants
        #endregion

        #region Fields
        private static Language lastLanguage = Language.English;
        #endregion

        #region Constructors
        #endregion

        #region Properties
        protected static Language LastLanguageUsed 
        {
            get
            {
                return lastLanguage;
            }
            set
            {
                lastLanguage = value;
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
    }
}
