using Module_USB_Watching.USB_API;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace Module_USB_Watching
{
    class _m_USB_Watcher_Log_DB
    {
        SQLiteConnection dbConnection;
        public _m_USB_Watcher_Log_DB()
        {
            dbConnection = new SQLiteConnection("Data Source=USBWatchingRulesAndLog.sqlite;Version=3;");
            dbConnection.Open();
        }

        public List<DeviceInfo> GetUSBDeviceLog()
        {
            string sql = "select * from USBLog";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;
            SQLiteDataReader reader = cmd.ExecuteReader();
            List<DeviceInfo> AllRuls = new List<DeviceInfo>();
            while (reader.Read())
            {
                AllRuls.Add(ReaderToDeviceInfo(reader));
            }
            return AllRuls;
        }

        private DeviceInfo ReaderToDeviceInfo(SQLiteDataReader Reader)
        {
            DeviceInfo deviceinfo = new DeviceInfo();
            deviceinfo.DevicePath = Reader["DevicePath"].ToString();
            deviceinfo.DeviceID = Reader["DeviceID"].ToString();
            deviceinfo.ClassGUID =System.Guid.Parse(Reader["ClassGUID"].ToString());
            deviceinfo.InterfaceGUID = System.Guid.Parse(Reader["InterfaceGUID"].ToString());
            deviceinfo.Instance =uint.Parse(Reader["Instance"].ToString());
            deviceinfo.Class = Reader["Class"].ToString();
            deviceinfo.Description = Reader["Description"].ToString();
            deviceinfo.Enumerator = Reader["Enumerator"].ToString();
            deviceinfo.Location = Reader["FriendlyName"].ToString();
            deviceinfo.Manufacturer = Reader["FriendlyName"].ToString();
            deviceinfo.ServiceName = Reader["FriendlyName"].ToString();
            deviceinfo.AddTime=DateTime.Parse(Reader["AddTime"].ToString());
            return deviceinfo;
        }

        public void AddDeviceInfoToDB(DeviceInfo deviceinfo)
        {
            SQLiteCommand cmd = new SQLiteCommand(dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into USBLog (DevicePath,DeviceID,ClassGUID,InterfaceGUID,Instance,Class,Description,Enumerator,Location,Manufacturer,ServiceName,AddTime)  values (@DevicePath,@DeviceID,@ClassGUID,@InterfaceGUID,@Instance,@Class,@Description,@Enumerator,@Location,@Manufacturer,@ServiceName,@AddTime)";
            cmd.Parameters.Add("DevicePath", DbType.String).Value = deviceinfo.DevicePath;
            cmd.Parameters.Add("DeviceID", DbType.String).Value = deviceinfo.DeviceID;
            cmd.Parameters.Add("ClassGUID", DbType.String).Value = deviceinfo.ClassGUID.ToString();
            cmd.Parameters.Add("InterfaceGUID", DbType.String).Value = deviceinfo.InterfaceGUID.ToString();
            cmd.Parameters.Add("Instance", DbType.UInt32).Value = deviceinfo.Instance;
            cmd.Parameters.Add("Class", DbType.String).Value = deviceinfo.Class;
            cmd.Parameters.Add("Description", DbType.String).Value = deviceinfo.Description;
            cmd.Parameters.Add("Enumerator", DbType.String).Value = deviceinfo.Enumerator;
            cmd.Parameters.Add("Location", DbType.String).Value = deviceinfo.Location;
            cmd.Parameters.Add("Manufacturer", DbType.String).Value = deviceinfo.Manufacturer;
            cmd.Parameters.Add("ServiceName", DbType.String).Value = deviceinfo.ServiceName;
            cmd.Parameters.Add("AddTime", DbType.DateTime).Value =deviceinfo.AddTime;
            cmd.ExecuteNonQuery();
        }
    }
}
