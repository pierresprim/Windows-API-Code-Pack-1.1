﻿//Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Win32Native.Core;
using Microsoft.WindowsAPICodePack.Win32Native.Shell;

namespace Microsoft.WindowsAPICodePack.Shell
{
    class ShellFolderItems : IEnumerator<ShellObject>
    {
        #region Private Fields

        private IEnumIDList nativeEnumIdList;
        private readonly ShellContainer nativeShellFolder;

        #endregion

        #region Internal Constructor

        internal ShellFolderItems(ShellContainer nativeShellFolder)
        {
            this.nativeShellFolder = nativeShellFolder;

            HResult hr = nativeShellFolder.NativeShellFolder.EnumObjects(
                IntPtr.Zero,
                ShellNativeMethods.ShellFolderEnumerationOptions.Folders | ShellNativeMethods.ShellFolderEnumerationOptions.NonFolders,
                out nativeEnumIdList);

            if (!CoreErrorHelper.Succeeded(hr))

                throw hr == HResult.Canceled ? new System.IO.FileNotFoundException() : throw new ShellException(hr);
        }

        #endregion

        #region IEnumerator<ShellObject> Members

        public ShellObject Current { get; private set; }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (nativeEnumIdList != null)
            {
                _ = Marshal.ReleaseComObject(nativeEnumIdList);
                nativeEnumIdList = null;
            }
        }

        #endregion

        #region IEnumerator Members

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (nativeEnumIdList == null) return false;

            uint itemsRequested = 1;

            HResult hr = nativeEnumIdList.Next(itemsRequested, out IntPtr item, out uint numItemsReturned);

            if (numItemsReturned < itemsRequested || hr != HResult.Ok) return false;

            Current = ShellObjectFactory.Create(item, nativeShellFolder);

            return true;
        }

        public void Reset()
        {
            if (nativeEnumIdList != null)

                nativeEnumIdList.Reset();
        }


        #endregion
    }
}
