using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
namespace Module_USB_Watching.USB_API
{
    /// <summary>
    /// p/Invoke Declarations for access to Windows system API contained in setupapi.dll
    /// </summary>
    internal static class SetupAPI
    {
        #region Device Info Structures

        internal const Int32 INVALID_HANDLE_VALUE = -1;
        internal const Int32 BUFFER_SIZE = 260;

        [StructLayout(LayoutKind.Sequential)]
        internal struct SP_DEVINFO_DATA
        {
            public uint cbSize;
            public Guid classGuid;
            public uint devInst;
            public IntPtr reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public int cbSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BUFFER_SIZE)]
            public string DevicePath;
        }

        #endregion

        #region GetDeviceInterfaceDetail

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern Boolean SetupDiGetDeviceInterfaceDetail(
           IntPtr hDevInfo,
           ref SP_DEVINFO_DATA deviceInterfaceData,
           IntPtr deviceInterfaceDetailData,
           int deviceInterfaceDetailDataSize,
           ref UInt32 requiredSize,
           ref SP_DEVINFO_DATA deviceInfoData
        );

        public static String GetDeviceInterfacePath(IntPtr DeviceInfoSet, ref SP_DEVINFO_DATA devInfo, ref SP_DEVINFO_DATA deviceInterfaceData)
        {
            String devicePath = null;
            IntPtr detailData = IntPtr.Zero;
            UInt32 detailSize = 0;

            SetupDiGetDeviceInterfaceDetail(DeviceInfoSet, ref deviceInterfaceData, detailData, 0, ref detailSize, ref devInfo);
            if (detailSize > 0)
            {
                int structSize = Marshal.SystemDefaultCharSize;
                if (IntPtr.Size == 8)
                    structSize += 6;  // 64-bit systems, with 8-byte packing
                else
                    structSize += 4; // 32-bit systems, with byte packing

                detailData = Marshal.AllocHGlobal((int)detailSize + structSize);
                Marshal.WriteInt32(detailData, (int)structSize);
                Boolean Success = SetupDiGetDeviceInterfaceDetail(DeviceInfoSet, ref deviceInterfaceData, detailData, (int)detailSize, ref detailSize, ref devInfo);
                if (Success)
                {
                    devicePath = Marshal.PtrToStringUni(new IntPtr(detailData.ToInt64() + 4));
                }
                Marshal.FreeHGlobal(detailData);
            }

            return devicePath;
        }


        #endregion

        #region GetClassDevs

        [Flags]
        internal enum DiGetClassFlags : uint
        {
            DIGCF_DEFAULT = 0x00000001,  // only valid with DIGCF_DEVICEINTERFACE
            DIGCF_PRESENT = 0x00000002,
            DIGCF_ALLCLASSES = 0x00000004,
            DIGCF_PROFILE = 0x00000008,
            DIGCF_DEVICEINTERFACE = 0x00000010,
        }

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, IntPtr Enumerator, IntPtr hwndParent, int Flags);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]     // 2nd form uses an Enumerator only, with null ClassGUID 
        internal static extern IntPtr SetupDiGetClassDevs(IntPtr ClassGuid, string Enumerator, IntPtr hwndParent, int Flags);

        #endregion

        #region GetDeviceID

        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern int CM_Get_Device_ID_Size(out int pulLen, UInt32 dnDevInst, int flags = 0);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        internal static extern int CM_Get_Device_ID(UInt32 dnDevInst, IntPtr buffer, int bufferLen, int flags = 0);

        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern int CM_Get_DevNode_Status(out UInt32 status, out UInt32 probNum, UInt32 devInst, int flags);

        internal enum PNP_VETO_TYPE : int
        {
            PNP_VetoTypeUnknown = 0,
            PNP_VetoLegacyDevice = 1,
            PNP_VetoPendingClose = 2,
            PNP_VetoWindowsApp = 3,
            PNP_VetoWindowsService = 4,
            PNP_VetoOutstandingOpen = 5,
            PNP_VetoDevice = 6,
            PNP_VetoDriver = 7,
            PNP_VetoIllegalDeviceRequest = 8,
            PNP_VetoInsufficientPower = 9,
            PNP_VetoNonDisableable = 10,
            PNP_VetoLegacyDriver = 11,
            PNP_VetoInsufficientRights = 12
        }

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        internal static extern int CM_Request_Device_Eject(uint devinst, out PNP_VETO_TYPE pVetoType, System.Text.StringBuilder pszVetoName, int ulNameLength, int ulFlags);






        /// <summary>
        /// Extract a Device ID string from a given device/interface info structure
        /// </summary>
        /// <param name="DevInfo">Device/Interface to obtain ID for</param>
        /// <returns>ID for the particular device/interface, or null on any error</returns>
        public static String GetDeviceID(SP_DEVINFO_DATA DevInfo)
        {
            int numChars;
            CM_Get_Device_ID_Size(out numChars, DevInfo.devInst);

            if (numChars < 1)
                return null;

            numChars += 1; // add room for null

            IntPtr idBuffer = Marshal.AllocHGlobal(numChars * Marshal.SystemDefaultCharSize);
            CM_Get_Device_ID(DevInfo.devInst, idBuffer, numChars);
            String DeviceID = Marshal.PtrToStringAuto(idBuffer);
            Marshal.FreeHGlobal(idBuffer);

            return DeviceID;
        }

        #endregion

        #region Device Registry Properties

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetupDiGetDeviceRegistryProperty(
            IntPtr DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, uint Property, out UInt32 PropertyRegDataType,
            IntPtr PropertyBuffer, uint PropertyBufferSize, out uint RequiredSize);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetupDiGetDeviceRegistryProperty(
            IntPtr DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, uint Property, out UInt32 PropertyRegDataType,
            IntPtr PropertyBuffer, uint PropertyBufferSize, IntPtr RequiredSize);

        /// <summary>
        /// Attempt to return a property value for a specific device
        /// </summary>
        /// <param name="deviceList">Device List containing the device</param>
        /// <param name="DevInfo">Device Info Structure</param>
        /// <param name="Property">Property to obtain, one of the SPDRP_ values</param>
        /// <returns>Property value as a string (if applicable), or null</returns>
        public static String GetDevicePropertyString(IntPtr deviceList, SP_DEVINFO_DATA DevInfo, uint Property)
        {
            UInt32 PropertyDataType;

            uint numBytes;
            SetupDiGetDeviceRegistryProperty(deviceList, ref DevInfo, Property, out PropertyDataType, IntPtr.Zero, 0, out numBytes);
            if (numBytes > 0)
            {
                IntPtr propertyBuffer = Marshal.AllocHGlobal((int)numBytes);
                SetupDiGetDeviceRegistryProperty(deviceList, ref DevInfo, Property, out PropertyDataType, propertyBuffer, numBytes, IntPtr.Zero);
                String PropertyValue = Marshal.PtrToStringAuto(propertyBuffer);
                Marshal.FreeHGlobal(propertyBuffer);
                return PropertyValue;
            }

            return null;
        }

        #endregion

        #region EnumDeviceInterfaces

        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern bool SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, uint MemberIndex, ref SP_DEVINFO_DATA DeviceInfoData);

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern Boolean SetupDiEnumDeviceInterfaces(
           IntPtr hDevInfo,
           ref SP_DEVINFO_DATA devInfo,
           ref Guid interfaceClassGuid,
           UInt32 memberIndex,
           ref SP_DEVINFO_DATA deviceInterfaceData
        );

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern Boolean SetupDiEnumDeviceInterfaces(
           IntPtr hDevInfo,
           IntPtr devInfo,
           ref Guid interfaceClassGuid,
           UInt32 memberIndex,
           ref SP_DEVINFO_DATA deviceInterfaceData
        );

        #endregion

        #region Memory Cleanup

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        #endregion
    }
}
