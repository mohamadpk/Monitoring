using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;

namespace Module_Internet_Reporter.Internet_Connection_Control
{
    public class _m_Internet_Connection_Disable_Enable_DB
    {
        SQLiteConnection dbConnection;

        public _m_Internet_Connection_Disable_Enable_DB()
        {
            dbConnection = new SQLiteConnection("Data Source=InternetControlrules.sqlite;Version=3;");
            dbConnection.Open();
        }

        public List<_m_Internet_Connection_Disable_Enable_Node> GetAllRules()
        {
            string sql = "select * from DisableEnableRuleList";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;
            SQLiteDataReader reader = cmd.ExecuteReader();
            List<_m_Internet_Connection_Disable_Enable_Node> AllRuls = new List<_m_Internet_Connection_Disable_Enable_Node>();
            while (reader.Read())
            {
                AllRuls.Add(ReaderToRule(reader));
            }
            return AllRuls;
        }

        private _m_Internet_Connection_Disable_Enable_Node ReaderToRule(SQLiteDataReader Reader)
        {
            _m_Internet_Connection_Disable_Enable_Node micden = new _m_Internet_Connection_Disable_Enable_Node();
            micden.RuleName= Reader["RuleName"].ToString();
            micden.StartTime =DateTime.Parse(Reader["StartTime"].ToString());
            micden.EndTime = DateTime.Parse(Reader["EndTime"].ToString());
            micden.Action = bool.Parse(Reader["Action"].ToString());
            micden.Eth_TargetName = Reader["Eth_TargetName"].ToString();
            micden.RuleStatus = bool.Parse(Reader["RuleStatus"].ToString());          
            return micden;
        }

        public void AddRuleToDB(_m_Internet_Connection_Disable_Enable_Node RuleNode)
        {
            SQLiteCommand cmd = new SQLiteCommand(dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into DisableEnableRuleList (RuleName,StartTime,EndTime,Action,Eth_TargetName,RuleStatus)  values (@RuleName,@StartTime,@EndTime,@Action,@Eth_TargetName,@RuleStatus)";
            cmd.Parameters.Add("RuleName", DbType.String).Value = RuleNode.RuleName;
            cmd.Parameters.Add("StartTime", DbType.DateTime).Value = RuleNode.StartTime;
            cmd.Parameters.Add("EndTime", DbType.DateTime).Value = RuleNode.EndTime;
            cmd.Parameters.Add("Action", DbType.Boolean).Value = RuleNode.Action;
            cmd.Parameters.Add("Eth_TargetName", DbType.String).Value = RuleNode.Eth_TargetName;
            cmd.Parameters.Add("RuleStatus", DbType.Boolean).Value = RuleNode.RuleStatus;
            cmd.ExecuteNonQuery();
        }
        public void RemoveRuleFromDB(_m_Internet_Connection_Disable_Enable_Node RuleNode)
        {
            string sql = "DELETE from DisableEnableRuleList where RuleName==@RuleName";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SQLiteParameter("@RuleName", RuleNode.RuleName));
            SQLiteDataReader reader = cmd.ExecuteReader();
        }
    }
}
