﻿//Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.PropertySystem;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using Microsoft.WindowsAPICodePack.Win32Native;
using Microsoft.WindowsAPICodePack.Win32Native.PropertySystem;
using Microsoft.WindowsAPICodePack.Win32Native.Shell;
using Microsoft.WindowsAPICodePack.COMNative.Shell.PropertySystem;
using Microsoft.WindowsAPICodePack.COMNative.Shell;

namespace Microsoft.WindowsAPICodePack.Shell
{
    /// <summary>
    /// Exposes properties and methods for retrieving information about a search condition.
    /// </summary>
    public class SearchCondition : IDisposable
    {
        internal SearchCondition(ICondition nativeSearchCondition)
        {
            NativeSearchCondition = nativeSearchCondition ?? throw new ArgumentNullException(nameof(nativeSearchCondition));

            HResult hr = NativeSearchCondition.GetConditionType(out conditionType);

            if (!CoreErrorHelper.Succeeded(hr))

                throw new ShellException(hr);

            if (ConditionType == SearchConditionType.Leaf)

                using (var propVar = new PropVariant())
                {
                    hr = NativeSearchCondition.GetComparisonInfo(out canonicalName, out conditionOperation, propVar);

                    if (!CoreErrorHelper.Succeeded(hr))

                        throw new ShellException(hr);

                    PropertyValue = propVar.Value.ToString();
                }
        }

        internal ICondition NativeSearchCondition { get; set; }

        private readonly string canonicalName;

        /// <summary>
        /// The name of a property to be compared or NULL for an unspecified property.
        /// </summary>
        public string PropertyCanonicalName => canonicalName;

        private PropertyKey propertyKey;
        private PropertyKey emptyPropertyKey = new PropertyKey();

        /// <summary>
        /// The property key for the property that is to be compared.
        /// </summary>        
        public PropertyKey PropertyKey
        {
            get
            {
                if (propertyKey == emptyPropertyKey)
                {
                    int hr = PropertySystemNativeMethods.PSGetPropertyKeyFromName(PropertyCanonicalName, out propertyKey);

                    if (!CoreErrorHelper.Succeeded(hr))

                        throw new ShellException(hr);
                }

                return propertyKey;
            }
        }

        /// <summary>
        /// A value (in <see cref="string"/> format) to which the property is compared.
        /// </summary>
        public string PropertyValue { get; internal set; }

        private readonly SearchConditionOperation conditionOperation = SearchConditionOperation.Implicit;

        /// <summary>
        /// Search condition operation to be performed on the property/value combination.
        /// See <see cref="SearchConditionOperation"/> for more details.
        /// </summary>        
        public SearchConditionOperation ConditionOperation => conditionOperation;

        private readonly SearchConditionType conditionType = SearchConditionType.Leaf;

        /// <summary>
        /// Represents the condition type for the given node. 
        /// </summary>        
        public SearchConditionType ConditionType => conditionType;

        /// <summary>
        /// Retrieves an array of the sub-conditions. 
        /// </summary>
        public IEnumerable<SearchCondition> GetSubConditions()
        {
            // Our list that we'll return
            var subConditionsList = new List<SearchCondition>();

            // Get the sub-conditions from the native API
            var guid = new Guid(Win32Native.Guids.Shell.IEnumUnknown);

            HResult hr = NativeSearchCondition.GetSubConditions(ref guid, out object subConditionObj);

            if (!CoreErrorHelper.Succeeded(hr))

                throw new ShellException(hr);

            // Convert each ICondition to SearchCondition
            if (subConditionObj != null)
            {
                var enumUnknown = subConditionObj as IEnumUnknown;

                IntPtr buffer = IntPtr.Zero;
                uint fetched = 0;

                while (hr == HResult.Ok)
                {
                    hr = enumUnknown.Next(1, ref buffer, ref fetched);

                    if (hr == HResult.Ok && fetched == 1)

                        subConditionsList.Add(new SearchCondition((ICondition)Marshal.GetObjectForIUnknown(buffer)));
                }
            }

            return subConditionsList;
        }

        #region IDisposable Members

        ~SearchCondition()
        {
            Dispose(false);
        }

        /// <summary>
        /// Release the native objects.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Release the native objects.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (NativeSearchCondition != null)
            {
                _ = Marshal.ReleaseComObject(NativeSearchCondition);
                NativeSearchCondition = null;
            }
        }

        #endregion

    }
}
