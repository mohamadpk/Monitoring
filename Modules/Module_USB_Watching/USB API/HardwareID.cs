using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module_USB_Watching.USB_API
{
    /// <summary>
    /// Manage Windows HardwareID/DeviceID strings.  Full HardwareID information can be found
    /// (at time of this writing) at https://msdn.microsoft.com/en-us/library/windows/hardware/ff546152(v=vs.85).aspx
    /// </summary>
    public class HardwareID
    {
        public enum HardwareIDFormat
        {
            Unknown,
            DeviceIdentifier,
            GenericIdentifier
        }

        private String _ID = null;
        public String ID { get { return _ID; } }

        private HardwareIDFormat _Format = HardwareIDFormat.Unknown;
        public HardwareIDFormat Format { get { return _Format; } }

        private String _Enumerator = null;
        public String Enumerator { get { return _Enumerator; } }

        private String _VID = null;
        /// <summary>
        /// Device hardware manufacturer's hexadecimal Vendor ID.  This ID is common 
        /// to all devices made by a particular manufacturer, usually assigned by the 
        /// hardware type industry association.  Exact format varies between device types.
        /// May be empty depending on device type.
        /// </summary>
        public String VID { get { return _VID; } }

        private String _PID = null;
        /// <summary>
        /// Device hardware manufacturer's Product ID for this particular device.  This is
        /// a vendor-assigned product code for the particular device, which can be used to
        /// distinguish different kinds of devices from the same manufacturer.  Exact format varies
        /// between device types.  May be empty depending on device type.
        /// </summary>
        public String PID { get { return _PID; } }

        private String _OID = null;
        /// <summary>
        /// Original Equipment Manufacturer (OEM) ID code assigned to the device hardware manufacturer
        /// by the hardware type industry assiociation, used for additional identifcation on some device
        /// types.  May be empty if device type doesn't support an OEM ID.
        /// </summary>
        public String OID { get { return _OID; } }

        private String _Revision = null;
        /// <summary>
        /// Vendor-assigned revision code used to distinguish different revisions on the same
        /// general device from the same manucturer.  Exact format varies between device types.
        /// May be empty if manufacturer didn't assign a revision code, or device type doesn't
        /// support revisions.
        /// </summary>
        public String Revision { get { return _Revision; } }

        private String _Interface = null;
        /// <summary>
        /// Interface number on composite devices (USB devices with multiple interfaces, etc.).
        /// </summary>
        public String Interface { get { return _Interface; } }

        public HardwareID(String IDString)
        {
            this._ID = IDString;
            ParseIDString();
        }

        private void ResetValues()
        {
            _Format = HardwareIDFormat.Unknown;
            _Enumerator = _VID = _PID = _Revision = _Interface = null;
        }

        private void ParseIDString()
        {
            ResetValues();
            if (!String.IsNullOrEmpty(_ID))
            {
                if (_ID[0] == '*')
                {
                    _Format = HardwareIDFormat.GenericIdentifier;
                    if (_ID.Length > 3)
                        _Enumerator = _ID.Substring(1, 3); // Should be "PNP"
                }
                else
                {
                    string[] Tokens = _ID.Split('\\');
                    if (Tokens.Count() > 1)
                    {
                        _Enumerator = Tokens[0];
                        ParseDescriptor(Tokens[1]);
                    }
                }
            }
        }

        private void ParseDescriptor(String Desc)
        {
            string[] Tokens = Desc.Split('&');
            foreach (String token in Tokens)
            {
                if (token.Length > 4)
                {
                    string KeyName = token.Substring(0, 4);
                    string Value = token.Substring(5);
                    if (KeyName.Equals("VID_", StringComparison.InvariantCulture))
                        _VID = Value;
                    else if (KeyName.Equals("PID_", StringComparison.InvariantCulture))
                        _PID = Value;
                    else if (KeyName.Equals("OID_", StringComparison.InvariantCulture))
                        _OID = Value;
                    else if (KeyName.Equals("REV_", StringComparison.InvariantCulture))
                        _Revision = Value;
                    else if (KeyName.Substring(0, 3).Equals("MI_", StringComparison.InvariantCulture))
                        _Interface = KeyName.Substring(4) + Value;
                }
            }
        }

    }
}
