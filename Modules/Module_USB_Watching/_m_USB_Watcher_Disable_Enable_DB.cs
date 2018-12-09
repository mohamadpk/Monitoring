using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace Module_USB_Watching
{
    class _m_USB_Watcher_Disable_Enable_DB
    {
        //TODO Add Log USB Function 
        SQLiteConnection dbConnection;

        public _m_USB_Watcher_Disable_Enable_DB()
        {
            dbConnection = new SQLiteConnection("Data Source=USBWatchingRulesAndLog.sqlite;Version=3;");
            dbConnection.Open();
        }

        public List<_m_USB_Watcher_Disable_Enable_Node> GetAllRules()
        {
            string sql = "select * from DisableEnableRuleList";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;
            SQLiteDataReader reader = cmd.ExecuteReader();
            List<_m_USB_Watcher_Disable_Enable_Node> AllRuls = new List<_m_USB_Watcher_Disable_Enable_Node>();
            while (reader.Read())
            {
                AllRuls.Add(ReaderToRule(reader));
            }
            return AllRuls;
        }

        private _m_USB_Watcher_Disable_Enable_Node ReaderToRule(SQLiteDataReader Reader)
        {
            _m_USB_Watcher_Disable_Enable_Node micden = new _m_USB_Watcher_Disable_Enable_Node();
            micden.RuleName = Reader["RuleName"].ToString();
            micden.StartTime = DateTime.Parse(Reader["StartTime"].ToString());
            micden.EndTime = DateTime.Parse(Reader["EndTime"].ToString());
            micden.Action = bool.Parse(Reader["Action"].ToString());
            micden.DeviceId = Reader["DeviceId"].ToString();
            micden.RuleStatus = bool.Parse(Reader["RuleStatus"].ToString());
            return micden;
        }

        public void AddRuleToDB(_m_USB_Watcher_Disable_Enable_Node RuleNode)
        {
            SQLiteCommand cmd = new SQLiteCommand(dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into DisableEnableRuleList (RuleName,StartTime,EndTime,Action,DeviceId,RuleStatus)  values (@RuleName,@StartTime,@EndTime,@Action,@DeviceId,@RuleStatus)";
            cmd.Parameters.Add("RuleName", DbType.String).Value = RuleNode.RuleName;
            cmd.Parameters.Add("StartTime", DbType.DateTime).Value = RuleNode.StartTime;
            cmd.Parameters.Add("EndTime", DbType.DateTime).Value = RuleNode.EndTime;
            cmd.Parameters.Add("Action", DbType.Boolean).Value = RuleNode.Action;
            cmd.Parameters.Add("DeviceId", DbType.String).Value = RuleNode.DeviceId;
            cmd.Parameters.Add("RuleStatus", DbType.Boolean).Value = RuleNode.RuleStatus;
            cmd.ExecuteNonQuery();
        }
        public void RemoveRuleFromDB(_m_USB_Watcher_Disable_Enable_Node RuleNode)
        {
            string sql = "DELETE from DisableEnableRuleList where RuleName==@RuleName";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SQLiteParameter("@RuleName", RuleNode.RuleName));
            SQLiteDataReader reader = cmd.ExecuteReader();
        }
    }
}
