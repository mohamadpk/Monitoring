using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Module_Out_Communication_Manager.HubProxyExtension
{
    public class _s_HubsManager
    {
        private Dictionary<string, _s_Custom_HubProxy> _Hubs;
        //public Dictionary<string, object> ListOfOnFunctions;
        //public void AddListenFunctionToHub(string HubName,string OnFunctionName,object OnFunctionPointer)
        //{
        //    _Hubs[HubName].On(OnFunctionName,(Action<dynamic>)OnFunctionPointer);
        //}
        public _s_HubsManager()
        {
            _Hubs = new Dictionary<string, _s_Custom_HubProxy>();
            //ListOfOnFunctions = new Dictionary<string, object>();
        }
        

        public Dictionary<string, _s_Custom_HubProxy> GetHubs()
        {
            return _Hubs;
        }
        public _s_Custom_HubProxy GetHub(string HubName)
        {
            _s_Custom_HubProxy CustomHubProxy;
            if(_Hubs.TryGetValue(HubName,out CustomHubProxy))
            {
                return CustomHubProxy;
            }
            return null;
        }
        public void AddHubsToConnection(HubConnection Connection)
        {
            for(int i=0;i<_Hubs.Count;i++)
            {
                _s_Custom_HubProxy CustomHubProxy = new _s_Custom_HubProxy((HubProxy)Connection.CreateHubProxy(_Hubs.Keys.ToList()[i]));
                _Hubs[_Hubs.Keys.ToList()[i]] = CustomHubProxy;
            }
        }
        public void AddHub(string HubName)
        {
            _Hubs.Add(HubName, null);
        }
        public void RemoveHub(string HubName)
        {
            _Hubs.Remove(HubName);
        }
    }
}
