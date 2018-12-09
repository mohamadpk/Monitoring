using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_Internet_Reporter.Internet_Connection_Control
{
    /// <summary>
    /// the rule node for disable and enable network connection on agent
    /// </summary>
    public class _m_Internet_Connection_Disable_Enable_Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="_m_Internet_Connection_Disable_Enable_Node"/> class.
        /// </summary>
        public _m_Internet_Connection_Disable_Enable_Node()
        {
            RuleName = "";
            StartTime = DateTime.Now;
            EndTime = DateTime.Now;
            Action = true;
            Eth_TargetName = "All";
            RuleStatus = true;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="_m_Internet_Connection_Disable_Enable_Node"/> class.
        /// </summary>
        /// <param name="_RuleName">Name of the rule.</param>
        /// <param name="_StartTime">The start time.</param>
        /// <param name="_EndTime">The end time.</param>
        /// <param name="_Action">if set to <c>true</c> [action].</param>
        /// <param name="_Eth_TargetName">Name of the eth target.</param>
        /// <param name="_RuleStatus">if set to <c>true</c> [rule status].</param>
        public _m_Internet_Connection_Disable_Enable_Node(string _RuleName, DateTime _StartTime,DateTime _EndTime,bool _Action=false,string _Eth_TargetName="All",bool _RuleStatus=true)
        {
            RuleName = _RuleName;
            StartTime = _StartTime;
            EndTime = _EndTime;
            Action = _Action;
            Eth_TargetName = _Eth_TargetName;
            RuleStatus = _RuleStatus;
        }
        /// <summary>
        /// The identifier
        /// </summary>
        int _Id;
        /// <summary>
        /// The rule name
        /// any name but it is important and not be duplicated
        /// </summary>
        string _RuleName;
        /// <summary>
        /// The start time for starting rule execution
        /// </summary>
        DateTime _StartTime;
        /// <summary>
        /// The end time for end of execution
        /// </summary>
        DateTime _EndTime;
        /// <summary>
        /// The action what action you need disable or enable for disable set false for enable set true
        /// </summary>
        bool _Action;
        /// <summary>
        /// The eth target name; name of eth like Ethernet0 or Ethernet1 or Kerio Virtual Network or Local Area Connection
        /// if eth name is set to All the action execute for all eth
        /// </summary>
        string _Eth_TargetName;
        /// <summary>
        /// The rule status; is activated or not;not impliment yet
        /// </summary>
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

            public string Eth_TargetName
        {
                get { return _Eth_TargetName; }
                set { _Eth_TargetName = value; }
            }
            public bool RuleStatus
        {
                get { return _RuleStatus; }
                set { _RuleStatus = value; }
            }
    }
}
