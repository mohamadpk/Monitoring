using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_USB_Watching
{
    public class _m_USB_Watcher_Disable_Enable_Node
    {
        public _m_USB_Watcher_Disable_Enable_Node()
        {
            RuleName = "";
            StartTime = DateTime.Now;
            EndTime = DateTime.Now;
            Action = true;
            DeviceId = "All";
            RuleStatus = true;
        }
        public _m_USB_Watcher_Disable_Enable_Node(string _RuleName, DateTime _StartTime, DateTime _EndTime, bool _Action = false, string _DeviceId = "All", bool _RuleStatus = true)
        {
            RuleName = _RuleName;
            StartTime = _StartTime;
            EndTime = _EndTime;
            Action = _Action;
            DeviceId = DeviceId;
            RuleStatus = _RuleStatus;
        }

        int _Id;
        string _RuleName;
        DateTime _StartTime;
        DateTime _EndTime;
        bool _Action;
        string _DeviceId;
        bool _RuleStatus;
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        public string RuleName
        {
            get { return _RuleName; }
            set { _RuleName = value; }
        }
        public DateTime StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; }
        }
        public DateTime EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; }
        }

        public bool Action
        {
            get { return _Action; }
            set { _Action = value; }
        }

        public string DeviceId
        {
            get { return _DeviceId; }
            set { _DeviceId = value; }
        }
        public bool RuleStatus
        {
            get { return _RuleStatus; }
            set { _RuleStatus = value; }
        }
    }
}
