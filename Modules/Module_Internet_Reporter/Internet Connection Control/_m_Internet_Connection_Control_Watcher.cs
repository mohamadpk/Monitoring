using Module_Internet_Reporter.Internet_Connection_Control.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;

namespace Module_Internet_Reporter.Internet_Connection_Control
{
    /// <summary>
    /// control the network connections by ruls
    /// we have whatcher for this solution
    /// what is the rule? rule is descripted in _m_Internet_Connection_Disable_Enable_Node
    /// </summary>
    public class _m_Internet_Connection_Control_Watcher
    {
        //TODO disable enable rule
        /// <summary>
        /// Adds the rule to list.
        /// </summary>
        /// <param name="RuleToAdd">The rule to add.</param>
        public void AddRuleToList(_m_Internet_Connection_Disable_Enable_Node RuleToAdd)
        {
                micdeDB.AddRuleToDB(RuleToAdd);
                RuleList.Add(RuleToAdd);
        }

        /// <summary>
        /// Removes the rule from list.
        /// </summary>
        /// <param name="RuleToRemove">The rule to remove.</param>
        /// <returns></returns>
        public bool RemoveRuleFromList(_m_Internet_Connection_Disable_Enable_Node RuleToRemove)
        {
                micdeDB.RemoveRuleFromDB(RuleToRemove);
                return RuleList.Remove(RuleToRemove);
        }

        /// <summary>
        /// The keep running publicly on this class
        /// when a _m_Internet_Connection_Control_Watcher object created and StartWatching called on a new thread
        /// the new thread work StartWatching end after create watcher this bool variable 
        /// keep running a while on end of StartWatching function
        /// for kill the thread set the keep running to false is enough
        /// </summary>
        public bool keepRunning = true;

        /// <summary>
        /// The micde database object of ruls database
        /// </summary>
        _m_Internet_Connection_Disable_Enable_DB micdeDB;
        /// <summary>
        /// The rule list on memory for speed access
        /// </summary>
        List<_m_Internet_Connection_Disable_Enable_Node> RuleList;
        /// <summary>
        /// Initializes a new instance of the <see cref="_m_Internet_Connection_Control_Watcher"/> class.
        /// </summary>
        public _m_Internet_Connection_Control_Watcher()
        {
            micdeDB = new _m_Internet_Connection_Disable_Enable_DB();
            RuleList = micdeDB.GetAllRules();
        }
        /// <summary>
        /// Starts the watching.
        /// </summary>
        public void StartWatching()
        {
            while (keepRunning)
            {
                //TODO change 1000 public var for control sleep time from outside
                System.Threading.Thread.Sleep(1000);
                DateTime NowTime = DateTime.Now;
                foreach (_m_Internet_Connection_Disable_Enable_Node rule in RuleList)
                {
                    int StartTimeCompareResualt = NowTime.CompareTo(rule.StartTime);
                    int EndTileCompareResualt = NowTime.CompareTo(rule.EndTime);
                    if (StartTimeCompareResualt >= 0 && EndTileCompareResualt <= 0 && rule.RuleStatus == true)
                    {
                        if (rule.Action == true && rule.Eth_TargetName != "All")
                        {
                            _m_Internet_Connection_Disable_Enable.OnEnableInternetConnection(rule.Eth_TargetName);
                        }
                        else if (rule.Action == false && rule.Eth_TargetName != "All")
                        {
                            _m_Internet_Connection_Disable_Enable.OnDisableInternetConnection(rule.Eth_TargetName);
                        }
                        else//Eth_TargetName==All
                        {
                            if (rule.Action == true)
                            {
                                _m_Internet_Connection_Disable_Enable.OnEnableAllInternetConnection();
                            }
                            else
                            {
                                _m_Internet_Connection_Disable_Enable.OnDisableAllInternetConnection();
                            }
                        }
                    }
                }
            }
        }

    }
}
