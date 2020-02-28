﻿//Copyright (c) Microsoft Corporation.  All rights reserved.

using Microsoft.WindowsAPICodePack.Win32Native;
using System.Security.Permissions;
namespace Microsoft.WindowsAPICodePack
{
    /// <summary>
    /// Safe Region Handle
    /// </summary>
    public class SafeRegionHandle : ZeroInvalidHandle
    {
        /// <summary>
        /// Release the handle
        /// </summary>
        /// <returns>true if handled is release successfully, false otherwise</returns>
        protected override bool ReleaseHandle() => CoreNativeMethods.DeleteObject(handle);
    }
}
