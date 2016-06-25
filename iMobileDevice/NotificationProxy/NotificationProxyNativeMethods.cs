// <copyright file="NotificationProxyNativeMethods.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.NotificationProxy
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public partial class NotificationProxyNativeMethods
    {
        
        const string libraryName = "libimobiledevice";
        
        /// <summary>
        /// Connects to the notification_proxy on the specified device.
        /// </summary>
        /// <param name="device">
        /// The device to connect to.
        /// </param>
        /// <param name="service">
        /// The service descriptor returned by lockdownd_start_service.
        /// </param>
        /// <param name="client">
        /// Pointer that will be set to a newly allocated np_client_t
        /// upon successful return.
        /// </param>
        /// <returns>
        /// NP_E_SUCCESS on success, NP_E_INVALID_ARG when device is NULL,
        /// or NP_E_CONN_FAILED when the connection to the device could not be
        /// established.
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(NotificationProxyNativeMethods.libraryName, EntryPoint="np_client_new", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern NotificationProxyError np_client_new(iDeviceHandle device, LockdownServiceDescriptorHandle service, out NotificationProxyClientHandle client);
        
        /// <summary>
        /// Starts a new notification proxy service on the specified device and connects to it.
        /// </summary>
        /// <param name="device">
        /// The device to connect to.
        /// </param>
        /// <param name="client">
        /// Pointer that will point to a newly allocated
        /// np_client_t upon successful return. Must be freed using
        /// np_client_free() after use.
        /// </param>
        /// <param name="label">
        /// The label to use for communication. Usually the program name.
        /// Pass NULL to disable sending the label in requests to lockdownd.
        /// </param>
        /// <returns>
        /// NP_E_SUCCESS on success, or an NP_E_* error
        /// code otherwise.
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(NotificationProxyNativeMethods.libraryName, EntryPoint="np_client_start_service", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern NotificationProxyError np_client_start_service(iDeviceHandle device, out NotificationProxyClientHandle client, [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string label);
        
        /// <summary>
        /// Disconnects a notification_proxy client from the device and frees up the
        /// notification_proxy client data.
        /// </summary>
        /// <param name="client">
        /// The notification_proxy client to disconnect and free.
        /// </param>
        /// <returns>
        /// NP_E_SUCCESS on success, or NP_E_INVALID_ARG when client is NULL.
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(NotificationProxyNativeMethods.libraryName, EntryPoint="np_client_free", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern NotificationProxyError np_client_free(System.IntPtr client);
        
        /// <summary>
        /// Sends a notification to the device's notification_proxy.
        /// </summary>
        /// <param name="client">
        /// The client to send to
        /// </param>
        /// <param name="notification">
        /// The notification message to send
        /// </param>
        /// <returns>
        /// NP_E_SUCCESS on success, or an error returned by np_plist_send
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(NotificationProxyNativeMethods.libraryName, EntryPoint="np_post_notification", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern NotificationProxyError np_post_notification(NotificationProxyClientHandle client, [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string notification);
        
        /// <summary>
        /// Tells the device to send a notification on the specified event.
        /// </summary>
        /// <param name="client">
        /// The client to send to
        /// </param>
        /// <param name="notification">
        /// The notifications that should be observed.
        /// </param>
        /// <returns>
        /// NP_E_SUCCESS on success, NP_E_INVALID_ARG when client or
        /// notification are NULL, or an error returned by np_plist_send.
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(NotificationProxyNativeMethods.libraryName, EntryPoint="np_observe_notification", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern NotificationProxyError np_observe_notification(NotificationProxyClientHandle client, [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string notification);
        
        /// <summary>
        /// Tells the device to send a notification on specified events.
        /// </summary>
        /// <param name="client">
        /// The client to send to
        /// </param>
        /// <param name="notification_spec">
        /// Specification of the notifications that should be
        /// observed. This is expected to be an array of const char* that MUST have a
        /// terminating NULL entry.
        /// </param>
        /// <returns>
        /// NP_E_SUCCESS on success, NP_E_INVALID_ARG when client is null,
        /// or an error returned by np_observe_notification.
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(NotificationProxyNativeMethods.libraryName, EntryPoint="np_observe_notifications", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern NotificationProxyError np_observe_notifications(NotificationProxyClientHandle client, [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(NativeStringMarshaler))] out string notificationSpec);
        
        /// <summary>
        /// This function allows an application to define a callback function that will
        /// be called when a notification has been received.
        /// It will start a thread that polls for notifications and calls the callback
        /// function if a notification has been received.
        /// In case of an error condition when polling for notifications - e.g. device
        /// disconnect - the thread will call the callback function with an empty
        /// notification "" and terminate itself.
        /// </summary>
        /// <param name="client">
        /// the NP client
        /// </param>
        /// <param name="notify_cb">
        /// pointer to a callback function or NULL to de-register a
        /// previously set callback function.
        /// </param>
        /// <param name="user_data">
        /// Pointer that will be passed to the callback function as
        /// user data. If notify_cb is NULL, this parameter is ignored.
        /// </param>
        /// <returns>
        /// NP_E_SUCCESS when the callback was successfully registered,
        /// NP_E_INVALID_ARG when client is NULL, or NP_E_UNKNOWN_ERROR when
        /// the callback thread could no be created.
        /// </returns>
        /// <remarks>
        /// Only one callback function can be registered at the same time;
        /// any previously set callback function will be removed automatically.
        /// </remarks>
        [System.Runtime.InteropServices.DllImportAttribute(NotificationProxyNativeMethods.libraryName, EntryPoint="np_set_notify_callback", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern NotificationProxyError np_set_notify_callback(NotificationProxyClientHandle client, NotificationProxyNotifyCallBack notifyCallBack, System.IntPtr userdata);
    }
}
