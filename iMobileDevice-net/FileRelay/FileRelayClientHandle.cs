// <copyright file="FileRelayClientHandle.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.FileRelay
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
#if !NETSTANDARD1_5
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, UnmanagedCode=true)]
#endif
#if !NETSTANDARD1_5
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.Demand, UnmanagedCode=true)]
#endif
    public partial class FileRelayClientHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        
        private string creationStackTrace;
        
        private ILibiMobileDevice api;
        
        protected FileRelayClientHandle() : 
                base(true)
        {
            this.creationStackTrace = System.Environment.StackTrace;
        }
        
        protected FileRelayClientHandle(bool ownsHandle) : 
                base(ownsHandle)
        {
            this.creationStackTrace = System.Environment.StackTrace;
        }
        
        public ILibiMobileDevice Api
        {
            get
            {
                return this.api;
            }
            set
            {
                this.api = value;
            }
        }
        
        public static FileRelayClientHandle Zero
        {
            get
            {
                return FileRelayClientHandle.DangerousCreate(System.IntPtr.Zero);
            }
        }
        
#if !NETSTANDARD1_5
        [System.Runtime.ConstrainedExecution.ReliabilityContractAttribute(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.MayFail)]
#endif
        protected override bool ReleaseHandle()
        {
            System.Diagnostics.Debug.WriteLine("Releasing {0} {1} using {2}. This object was created at {3}", this.GetType().Name, this.handle, this.Api, this.creationStackTrace);
            return (this.Api.FileRelay.file_relay_client_free(this.handle) == FileRelayError.Success);
        }
        
        public static FileRelayClientHandle DangerousCreate(System.IntPtr unsafeHandle, bool ownsHandle)
        {
            FileRelayClientHandle safeHandle;
            safeHandle = new FileRelayClientHandle(ownsHandle);
            safeHandle.SetHandle(unsafeHandle);
            return safeHandle;
        }
        
        public static FileRelayClientHandle DangerousCreate(System.IntPtr unsafeHandle)
        {
            return FileRelayClientHandle.DangerousCreate(unsafeHandle, true);
        }
        
        public override string ToString()
        {
            return string.Format("{0} ({1})", this.handle, "FileRelayClientHandle");
        }
        
        public override bool Equals(object obj)
        {
            if (((obj != null) & (obj.GetType() == typeof(FileRelayClientHandle))))
            {
                return ((FileRelayClientHandle)obj).handle.Equals(this.handle);
            }
            else
            {
                return false;
            }
        }
        
        public override int GetHashCode()
        {
            return this.handle.GetHashCode();
        }
    }
}
