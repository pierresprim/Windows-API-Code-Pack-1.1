﻿using System.IO;

using Microsoft.WindowsAPICodePack.Shell;

using FileInfo = System.IO.FileInfo;

namespace Microsoft.WindowsAPICodePack.ShellExtensions
{
    /// <summary>
    /// This interface exposes the <see cref="Load"/> function for initializing the 
    /// Preview Handler with a <see cref="Stream"/>.
    /// This interface can be used in conjunction with the other intialization interfaces,
    /// but only 1 will be accessed according to the priorities preset by the Windows Shell:
    /// <seealso cref="IPreviewFromStream"/>
    /// <seealso cref="IPreviewFromShellObject"/>
    /// <seealso cref="IPreviewFromFile"/>
    /// </summary>
    public interface IPreviewFromStream
    {
        /// <summary>
        /// Provides the <see cref="Stream"/> to the item from which a preview should be created.        
        /// </summary>
        /// <param name="stream">Stream to the previewed file, this stream is only available in the scope of this method.</param>
        void Load(in Stream stream);
    }

    /// <summary>
    /// This interface exposes the <see cref="Load"/> function for initializing the 
    /// Preview Handler with a <see cref="FileInfo"/>.
    /// This interface can be used in conjunction with the other intialization interfaces,
    /// but only 1 will be accessed according to the priorities preset by the Windows Shell:
    /// <seealso cref="IPreviewFromStream"/>
    /// <seealso cref="IPreviewFromShellObject"/>
    /// <seealso cref="IPreviewFromFile"/>
    /// </summary>
    public interface IPreviewFromFile
    {
        /// <summary>
        /// Provides the <see cref="FileInfo"/> to the item from which a preview should be created.        
        /// </summary>
        /// <param name="info">File information to the previewed file.</param>
        void Load(in FileInfo info);
    }

    /// <summary>
    /// This interface exposes the <see cref="Load"/> function for initializing the 
    /// Preview Handler with a <see cref="ShellObject"/>.
    /// This interface can be used in conjunction with the other intialization interfaces,
    /// but only 1 will be accessed according to the priorities preset by the Windows Shell:
    /// <seealso cref="IPreviewFromStream"/>
    /// <seealso cref="IPreviewFromShellObject"/>
    /// <seealso cref="IPreviewFromFile"/>
    /// </summary>
    public interface IPreviewFromShellObject
    {
        /// <summary>
        /// Provides the <see cref="ShellObject"/> from which a preview should be created.        
        /// </summary>
        /// <param name="shellObject">ShellObject for the previewed file, this ShellObject is only available in the scope of this method.</param>
        void Load(in ShellObject shellObject);
    }
}
