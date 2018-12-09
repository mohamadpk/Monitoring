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
    public class _s_Custom_HubProxy
    {
        HubProxy _HubProxy;

        public _s_Custom_HubProxy(HubProxy hubproxy)
        {
            _HubProxy = hubproxy;
        }

        public JToken this[string name]
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public JsonSerializer JsonSerializer
        {
            get { return _HubProxy.JsonSerializer; }
        }

        public Subscription Subscribe(string eventName)
        {
            return _HubProxy.Subscribe(eventName);
        }

        public Task Invoke(string method, params object[] args)
        {
            return _HubProxy.Invoke(method, args);
        }

        public Task<T> Invoke<T>(string method, params object[] args)
        {
            return _HubProxy.Invoke<T>(method, args);
        }

        public Task Invoke<T>(string method, Action<T> onProgress, params object[] args)
        {
            return _HubProxy.Invoke<T>(method, onProgress, args);
        }
        public Task<TResult> Invoke<TResult, TProgress>(string method, Action<TProgress> onProgress, params object[] args)
        {
            return _HubProxy.Invoke<TResult, TProgress>(method, onProgress, args);
        }

        public void InvokeEvent(string eventName, IList<JToken> args)
        {
            _HubProxy.InvokeEvent(eventName, args);
        }
        public T GetValue<T>(string name)
        {
            return _HubProxy.GetValue<T>(name);
        }
        public  IObservable<IList<JToken>> Observe(string eventName)
        {
            return _HubProxy.Observe(eventName);
        }
        public IDisposable On(string eventName, Action onData)
        {
            return _HubProxy.On(eventName, onData);
        }
        public IDisposable On(string eventName, Action<dynamic> onData)
        {
            return _HubProxy.On(eventName, onData);
        }
        public IDisposable On<T>(string eventName, Action<T> onData)
        {
            return _HubProxy.On(eventName, onData);
        }
        public IDisposable On<T1, T2>(string eventName, Action<T1, T2> onData)
        {
            return _HubProxy.On(eventName, onData);
        }
        public IDisposable On<T1, T2, T3>(string eventName, Action<T1, T2, T3> onData)
        {
            return _HubProxy.On(eventName, onData);
        }
        public IDisposable On<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> onData)
        {
            return _HubProxy.On(eventName, onData);
        }
        public IDisposable On<T1, T2, T3, T4, T5>(string eventName, Action<T1, T2, T3, T4, T5> onData)
        {
            return _HubProxy.On(eventName, onData);
        }
        public IDisposable On<T1, T2, T3, T4, T5, T6>(string eventName, Action<T1, T2, T3, T4, T5, T6> onData)
        {
            return _HubProxy.On(eventName, onData);
        }
        public IDisposable On<T1, T2, T3, T4, T5, T6, T7>(string eventName, Action<T1, T2, T3, T4, T5, T6, T7> onData)
        {
            return _HubProxy.On(eventName, onData);
        }
    }
}
