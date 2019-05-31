﻿//Copyright (c) Microsoft Corporation.  All rights reserved.

using Microsoft.WindowsAPICodePack.Win32Native;
using Microsoft.WindowsAPICodePack.Win32Native.Core;
using System.Security.Permissions;
namespace MS.WindowsAPICodePack.Internal
{
    /// <summary>
    /// Safe Window Handle
    /// </summary>
    public class SafeWindowHandle : ZeroInvalidHandle
    {
        /// <summary>
        /// Release the handle
        /// </summary>
        /// <returns>true if handled is release successfully, false otherwise</returns>
        protected override bool ReleaseHandle() => IsInvalid ? true : CoreNativeMethods.DestroyWindow(handle) != 0;
    }
}
