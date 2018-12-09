using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Reflection.Emit;

namespace Module_USB_Watching.USB_API
{
    #region Device Class and Interface GUIDS

    /// <summary>
    ///  Defines GUIDs for device classes used in Plug & Play.
    /// </summary>
    public static class GUID_DEVCLASS
    {
        public static Guid GUID_ALL_DEVICES = new Guid();
        public static Guid GUID_DEVCLASS_1394 = new Guid("{0x6bdd1fc1, 0x810f, 0x11d0, {0xbe, 0xc7, 0x08, 0x00, 0x2b, 0xe2, 0x09, 0x2f}}");
        public static Guid GUID_DEVCLASS_1394DEBUG = new Guid("{0x66f250d6, 0x7801, 0x4a64, {0xb1, 0x39, 0xee, 0xa8, 0x0a, 0x45, 0x0b, 0x24}}");
        public static Guid GUID_DEVCLASS_61883 = new Guid("{0x7ebefbc0, 0x3200, 0x11d2, {0xb4, 0xc2, 0x00, 0xa0, 0xc9, 0x69, 0x7d, 0x07}}");
        public static Guid GUID_DEVCLASS_ADAPTER = new Guid("{0x4d36e964, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_APMSUPPORT = new Guid("{0xd45b1c18, 0xc8fa, 0x11d1, {0x9f, 0x77, 0x00, 0x00, 0xf8, 0x05, 0xf5, 0x30}}");
        public static Guid GUID_DEVCLASS_AVC = new Guid("{0xc06ff265, 0xae09, 0x48f0, {0x81, 0x2c, 0x16, 0x75, 0x3d, 0x7c, 0xba, 0x83}}");
        public static Guid GUID_DEVCLASS_BATTERY = new Guid("{0x72631e54, 0x78a4, 0x11d0, {0xbc, 0xf7, 0x00, 0xaa, 0x00, 0xb7, 0xb3, 0x2a}}");
        public static Guid GUID_DEVCLASS_BIOMETRIC = new Guid("{0x53d29ef7, 0x377c, 0x4d14, {0x86, 0x4b, 0xeb, 0x3a, 0x85, 0x76, 0x93, 0x59}}");
        public static Guid GUID_DEVCLASS_BLUETOOTH = new Guid("{0xe0cbf06c, 0xcd8b, 0x4647, {0xbb, 0x8a, 0x26, 0x3b, 0x43, 0xf0, 0xf9, 0x74}}");
        public static Guid GUID_DEVCLASS_CDROM = new Guid("{0x4d36e965, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_COMPUTER = new Guid("{0x4d36e966, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_DECODER = new Guid("{0x6bdd1fc2, 0x810f, 0x11d0, {0xbe, 0xc7, 0x08, 0x00, 0x2b, 0xe2, 0x09, 0x2f}}");
        public static Guid GUID_DEVCLASS_DISKDRIVE = new Guid("{0x4d36e967, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_DISPLAY = new Guid("{0x4d36e968, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_DOT4 = new Guid("{0x48721b56, 0x6795, 0x11d2, {0xb1, 0xa8, 0x00, 0x80, 0xc7, 0x2e, 0x74, 0xa2}}");
        public static Guid GUID_DEVCLASS_DOT4PRINT = new Guid("{0x49ce6ac8, 0x6f86, 0x11d2, {0xb1, 0xe5, 0x00, 0x80, 0xc7, 0x2e, 0x74, 0xa2}}");
        public static Guid GUID_DEVCLASS_ENUM1394 = new Guid("{0xc459df55, 0xdb08, 0x11d1, {0xb0, 0x09, 0x00, 0xa0, 0xc9, 0x08, 0x1f, 0xf6}}");
        public static Guid GUID_DEVCLASS_FDC = new Guid("{0x4d36e969, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_FLOPPYDISK = new Guid("{0x4d36e980, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_GPS = new Guid("{0x6bdd1fc3, 0x810f, 0x11d0, {0xbe, 0xc7, 0x08, 0x00, 0x2b, 0xe2, 0x09, 0x2f}}");
        public static Guid GUID_DEVCLASS_HDC = new Guid("{0x4d36e96a, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_HIDCLASS = new Guid("{0x745a17a0, 0x74d3, 0x11d0, {0xb6, 0xfe, 0x00, 0xa0, 0xc9, 0x0f, 0x57, 0xda}}");
        public static Guid GUID_DEVCLASS_IMAGE = new Guid("{0x6bdd1fc6, 0x810f, 0x11d0, {0xbe, 0xc7, 0x08, 0x00, 0x2b, 0xe2, 0x09, 0x2f}}");
        public static Guid GUID_DEVCLASS_INFINIBAND = new Guid("{0x30ef7132, 0xd858, 0x4a0c, {0xac, 0x24, 0xb9, 0x02, 0x8a, 0x5c, 0xca, 0x3f}}");
        public static Guid GUID_DEVCLASS_INFRARED = new Guid("{0x6bdd1fc5, 0x810f, 0x11d0, {0xbe, 0xc7, 0x08, 0x00, 0x2b, 0xe2, 0x09, 0x2f}}");
        public static Guid GUID_DEVCLASS_KEYBOARD = new Guid("{0x4d36e96b, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_LEGACYDRIVER = new Guid("{0x8ecc055d, 0x047f, 0x11d1, {0xa5, 0x37, 0x00, 0x00, 0xf8, 0x75, 0x3e, 0xd1}}");
        public static Guid GUID_DEVCLASS_MEDIA = new Guid("{0x4d36e96c, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_MEDIUM_CHANGER = new Guid("{0xce5939ae, 0xebde, 0x11d0, {0xb1, 0x81, 0x00, 0x00, 0xf8, 0x75, 0x3e, 0xc4}}");
        public static Guid GUID_DEVCLASS_MODEM = new Guid("{0x4d36e96d, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_MONITOR = new Guid("{0x4d36e96e, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_MOUSE = new Guid("{0x4d36e96f, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_MTD = new Guid("{0x4d36e970, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_MULTIFUNCTION = new Guid("{0x4d36e971, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_MULTIPORTSERIAL = new Guid("{0x50906cb8, 0xba12, 0x11d1, {0xbf, 0x5d, 0x00, 0x00, 0xf8, 0x05, 0xf5, 0x30}}");
        public static Guid GUID_DEVCLASS_NET = new Guid("{0x4d36e972, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_NETCLIENT = new Guid("{0x4d36e973, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_NETSERVICE = new Guid("{0x4d36e974, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_NETTRANS = new Guid("{0x4d36e975, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_NODRIVER = new Guid("{0x4d36e976, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_PCMCIA = new Guid("{0x4d36e977, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_PNPPRINTERS = new Guid("{0x4658ee7e, 0xf050, 0x11d1, {0xb6, 0xbd, 0x00, 0xc0, 0x4f, 0xa3, 0x72, 0xa7}}");
        public static Guid GUID_DEVCLASS_PORTS = new Guid("{0x4d36e978, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_PRINTER = new Guid("{0x4d36e979, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_PRINTERUPGRADE = new Guid("{0x4d36e97a, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_PROCESSOR = new Guid("{0x50127dc3, 0x0f36, 0x415e, {0xa6, 0xcc, 0x4c, 0xb3, 0xbe, 0x91, 0x0B, 0x65}}");
        public static Guid GUID_DEVCLASS_SBP2 = new Guid("{0xd48179be, 0xec20, 0x11d1, {0xb6, 0xb8, 0x00, 0xc0, 0x4f, 0xa3, 0x72, 0xa7}}");
        public static Guid GUID_DEVCLASS_SCSIADAPTER = new Guid("{0x4d36e97b, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_SECURITYACCELERATOR = new Guid("{0x268c95a1, 0xedfe, 0x11d3, {0x95, 0xc3, 0x00, 0x10, 0xdc, 0x40, 0x50, 0xa5}}");
        public static Guid GUID_DEVCLASS_SMARTCARDREADER = new Guid("{0x50dd5230, 0xba8a, 0x11d1, {0xbf, 0x5d, 0x00, 0x00, 0xf8, 0x05, 0xf5, 0x30}}");
        public static Guid GUID_DEVCLASS_SOUND = new Guid("{0x4d36e97c, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_SYSTEM = new Guid("{0x4d36e97d, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_TAPEDRIVE = new Guid("{0x6d807884, 0x7d21, 0x11cf, {0x80, 0x1c, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_UNKNOWN = new Guid("{0x4d36e97e, 0xe325, 0x11ce, {0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18}}");
        public static Guid GUID_DEVCLASS_USB = new Guid("{0x36fc9e60, 0xc465, 0x11cf, {0x80, 0x56, 0x44, 0x45, 0x53, 0x54, 0x00, 0x00}}");
        public static Guid GUID_DEVCLASS_VOLUME = new Guid("{0x71a27cdd, 0x812a, 0x11d0, {0xbe, 0xc7, 0x08, 0x00, 0x2b, 0xe2, 0x09, 0x2f}}");
        public static Guid GUID_DEVCLASS_VOLUMESNAPSHOT = new Guid("{0x533c5b84, 0xec70, 0x11d2, {0x95, 0x05, 0x00, 0xc0, 0x4f, 0x79, 0xde, 0xaf}}");
        public static Guid GUID_DEVCLASS_WCEUSBS = new Guid("{0x25dbce51, 0x6c8f, 0x4a72, {0x8a, 0x6d, 0xb5, 0x4c, 0x2b, 0x4f, 0xc8, 0x35}}");
        public static Guid GUID_DEVCLASS_FSFILTER_ACTIVITYMONITOR = new Guid("{0xb86dff51, 0xa31e, 0x4bac, {0xb3, 0xcf, 0xe8, 0xcf, 0xe7, 0x5c, 0x9f, 0xc2}}");
        public static Guid GUID_DEVCLASS_FSFILTER_UNDELETE = new Guid("{0xfe8f1572, 0xc67a, 0x48c0, {0xbb, 0xac, 0x0b, 0x5c, 0x6d, 0x66, 0xca, 0xfb}}");
        public static Guid GUID_DEVCLASS_FSFILTER_ANTIVIRUS = new Guid("{0xb1d1a169, 0xc54f, 0x4379, {0x81, 0xdb, 0xbe, 0xe7, 0xd8, 0x8d, 0x74, 0x54}}");
        public static Guid GUID_DEVCLASS_FSFILTER_REPLICATION = new Guid("{0x48d3ebc4, 0x4cf8, 0x48ff, {0xb8, 0x69, 0x9c, 0x68, 0xad, 0x42, 0xeb, 0x9f}}");
        public static Guid GUID_DEVCLASS_FSFILTER_CONTINUOUSBACKUP = new Guid("{0x71aa14f8, 0x6fad, 0x4622, {0xad, 0x77, 0x92, 0xbb, 0x9d, 0x7e, 0x69, 0x47}}");
        public static Guid GUID_DEVCLASS_FSFILTER_CONTENTSCREENER = new Guid("{0x3e3f0674, 0xc83c, 0x4558, {0xbb, 0x26, 0x98, 0x20, 0xe1, 0xeb, 0xa5, 0xc5}}");
        public static Guid GUID_DEVCLASS_FSFILTER_QUOTAMANAGEMENT = new Guid("{0x8503c911, 0xa6c7, 0x4919, {0x8f, 0x79, 0x50, 0x28, 0xf5, 0x86, 0x6b, 0x0c}}");
        public static Guid GUID_DEVCLASS_FSFILTER_SYSTEMRECOVERY = new Guid("{0x2db15374, 0x706e, 0x4131, {0xa0, 0xc7, 0xd7, 0xc7, 0x8e, 0xb0, 0x28, 0x9a}}");
        public static Guid GUID_DEVCLASS_FSFILTER_CFSMETADATASERVER = new Guid("{0xcdcf0939, 0xb75b, 0x4630, {0xbf, 0x76, 0x80, 0xf7, 0xba, 0x65, 0x58, 0x84}}");
        public static Guid GUID_DEVCLASS_FSFILTER_HSM = new Guid("{0xd546500a, 0x2aeb, 0x45f6, {0x94, 0x82, 0xf4, 0xb1, 0x79, 0x9c, 0x31, 0x77}}");
        public static Guid GUID_DEVCLASS_FSFILTER_COMPRESSION = new Guid("{0xf3586baf, 0xb5aa, 0x49b5, {0x8d, 0x6c, 0x05, 0x69, 0x28, 0x4c, 0x63, 0x9f}}");
        public static Guid GUID_DEVCLASS_FSFILTER_ENCRYPTION = new Guid("{0xa0a701c0, 0xa511, 0x42ff, {0xaa, 0x6c, 0x06, 0xdc, 0x03, 0x95, 0x57, 0x6f}}");
        public static Guid GUID_DEVCLASS_FSFILTER_PHYSICALQUOTAMANAGEMENT = new Guid("{0x6a0a8e78, 0xbba6, 0x4fc4, {0xa7, 0x09, 0x1e, 0x33, 0xcd, 0x09, 0xd6, 0x7e}}");
        public static Guid GUID_DEVCLASS_FSFILTER_OPENFILEBACKUP = new Guid("{0xf8ecafa6, 0x66d1, 0x41a5, {0x89, 0x9b, 0x66, 0x58, 0x5d, 0x72, 0x16, 0xb7}}");
        public static Guid GUID_DEVCLASS_FSFILTER_SECURITYENHANCER = new Guid("{0xd02bc3da, 0x0c8e, 0x4945, {0x9b, 0xd5, 0xf1, 0x88, 0x3c, 0x22, 0x6c, 0x8c}}");
        public static Guid GUID_DEVCLASS_FSFILTER_COPYPROTECTION = new Guid("{0x89786ff1, 0x9c12, 0x402f, {0x9c, 0x9e, 0x17, 0x75, 0x3c, 0x7f, 0x43, 0x75}}");
        public static Guid GUID_DEVCLASS_FSFILTER_SYSTEM = new Guid("{0x5d1b9aaa, 0x01e2, 0x46af, {0x84, 0x9f, 0x27, 0x2b, 0x3f, 0x32, 0x4c, 0x46}}");
        public static Guid GUID_DEVCLASS_FSFILTER_INFRASTRUCTURE = new Guid("{0xe55fa6f9, 0x128c, 0x4d04, {0xab, 0xab, 0x63, 0x0c, 0x74, 0xb1, 0x45, 0x3a}}");
    }

    /// <summary>
    /// taken from http://msdn.microsoft.com/en-us/library/windows/hardware/ff553412(v=vs.85).aspx
    /// </summary>
    public static class GUID_DEVINTERFACE
    {
        public static Guid BUS1394_CLASS_GUID = new Guid("6BDD1FC1-810F-11d0-BEC7-08002BE2092F");
        public static Guid GUID_61883_CLASS = new Guid("7EBEFBC0-3200-11d2-B4C2-00A0C9697D07");
        public static Guid GUID_DEVICE_APPLICATIONLAUNCH_BUTTON = new Guid("629758EE-986E-4D9E-8E47-DE27F8AB054D");
        public static Guid GUID_DEVICE_BATTERY = new Guid("72631E54-78A4-11D0-BCF7-00AA00B7B32A");
        public static Guid GUID_DEVICE_LID = new Guid("4AFA3D52-74A7-11d0-be5e-00A0C9062857");
        public static Guid GUID_DEVICE_MEMORY = new Guid("3FD0F03D-92E0-45FB-B75C-5ED8FFB01021");
        public static Guid GUID_DEVICE_MESSAGE_INDICATOR = new Guid("CD48A365-FA94-4CE2-A232-A1B764E5D8B4");
        public static Guid GUID_DEVICE_PROCESSOR = new Guid("97FADB10-4E33-40AE-359C-8BEF029DBDD0");
        public static Guid GUID_DEVICE_BUTTON = new Guid("4AFA3D53-74A7-11d0-be5e-00A0C9062857");
        public static Guid GUID_DEVICE_THERMAL_ZONE = new Guid("4AFA3D51-74A7-11d0-be5e-00A0C9062857");
        public static Guid GUID_BTHPORT_DEVICE_INTERFACE = new Guid("0850302A-B344-4fda-9BE9-90576B8D46F0");
        public static Guid GUID_DEVINTERFACE_BRIGHTNESS = new Guid("FDE5BBA4-B3F9-46FB-BDAA-0728CE3100B4");
        public static Guid GUID_DEVINTERFACE_DISPLAY_ADAPTER = new Guid("5B45201D-F2F2-4F3B-85BB-30FF1F953599");
        public static Guid GUID_DEVINTERFACE_I2C = new Guid("2564AA4F-DDDB-4495-B497-6AD4A84163D7");
        public static Guid GUID_DEVINTERFACE_IMAGE = new Guid("6BDD1FC6-810F-11D0-BEC7-08002BE2092F");
        public static Guid GUID_DEVINTERFACE_MONITOR = new Guid("E6F07B5F-EE97-4a90-B076-33F57BF4EAA7");
        public static Guid GUID_DEVINTERFACE_OPM = new Guid("BF4672DE-6B4E-4BE4-A325-68A91EA49C09");
        public static Guid GUID_DEVINTERFACE_VIDEO_OUTPUT_ARRIVAL = new Guid("1AD9E4F0-F88D-4360-BAB9-4C2D55E564CD");
        public static Guid GUID_DISPLAY_DEVICE_ARRIVAL = new Guid("1CA05180-A699-450A-9A0C-DE4FBE3DDD89");
        public static Guid GUID_DEVINTERFACE_HID = new Guid("4D1E55B2-F16F-11CF-88CB-001111000030");
        public static Guid GUID_DEVINTERFACE_KEYBOARD = new Guid("884b96c3-56ef-11d1-bc8c-00a0c91405dd");
        public static Guid GUID_DEVINTERFACE_MOUSE = new Guid("378DE44C-56EF-11D1-BC8C-00A0C91405DD");
        public static Guid GUID_DEVINTERFACE_MODEM = new Guid("2C7089AA-2E0E-11D1-B114-00C04FC2AAE4");
        public static Guid GUID_DEVINTERFACE_NET = new Guid("CAC88484-7515-4C03-82E6-71A87ABAC361");
        public static Guid GUID_DEVINTERFACE_SENSOR = new Guid(0XBA1BB692, 0X9B7A, 0X4833, 0X9A, 0X1E, 0X52, 0X5E, 0XD1, 0X34, 0XE7, 0XE2);
        public static Guid GUID_DEVINTERFACE_COMPORT = new Guid("86E0D1E0-8089-11D0-9CE4-08003E301F73");
        public static Guid GUID_DEVINTERFACE_PARALLEL = new Guid("97F76EF0-F883-11D0-AF1F-0000F800845C");
        public static Guid GUID_DEVINTERFACE_PARCLASS = new Guid("811FC6A5-F728-11D0-A537-0000F8753ED1");
        public static Guid GUID_DEVINTERFACE_SERENUM_BUS_ENUMERATOR = new Guid("4D36E978-E325-11CE-BFC1-08002BE10318");
        public static Guid GUID_DEVINTERFACE_CDCHANGER = new Guid("53F56312-B6BF-11D0-94F2-00A0C91EFB8B");
        public static Guid GUID_DEVINTERFACE_CDROM = new Guid("53F56308-B6BF-11D0-94F2-00A0C91EFB8B");
        public static Guid GUID_DEVINTERFACE_DISK = new Guid("53F56307-B6BF-11D0-94F2-00A0C91EFB8B");
        public static Guid GUID_DEVINTERFACE_FLOPPY = new Guid("53F56311-B6BF-11D0-94F2-00A0C91EFB8B");
        public static Guid GUID_DEVINTERFACE_MEDIUMCHANGER = new Guid("53F56310-B6BF-11D0-94F2-00A0C91EFB8B");
        public static Guid GUID_DEVINTERFACE_PARTITION = new Guid("53F5630A-B6BF-11D0-94F2-00A0C91EFB8B");
        public static Guid GUID_DEVINTERFACE_STORAGEPORT = new Guid("2ACCFE60-C130-11D2-B082-00A0C91EFB8B");
        public static Guid GUID_DEVINTERFACE_TAPE = new Guid("53F5630B-B6BF-11D0-94F2-00A0C91EFB8B");
        public static Guid GUID_DEVINTERFACE_VOLUME = new Guid("53F5630D-B6BF-11D0-94F2-00A0C91EFB8B");
        public static Guid GUID_DEVINTERFACE_WRITEONCEDISK = new Guid("53F5630C-B6BF-11D0-94F2-00A0C91EFB8B");
        public static Guid GUID_IO_VOLUME_DEVICE_INTERFACE = new Guid("53F5630D-B6BF-11D0-94F2-00A0C91EFB8B");
        public static Guid MOUNTDEV_MOUNTED_DEVICE_GUID = new Guid("53F5630D-B6BF-11D0-94F2-00A0C91EFB8B");
        public static Guid GUID_AVC_CLASS = new Guid("095780C3-48A1-4570-BD95-46707F78C2DC");
        public static Guid GUID_VIRTUAL_AVC_CLASS = new Guid("616EF4D0-23CE-446D-A568-C31EB01913D0");
        public static Guid KSCATEGORY_ACOUSTIC_ECHO_CANCEL = new Guid("BF963D80-C559-11D0-8A2B-00A0C9255AC1");
        public static Guid KSCATEGORY_AUDIO = new Guid("6994AD04-93EF-11D0-A3CC-00A0C9223196");
        public static Guid KSCATEGORY_AUDIO_DEVICE = new Guid("FBF6F530-07B9-11D2-A71E-0000F8004788");
        public static Guid KSCATEGORY_AUDIO_GFX = new Guid("9BAF9572-340C-11D3-ABDC-00A0C90AB16F");
        public static Guid KSCATEGORY_AUDIO_SPLITTER = new Guid("9EA331FA-B91B-45F8-9285-BD2BC77AFCDE");
        public static Guid KSCATEGORY_BDA_IP_SINK = new Guid("71985F4A-1CA1-11d3-9CC8-00C04F7971E0");
        public static Guid KSCATEGORY_BDA_NETWORK_EPG = new Guid("71985F49-1CA1-11d3-9CC8-00C04F7971E0");
        public static Guid KSCATEGORY_BDA_NETWORK_PROVIDER = new Guid("71985F4B-1CA1-11d3-9CC8-00C04F7971E0");
        public static Guid KSCATEGORY_BDA_NETWORK_TUNER = new Guid("71985F48-1CA1-11d3-9CC8-00C04F7971E0");
        public static Guid KSCATEGORY_BDA_RECEIVER_COMPONENT = new Guid("FD0A5AF4-B41D-11d2-9C95-00C04F7971E0");
        public static Guid KSCATEGORY_BDA_TRANSPORT_INFORMATION = new Guid("A2E3074F-6C3D-11d3-B653-00C04F79498E");
        public static Guid KSCATEGORY_BRIDGE = new Guid("085AFF00-62CE-11CF-A5D6-28DB04C10000");
        public static Guid KSCATEGORY_CAPTURE = new Guid("65E8773D-8F56-11D0-A3B9-00A0C9223196");
        public static Guid KSCATEGORY_CLOCK = new Guid("53172480-4791-11D0-A5D6-28DB04C10000");
        public static Guid KSCATEGORY_COMMUNICATIONSTRANSFORM = new Guid("CF1DDA2C-9743-11D0-A3EE-00A0C9223196");
        public static Guid KSCATEGORY_CROSSBAR = new Guid("A799A801-A46D-11D0-A18C-00A02401DCD4");
        public static Guid KSCATEGORY_DATACOMPRESSOR = new Guid("1E84C900-7E70-11D0-A5D6-28DB04C10000");
        public static Guid KSCATEGORY_DATADECOMPRESSOR = new Guid("2721AE20-7E70-11D0-A5D6-28DB04C10000");
        public static Guid KSCATEGORY_DATATRANSFORM = new Guid("2EB07EA0-7E70-11D0-A5D6-28DB04C10000");
        public static Guid KSCATEGORY_DRM_DESCRAMBLE = new Guid("FFBB6E3F-CCFE-4D84-90D9-421418B03A8E");
        public static Guid KSCATEGORY_ENCODER = new Guid("19689BF6-C384-48fd-AD51-90E58C79F70B");
        public static Guid KSCATEGORY_ESCALANTE_PLATFORM_DRIVER = new Guid("74F3AEA8-9768-11D1-8E07-00A0C95EC22E");
        public static Guid KSCATEGORY_FILESYSTEM = new Guid("760FED5E-9357-11D0-A3CC-00A0C9223196");
        public static Guid KSCATEGORY_INTERFACETRANSFORM = new Guid("CF1DDA2D-9743-11D0-A3EE-00A0C9223196");
        public static Guid KSCATEGORY_MEDIUMTRANSFORM = new Guid("CF1DDA2E-9743-11D0-A3EE-00A0C9223196");
        public static Guid KSCATEGORY_MICROPHONE_ARRAY_PROCESSOR = new Guid("830A44F2-A32D-476B-BE97-42845673B35A");
        public static Guid KSCATEGORY_MIXER = new Guid("AD809C00-7B88-11D0-A5D6-28DB04C10000");
        public static Guid KSCATEGORY_MULTIPLEXER = new Guid("7A5DE1D3-01A1-452c-B481-4FA2B96271E8");
        public static Guid KSCATEGORY_NETWORK = new Guid("67C9CC3C-69C4-11D2-8759-00A0C9223196");
        public static Guid KSCATEGORY_PREFERRED_MIDIOUT_DEVICE = new Guid("D6C50674-72C1-11D2-9755-0000F8004788");
        public static Guid KSCATEGORY_PREFERRED_WAVEIN_DEVICE = new Guid("D6C50671-72C1-11D2-9755-0000F8004788");
        public static Guid KSCATEGORY_PREFERRED_WAVEOUT_DEVICE = new Guid("D6C5066E-72C1-11D2-9755-0000F8004788");
        public static Guid KSCATEGORY_PROXY = new Guid("97EBAACA-95BD-11D0-A3EA-00A0C9223196");
        public static Guid KSCATEGORY_QUALITY = new Guid("97EBAACB-95BD-11D0-A3EA-00A0C9223196");
        public static Guid KSCATEGORY_REALTIME = new Guid("EB115FFC-10C8-4964-831D-6DCB02E6F23F");
        public static Guid KSCATEGORY_RENDER = new Guid("65E8773E-8F56-11D0-A3B9-00A0C9223196");
        public static Guid KSCATEGORY_SPLITTER = new Guid("0A4252A0-7E70-11D0-A5D6-28DB04C10000");
        public static Guid KSCATEGORY_SYNTHESIZER = new Guid("DFF220F3-F70F-11D0-B917-00A0C9223196");
        public static Guid KSCATEGORY_SYSAUDIO = new Guid("A7C7A5B1-5AF3-11D1-9CED-00A024BF0407");
        public static Guid KSCATEGORY_TEXT = new Guid("6994AD06-93EF-11D0-A3CC-00A0C9223196");
        public static Guid KSCATEGORY_TOPOLOGY = new Guid("DDA54A40-1E4C-11D1-A050-405705C10000");
        public static Guid KSCATEGORY_TVAUDIO = new Guid("A799A802-A46D-11D0-A18C-00A02401DCD4");
        public static Guid KSCATEGORY_TVTUNER = new Guid("A799A800-A46D-11D0-A18C-00A02401DCD4");
        public static Guid KSCATEGORY_VBICODEC = new Guid("07DAD660-22F1-11D1-A9F4-00C04FBBDE8F");
        public static Guid KSCATEGORY_VIDEO = new Guid("6994AD05-93EF-11D0-A3CC-00A0C9223196");
        public static Guid KSCATEGORY_VIRTUAL = new Guid("3503EAC4-1F26-11D1-8AB0-00A0C9223196");
        public static Guid KSCATEGORY_VPMUX = new Guid("A799A803-A46D-11D0-A18C-00A02401DCD4");
        public static Guid KSCATEGORY_WDMAUD = new Guid("3E227E76-690D-11D2-8161-0000F8775BF1");
        public static Guid KSMFT_CATEGORY_AUDIO_DECODER = new Guid("9ea73fb4-ef7a-4559-8d5d-719d8f0426c7");
        public static Guid KSMFT_CATEGORY_AUDIO_EFFECT = new Guid("11064c48-3648-4ed0-932e-05ce8ac811b7");
        public static Guid KSMFT_CATEGORY_AUDIO_ENCODER = new Guid("91c64bd0-f91e-4d8c-9276-db248279d975");
        public static Guid KSMFT_CATEGORY_DEMULTIPLEXER = new Guid("a8700a7a-939b-44c5-99d7-76226b23b3f1");
        public static Guid KSMFT_CATEGORY_MULTIPLEXER = new Guid("059c561e-05ae-4b61-b69d-55b61ee54a7b");
        public static Guid KSMFT_CATEGORY_OTHER = new Guid("90175d57-b7ea-4901-aeb3-933a8747756f");
        public static Guid KSMFT_CATEGORY_VIDEO_DECODER = new Guid("d6c02d4b-6833-45b4-971a-05a4b04bab91");
        public static Guid KSMFT_CATEGORY_VIDEO_EFFECT = new Guid("12e17c21-532c-4a6e-8a1c-40825a736397");
        public static Guid KSMFT_CATEGORY_VIDEO_ENCODER = new Guid("f79eac7d-e545-4387-bdee-d647d7bde42a");
        public static Guid KSMFT_CATEGORY_VIDEO_PROCESSOR = new Guid("302ea3fc-aa5f-47f9-9f7a-c2188bb16302");
        public static Guid GUID_DEVINTERFACE_USB_DEVICE = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED");
        public static Guid GUID_DEVINTERFACE_USB_HOST_CONTROLLER = new Guid("3ABF6F2D-71C4-462A-8A92-1E6861E6AF27");
        public static Guid GUID_DEVINTERFACE_USB_HUB = new Guid("F18A0E88-C30C-11D0-8815-00A0C906BED8");
        public static Guid GUID_DEVINTERFACE_WPD = new Guid("6AC27878-A6FA-4155-BA85-F98F491D4F33");
        public static Guid GUID_DEVINTERFACE_WPD_PRIVATE = new Guid("BA0C718F-4DED-49B7-BDD3-FABE28661211");
        public static Guid GUID_DEVINTERFACE_SIDESHOW = new Guid("152E5811-FEB9-4B00-90F4-D32947AE1681");
    }

    #endregion

    #region Device Registry Property Codes

    /// <summary>
    /// Device registry property codes
    /// </summary>
    public enum SPDRP : uint
    {
        /// <summary>
        /// DeviceDesc (R/W)
        /// </summary>
        SPDRP_DEVICEDESC = 0x00000000,

        /// <summary>
        /// HardwareID (R/W)
        /// </summary>
        SPDRP_HARDWAREID = 0x00000001,

        /// <summary>
        /// CompatibleIDs (R/W)
        /// </summary>
        SPDRP_COMPATIBLEIDS = 0x00000002,

        /// <summary>
        /// unused
        /// </summary>
        SPDRP_UNUSED0 = 0x00000003,

        /// <summary>
        /// Service (R/W)
        /// </summary>
        SPDRP_SERVICE = 0x00000004,

        /// <summary>
        /// unused
        /// </summary>
        SPDRP_UNUSED1 = 0x00000005,

        /// <summary>
        /// unused
        /// </summary>
        SPDRP_UNUSED2 = 0x00000006,

        /// <summary>
        /// Class (R--tied to ClassGUID)
        /// </summary>
        SPDRP_CLASS = 0x00000007,

        /// <summary>
        /// ClassGUID (R/W)
        /// </summary>
        SPDRP_CLASSGUID = 0x00000008,

        /// <summary>
        /// Driver (R/W)
        /// </summary>
        SPDRP_DRIVER = 0x00000009,

        /// <summary>
        /// ConfigFlags (R/W)
        /// </summary>
        SPDRP_CONFIGFLAGS = 0x0000000A,

        /// <summary>
        /// Mfg (R/W)
        /// </summary>
        SPDRP_MFG = 0x0000000B,

        /// <summary>
        /// FriendlyName (R/W)
        /// </summary>
        SPDRP_FRIENDLYNAME = 0x0000000C,

        /// <summary>
        /// LocationInformation (R/W)
        /// </summary>
        SPDRP_LOCATION_INFORMATION = 0x0000000D,

        /// <summary>
        /// PhysicalDeviceObjectName (R)
        /// </summary>
        SPDRP_PHYSICAL_DEVICE_OBJECT_NAME = 0x0000000E,

        /// <summary>
        /// Capabilities (R)
        /// </summary>
        SPDRP_CAPABILITIES = 0x0000000F,

        /// <summary>
        /// UiNumber (R)
        /// </summary>
        SPDRP_UI_NUMBER = 0x00000010,

        /// <summary>
        /// UpperFilters (R/W)
        /// </summary>
        SPDRP_UPPERFILTERS = 0x00000011,

        /// <summary>
        /// LowerFilters (R/W)
        /// </summary>
        SPDRP_LOWERFILTERS = 0x00000012,

        /// <summary>
        /// BusTypeGUID (R)
        /// </summary>
        SPDRP_BUSTYPEGUID = 0x00000013,

        /// <summary>
        /// LegacyBusType (R)
        /// </summary>
        SPDRP_LEGACYBUSTYPE = 0x00000014,

        /// <summary>
        /// BusNumber (R)
        /// </summary>
        SPDRP_BUSNUMBER = 0x00000015,

        /// <summary>
        /// Enumerator Name (R)
        /// </summary>
        SPDRP_ENUMERATOR_NAME = 0x00000016,

        /// <summary>
        /// Security (R/W, binary form)
        /// </summary>
        SPDRP_SECURITY = 0x00000017,

        /// <summary>
        /// Security (W, SDS form)
        /// </summary>
        SPDRP_SECURITY_SDS = 0x00000018,

        /// <summary>
        /// Device Type (R/W)
        /// </summary>
        SPDRP_DEVTYPE = 0x00000019,

        /// <summary>
        /// Device is exclusive-access (R/W)
        /// </summary>
        SPDRP_EXCLUSIVE = 0x0000001A,

        /// <summary>
        /// Device Characteristics (R/W)
        /// </summary>
        SPDRP_CHARACTERISTICS = 0x0000001B,

        /// <summary>
        /// Device Address (R)
        /// </summary>
        SPDRP_ADDRESS = 0x0000001C,

        /// <summary>
        /// UiNumberDescFormat (R/W)
        /// </summary>
        SPDRP_UI_NUMBER_DESC_FORMAT = 0X0000001D,

        /// <summary>
        /// Device Power Data (R)
        /// </summary>
        SPDRP_DEVICE_POWER_DATA = 0x0000001E,

        /// <summary>
        /// Removal Policy (R)
        /// </summary>
        SPDRP_REMOVAL_POLICY = 0x0000001F,

        /// <summary>
        /// Hardware Removal Policy (R)
        /// </summary>
        SPDRP_REMOVAL_POLICY_HW_DEFAULT = 0x00000020,

        /// <summary>
        /// Removal Policy Override (RW)
        /// </summary>
        SPDRP_REMOVAL_POLICY_OVERRIDE = 0x00000021,

        /// <summary>
        /// Device Install State (R)
        /// </summary>
        SPDRP_INSTALL_STATE = 0x00000022,

        /// <summary>
        /// Device Location Paths (R)
        /// </summary>
        SPDRP_LOCATION_PATHS = 0x00000023,
    }

    #endregion

    public class DeviceInfo
    {
        public String DevicePath { get; set; }

        public String DeviceID { get; set; }
        public Guid ClassGUID { get; set; }
        public Guid InterfaceGUID { get; set; }
        public uint Instance { get; set; }

        public String Class { get; set; }

        public String FriendlyName { get; set; }
        public String Description { get; set; }

        public String Enumerator { get; set; }
        public String Location { get; set; }

        public String Manufacturer { get; set; }
        public String ServiceName { get; set; }

        /// <summary>
        /// Device hardware manufacturer's hexadecimal Vendor ID.  This ID is common 
        /// to all devices made by a particular manufacturer, usually assigned by the 
        /// hardware type industry association.  Exact format varies between device types.
        /// May be empty depending on device type.
        /// </summary>
        public String VID { get; set; }

        /// <summary>
        /// Device hardware manufacturer's Product ID for this particular device.  This is
        /// a vendor-assigned product code for the particular device, which can be used to
        /// distinguish different kinds of devices from the same manufacturer.  Exact format varies
        /// between device types.  May be empty depending on device type.
        /// </summary>
        public String PID { get; set; }

        /// <summary>
        /// Vendor-assigned revision code used to distinguish different revisions on the same
        /// general device from the same manucturer.  Exact format varies between device types.
        /// May be empty if manufacturer didn't assign a revision code, or device type doesn't
        /// support revisions.
        /// </summary>
        public String Revision { get; set; }

        /// <summary>
        /// Interface number on composite devices (USB devices with multiple interfaces, etc.).
        /// </summary>
        public String Interface { get; set; }

        public DateTime AddTime { get; set; }


        #region DeviceID Parsing

        public DeviceInfo(String DeviceID)
        {
            SetAndParseDeviceID(DeviceID);
        }
        public DeviceInfo()
        {

        }
        public void SetAndParseDeviceID(String DeviceID)
        {
            HardwareID H = new HardwareID(DeviceID);
            this.DeviceID = DeviceID;

            Enumerator = H.Enumerator;
            VID = H.VID;
            PID = H.PID;
            Revision = H.Revision;
            Interface = H.Interface;
        }

        #endregion

    }

    public static class Devices
    {
        

        /// <summary>
        /// Get a list of all currently-connected USB devices.  Calls GetDevices()
        /// </summary>
        /// <param name="VID">Only find USB devices with this Vendor ID</param>
        /// <param name="PID">Only find USB devices with the given VID and this Product ID.  Ignored if VID==null</param>
        /// <returns>List of all found devices</returns>
        static public List<DeviceInfo> GetConnectedUSBDevices(String VID = null, String PID = null)
        {
            List<DeviceInfo> Devices = GetDevices(GUID_DEVINTERFACE.GUID_DEVINTERFACE_USB_DEVICE, true);

            if (String.IsNullOrEmpty(VID))
                return Devices;
            else if (String.IsNullOrEmpty(PID))
                return Devices.Where(x => x.VID.Equals(VID)).ToList();
            else
                return Devices.Where(x => x.VID.Equals(VID) && x.PID.Equals(PID)).ToList();
        }

        /// <summary>
        /// Get a list of all currently-connected HID devices.  Calls GetDevices()
        /// </summary>
        /// <param name="VID">Only find USB devices with this Vendor ID</param>
        /// <param name="PID">Only find USB devices with the given VID and this Product ID.  Ignored if VID==null</param>
        /// <returns>List of all found devices</returns>
        static public List<DeviceInfo> GetConnectedHIDDevices(String VID = null, String PID = null)
        {
            List<DeviceInfo> Devices = GetDevices(GUID_DEVINTERFACE.GUID_DEVINTERFACE_HID, true);

            if (String.IsNullOrEmpty(VID))
                return Devices;
            else if (String.IsNullOrEmpty(PID))
                return Devices.Where(x => x.VID.Equals(VID)).ToList();
            else
                return Devices.Where(x => x.VID.Equals(VID) && x.PID.Equals(PID)).ToList();
        }


        static public bool RemoveDevices(uint Instance)
        {

            int DN_REMOVABLE = 0x00004000;
            int CR_SUCCESS = 0;
            uint status = 0;
            uint problem = 0;
            bool success = false;
            if (CR_SUCCESS == SetupAPI.CM_Get_DevNode_Status(out status, out problem, Instance, 0) && (DN_REMOVABLE & status) > 0)
            {
                SetupAPI.PNP_VETO_TYPE pnp_veto_type;
                System.Text.StringBuilder sb = new System.Text.StringBuilder(255);

                success = (CR_SUCCESS == SetupAPI.CM_Request_Device_Eject(Instance, out pnp_veto_type, sb, sb.Capacity, 0));
            }

            return success;

        }
        /// <summary>
        /// Get a list of all known devices of a given class, optionally only selecting devices 
        /// that are physically present at the time of the call.
        /// 
        /// Throws a System.ComponentModel.Win32Exception on any Windows API error
        /// </summary>
        /// <param name="DeviceClassGUID">GUID of device class to enumerate.  May be a value from
        /// GUID_DEVINTERFACE, any other known device class GUID, or Guid.Empty to find all USB devices.</param>
        /// <param name="Present">TRUE if only present (conencted) devices should be returned</param>
        /// <returns>List of all found devices</returns>
        static public List<DeviceInfo> GetDevices(Guid DeviceClassGUID, Boolean Present = true)
        {
            List<DeviceInfo> Devices = new List<DeviceInfo>();
            if (DeviceClassGUID == Guid.Empty)
                DeviceClassGUID = GUID_DEVINTERFACE.GUID_DEVINTERFACE_USB_DEVICE;

            IntPtr deviceList = IntPtr.Zero;
            int Flags = (Present ? (int)SetupAPI.DiGetClassFlags.DIGCF_PRESENT : 0);
            Flags = Flags | (int)SetupAPI.DiGetClassFlags.DIGCF_DEVICEINTERFACE;
            deviceList = SetupAPI.SetupDiGetClassDevs(ref DeviceClassGUID, IntPtr.Zero, IntPtr.Zero, Flags);

            if (deviceList.ToInt64() == SetupAPI.INVALID_HANDLE_VALUE)
                throw new Win32Exception();

            SetupAPI.SP_DEVINFO_DATA DevInfo = new SetupAPI.SP_DEVINFO_DATA();
            DevInfo.cbSize = (uint)Marshal.SizeOf(DevInfo);

            uint DeviceIndex = 0;
            while (SetupAPI.SetupDiEnumDeviceInfo(deviceList, DeviceIndex, ref DevInfo))
            {
                    String DevID = SetupAPI.GetDeviceID(DevInfo);
                if (String.IsNullOrEmpty(DevID))
                    throw new Win32Exception();

                SetupAPI.SP_DEVINFO_DATA IntInfo = new SetupAPI.SP_DEVINFO_DATA();
                IntInfo.cbSize = (uint)Marshal.SizeOf(IntInfo);

                uint InterfaceIndex = 0;
                while (SetupAPI.SetupDiEnumDeviceInterfaces(deviceList, ref DevInfo, ref DeviceClassGUID, InterfaceIndex, ref IntInfo))
                {
                    DeviceInfo Info = new DeviceInfo(DevID);

                    Info.DevicePath = SetupAPI.GetDeviceInterfacePath(deviceList, ref DevInfo, ref IntInfo);
                    if (String.IsNullOrEmpty(Info.DevicePath))
                        throw new Win32Exception();

                    Info.InterfaceGUID = IntInfo.classGuid;
                    Info.Instance = DevInfo.devInst;

                    Info.Class = SetupAPI.GetDevicePropertyString(deviceList, DevInfo, (uint)SPDRP.SPDRP_CLASS);
                    Info.FriendlyName = SetupAPI.GetDevicePropertyString(deviceList, DevInfo, (uint)SPDRP.SPDRP_FRIENDLYNAME);
                    Info.Description = SetupAPI.GetDevicePropertyString(deviceList, DevInfo, (uint)SPDRP.SPDRP_DEVICEDESC);

                    Info.Enumerator = SetupAPI.GetDevicePropertyString(deviceList, DevInfo, (uint)SPDRP.SPDRP_ENUMERATOR_NAME);
                    Info.Location = SetupAPI.GetDevicePropertyString(deviceList, DevInfo, (uint)SPDRP.SPDRP_LOCATION_INFORMATION);

                    Info.Manufacturer = SetupAPI.GetDevicePropertyString(deviceList, DevInfo, (uint)SPDRP.SPDRP_MFG);
                    Info.ServiceName = SetupAPI.GetDevicePropertyString(deviceList, DevInfo, (uint)SPDRP.SPDRP_SERVICE);
                    Info.ClassGUID = System.Guid.Parse(SetupAPI.GetDevicePropertyString(deviceList, DevInfo, (uint)SPDRP.SPDRP_CLASSGUID));
                    Devices.Add(Info);
                    InterfaceIndex++;

                    IntInfo = new SetupAPI.SP_DEVINFO_DATA();
                    IntInfo.cbSize = (uint)Marshal.SizeOf(IntInfo);
                }

                DeviceIndex++;
            }

            SetupAPI.SetupDiDestroyDeviceInfoList(deviceList);

            return Devices;
        }
    }
}
