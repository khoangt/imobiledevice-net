// <copyright file="AfcApi.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Afc
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public partial class AfcApi : IAfcApi
    {
        
        /// <summary>
        /// Makes a connection to the AFC service on the device.
        /// </summary>
        /// <param name="device">
        /// The device to connect to.
        /// </param>
        /// <param name="service">
        /// The service descriptor returned by lockdownd_start_service.
        /// </param>
        /// <param name="client">
        /// Pointer that will be set to a newly allocated afc_client_t
        /// upon successful return.
        /// </param>
        /// <returns>
        /// AFC_E_SUCCESS on success, AFC_E_INVALID_ARG if device or service is
        /// invalid, AFC_E_MUX_ERROR if the connection cannot be established,
        /// or AFC_E_NO_MEM if there is a memory allocation problem.
        /// </returns>
        public virtual AfcError afc_client_new(iDeviceHandle device, LockdownServiceDescriptorHandle service, out AfcClientHandle client)
        {
            return AfcNativeMethods.afc_client_new(device, service, out client);
        }
        
        /// <summary>
        /// Starts a new AFC service on the specified device and connects to it.
        /// </summary>
        /// <param name="device">
        /// The device to connect to.
        /// </param>
        /// <param name="client">
        /// Pointer that will point to a newly allocated afc_client_t upon
        /// successful return. Must be freed using afc_client_free() after use.
        /// </param>
        /// <param name="label">
        /// The label to use for communication. Usually the program name.
        /// Pass NULL to disable sending the label in requests to lockdownd.
        /// </param>
        /// <returns>
        /// AFC_E_SUCCESS on success, or an AFC_E_* error code otherwise.
        /// </returns>
        public virtual AfcError afc_client_start_service(iDeviceHandle device, out AfcClientHandle client, string label)
        {
            return AfcNativeMethods.afc_client_start_service(device, out client, label);
        }
        
        /// <summary>
        /// Frees up an AFC client. If the connection was created by the client itself,
        /// the connection will be closed.
        /// </summary>
        /// <param name="client">
        /// The client to free.
        /// </param>
        public virtual AfcError afc_client_free(System.IntPtr client)
        {
            return AfcNativeMethods.afc_client_free(client);
        }
        
        /// <summary>
        /// Get device information for a connected client. The device information
        /// returned is the device model as well as the free space, the total capacity
        /// and blocksize on the accessed disk partition.
        /// </summary>
        /// <param name="client">
        /// The client to get device info for.
        /// </param>
        /// <param name="device_information">
        /// A char list of device information terminated by an
        /// empty string or NULL if there was an error. Free with
        /// afc_dictionary_free().
        /// </param>
        /// <returns>
        /// AFC_E_SUCCESS on success or an AFC_E_* error value.
        /// </returns>
        public virtual AfcError afc_get_device_info(AfcClientHandle client, out System.Collections.ObjectModel.ReadOnlyCollection<string> deviceInformation)
        {
            return AfcNativeMethods.afc_get_device_info(client, out deviceInformation);
        }
        
        /// <summary>
        /// Gets a directory listing of the directory requested.
        /// </summary>
        /// <param name="client">
        /// The client to get a directory listing from.
        /// </param>
        /// <param name="path">
        /// The directory for listing. (must be a fully-qualified path)
        /// </param>
        /// <param name="directory_information">
        /// A char list of files in the directory
        /// terminated by an empty string or NULL if there was an error. Free with
        /// afc_dictionary_free().
        /// </param>
        /// <returns>
        /// AFC_E_SUCCESS on success or an AFC_E_* error value.
        /// </returns>
        public virtual AfcError afc_read_directory(AfcClientHandle client, string path, out System.Collections.ObjectModel.ReadOnlyCollection<string> directoryInformation)
        {
            return AfcNativeMethods.afc_read_directory(client, path, out directoryInformation);
        }
        
        /// <summary>
        /// Gets information about a specific file.
        /// </summary>
        /// <param name="client">
        /// The client to use to get the information of the file.
        /// </param>
        /// <param name="filename">
        /// The fully-qualified path to the file.
        /// </param>
        /// <param name="file_information">
        /// Pointer to a buffer that will be filled with a
        /// NULL-terminated list of strings with the file information. Set to NULL
        /// before calling this function. Free with afc_dictionary_free().
        /// </param>
        /// <returns>
        /// AFC_E_SUCCESS on success or an AFC_E_* error value.
        /// </returns>
        public virtual AfcError afc_get_file_info(AfcClientHandle client, string filename, out System.Collections.ObjectModel.ReadOnlyCollection<string> fileInformation)
        {
            return AfcNativeMethods.afc_get_file_info(client, filename, out fileInformation);
        }
        
        /// <summary>
        /// Opens a file on the device.
        /// </summary>
        /// <param name="client">
        /// The client to use to open the file.
        /// </param>
        /// <param name="filename">
        /// The file to open. (must be a fully-qualified path)
        /// </param>
        /// <param name="file_mode">
        /// The mode to use to open the file.
        /// </param>
        /// <param name="handle">
        /// Pointer to a uint64_t that will hold the handle of the file
        /// </param>
        /// <returns>
        /// AFC_E_SUCCESS on success or an AFC_E_* error value.
        /// </returns>
        public virtual AfcError afc_file_open(AfcClientHandle client, string filename, AfcFileMode fileMode, ref ulong handle)
        {
            return AfcNativeMethods.afc_file_open(client, filename, fileMode, ref handle);
        }
        
        /// <summary>
        /// Closes a file on the device.
        /// </summary>
        /// <param name="client">
        /// The client to close the file with.
        /// </param>
        /// <param name="handle">
        /// File handle of a previously opened file.
        /// </param>
        public virtual AfcError afc_file_close(AfcClientHandle client, ulong handle)
        {
            return AfcNativeMethods.afc_file_close(client, handle);
        }
        
        /// <summary>
        /// Locks or unlocks a file on the device.
        /// Makes use of flock on the device.
        /// </summary>
        /// <param name="client">
        /// The client to lock the file with.
        /// </param>
        /// <param name="handle">
        /// File handle of a previously opened file.
        /// </param>
        /// <param name="operation">
        /// the lock or unlock operation to perform, this is one of
        /// AFC_LOCK_SH (shared lock), AFC_LOCK_EX (exclusive lock), or
        /// AFC_LOCK_UN (unlock).
        /// </param>
        public virtual AfcError afc_file_lock(AfcClientHandle client, ulong handle, AfcLockOp operation)
        {
            return AfcNativeMethods.afc_file_lock(client, handle, operation);
        }
        
        /// <summary>
        /// Attempts to the read the given number of bytes from the given file.
        /// </summary>
        /// <param name="client">
        /// The relevant AFC client
        /// </param>
        /// <param name="handle">
        /// File handle of a previously opened file
        /// </param>
        /// <param name="data">
        /// The pointer to the memory region to store the read data
        /// </param>
        /// <param name="length">
        /// The number of bytes to read
        /// </param>
        /// <param name="bytes_read">
        /// The number of bytes actually read.
        /// </param>
        /// <returns>
        /// AFC_E_SUCCESS on success or an AFC_E_* error value.
        /// </returns>
        public virtual AfcError afc_file_read(AfcClientHandle client, ulong handle, byte[] data, uint length, ref uint bytesRead)
        {
            return AfcNativeMethods.afc_file_read(client, handle, data, length, ref bytesRead);
        }
        
        /// <summary>
        /// Writes a given number of bytes to a file.
        /// </summary>
        /// <param name="client">
        /// The client to use to write to the file.
        /// </param>
        /// <param name="handle">
        /// File handle of previously opened file.
        /// </param>
        /// <param name="data">
        /// The data to write to the file.
        /// </param>
        /// <param name="length">
        /// How much data to write.
        /// </param>
        /// <param name="bytes_written">
        /// The number of bytes actually written to the file.
        /// </param>
        /// <returns>
        /// AFC_E_SUCCESS on success or an AFC_E_* error value.
        /// </returns>
        public virtual AfcError afc_file_write(AfcClientHandle client, ulong handle, byte[] data, uint length, ref uint bytesWritten)
        {
            return AfcNativeMethods.afc_file_write(client, handle, data, length, ref bytesWritten);
        }
        
        /// <summary>
        /// Seeks to a given position of a pre-opened file on the device.
        /// </summary>
        /// <param name="client">
        /// The client to use to seek to the position.
        /// </param>
        /// <param name="handle">
        /// File handle of a previously opened.
        /// </param>
        /// <param name="offset">
        /// Seek offset.
        /// </param>
        /// <param name="whence">
        /// Seeking direction, one of SEEK_SET, SEEK_CUR, or SEEK_END.
        /// </param>
        /// <returns>
        /// AFC_E_SUCCESS on success or an AFC_E_* error value.
        /// </returns>
        public virtual AfcError afc_file_seek(AfcClientHandle client, ulong handle, long offset, int whence)
        {
            return AfcNativeMethods.afc_file_seek(client, handle, offset, whence);
        }
        
        /// <summary>
        /// Returns current position in a pre-opened file on the device.
        /// </summary>
        /// <param name="client">
        /// The client to use.
        /// </param>
        /// <param name="handle">
        /// File handle of a previously opened file.
        /// </param>
        /// <param name="position">
        /// Position in bytes of indicator
        /// </param>
        /// <returns>
        /// AFC_E_SUCCESS on success or an AFC_E_* error value.
        /// </returns>
        public virtual AfcError afc_file_tell(AfcClientHandle client, ulong handle, ref ulong position)
        {
            return AfcNativeMethods.afc_file_tell(client, handle, ref position);
        }
        
        /// <summary>
        /// Sets the size of a file on the device.
        /// </summary>
        /// <param name="client">
        /// The client to use to set the file size.
        /// </param>
        /// <param name="handle">
        /// File handle of a previously opened file.
        /// </param>
        /// <param name="newsize">
        /// The size to set the file to.
        /// </param>
        /// <returns>
        /// AFC_E_SUCCESS on success or an AFC_E_* error value.
        /// </returns>
        /// <remarks>
        /// This function is more akin to ftruncate than truncate, and truncate
        /// calls would have to open the file before calling this, sadly.
        /// </remarks>
        public virtual AfcError afc_file_truncate(AfcClientHandle client, ulong handle, ulong newsize)
        {
            return AfcNativeMethods.afc_file_truncate(client, handle, newsize);
        }
        
        /// <summary>
        /// Deletes a file or directory.
        /// </summary>
        /// <param name="client">
        /// The client to use.
        /// </param>
        /// <param name="path">
        /// The path to delete. (must be a fully-qualified path)
        /// </param>
        /// <returns>
        /// AFC_E_SUCCESS on success or an AFC_E_* error value.
        /// </returns>
        public virtual AfcError afc_remove_path(AfcClientHandle client, string path)
        {
            return AfcNativeMethods.afc_remove_path(client, path);
        }
        
        /// <summary>
        /// Renames a file or directory on the device.
        /// </summary>
        /// <param name="client">
        /// The client to have rename.
        /// </param>
        /// <param name="from">
        /// The name to rename from. (must be a fully-qualified path)
        /// </param>
        /// <param name="to">
        /// The new name. (must also be a fully-qualified path)
        /// </param>
        /// <returns>
        /// AFC_E_SUCCESS on success or an AFC_E_* error value.
        /// </returns>
        public virtual AfcError afc_rename_path(AfcClientHandle client, string from, string to)
        {
            return AfcNativeMethods.afc_rename_path(client, from, to);
        }
        
        /// <summary>
        /// Creates a directory on the device.
        /// </summary>
        /// <param name="client">
        /// The client to use to make a directory.
        /// </param>
        /// <param name="path">
        /// The directory's path. (must be a fully-qualified path, I assume
        /// all other mkdir restrictions apply as well)
        /// </param>
        /// <returns>
        /// AFC_E_SUCCESS on success or an AFC_E_* error value.
        /// </returns>
        public virtual AfcError afc_make_directory(AfcClientHandle client, string path)
        {
            return AfcNativeMethods.afc_make_directory(client, path);
        }
        
        /// <summary>
        /// Sets the size of a file on the device without prior opening it.
        /// </summary>
        /// <param name="client">
        /// The client to use to set the file size.
        /// </param>
        /// <param name="path">
        /// The path of the file to be truncated.
        /// </param>
        /// <param name="newsize">
        /// The size to set the file to.
        /// </param>
        /// <returns>
        /// AFC_E_SUCCESS on success or an AFC_E_* error value.
        /// </returns>
        public virtual AfcError afc_truncate(AfcClientHandle client, string path, ulong newsize)
        {
            return AfcNativeMethods.afc_truncate(client, path, newsize);
        }
        
        /// <summary>
        /// Creates a hard link or symbolic link on the device.
        /// </summary>
        /// <param name="client">
        /// The client to use for making a link
        /// </param>
        /// <param name="linktype">
        /// 1 = hard link, 2 = symlink
        /// </param>
        /// <param name="target">
        /// The file to be linked.
        /// </param>
        /// <param name="linkname">
        /// The name of link.
        /// </param>
        /// <returns>
        /// AFC_E_SUCCESS on success or an AFC_E_* error value.
        /// </returns>
        public virtual AfcError afc_make_link(AfcClientHandle client, AfcLinkType linktype, string target, string linkname)
        {
            return AfcNativeMethods.afc_make_link(client, linktype, target, linkname);
        }
        
        /// <summary>
        /// Sets the modification time of a file on the device.
        /// </summary>
        /// <param name="client">
        /// The client to use to set the file size.
        /// </param>
        /// <param name="path">
        /// Path of the file for which the modification time should be set.
        /// </param>
        /// <param name="mtime">
        /// The modification time to set in nanoseconds since epoch.
        /// </param>
        /// <returns>
        /// AFC_E_SUCCESS on success or an AFC_E_* error value.
        /// </returns>
        public virtual AfcError afc_set_file_time(AfcClientHandle client, string path, ulong mtime)
        {
            return AfcNativeMethods.afc_set_file_time(client, path, mtime);
        }
        
        /// <summary>
        /// Deletes a file or directory including possible contents.
        /// </summary>
        /// <param name="client">
        /// The client to use.
        /// </param>
        /// <param name="path">
        /// The path to delete. (must be a fully-qualified path)
        /// </param>
        /// <returns>
        /// AFC_E_SUCCESS on success or an AFC_E_* error value.
        /// </returns>
        /// <remarks>
        /// Only available in iOS 6 and later.
        /// </remarks>
        public virtual AfcError afc_remove_path_and_contents(AfcClientHandle client, string path)
        {
            return AfcNativeMethods.afc_remove_path_and_contents(client, path);
        }
        
        /// <summary>
        /// Get a specific key of the device info list for a client connection.
        /// Known key values are: Model, FSTotalBytes, FSFreeBytes and FSBlockSize.
        /// This is a helper function for afc_get_device_info().
        /// </summary>
        /// <param name="client">
        /// The client to get device info for.
        /// </param>
        /// <param name="key">
        /// The key to get the value of.
        /// </param>
        /// <param name="value">
        /// The value for the key if successful or NULL otherwise.
        /// </param>
        /// <returns>
        /// AFC_E_SUCCESS on success or an AFC_E_* error value.
        /// </returns>
        public virtual AfcError afc_get_device_info_key(AfcClientHandle client, string key, out string value)
        {
            return AfcNativeMethods.afc_get_device_info_key(client, key, out value);
        }
        
        /// <summary>
        /// Frees up a char dictionary as returned by some AFC functions.
        /// </summary>
        /// <param name="dictionary">
        /// The char array terminated by an empty string.
        /// </param>
        /// <returns>
        /// AFC_E_SUCCESS on success or an AFC_E_* error value.
        /// </returns>
        public virtual AfcError afc_dictionary_free(System.IntPtr dictionary)
        {
            return AfcNativeMethods.afc_dictionary_free(dictionary);
        }
    }
}
