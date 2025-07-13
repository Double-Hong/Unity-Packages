using System;
using System.Collections.Generic;
using UnityEngine;

namespace myh
{
    public class GlobalEvent : MonoBehaviour
    {
        private static GlobalEvent _instance;
        
        private Dictionary<string, EventHandle> eventDict;

        public static GlobalEvent Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("GlobalEvent");
                    _instance = go.AddComponent<GlobalEvent>();
                    DontDestroyOnLoad(go);
                }

                return _instance;
            }
        }

        public void AddEvent(string eventId, EventHandle method)
        {
            eventDict ??= new Dictionary<string, EventHandle>();
            if (eventDict.TryGetValue(eventId,out var existing))
            {
                eventDict[eventId] += method;
            }
            else
            {
                eventDict[eventId] = method;
            }
            
        }
        
        public void DispatchEvent(string eventId, params object[] args)
        {
            if (eventDict != null && eventDict.TryGetValue(eventId, out var callback))
            {
                try
                {
                    callback(args);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[GlobalEvent] 事件 {eventId} 执行失败: {e}");
                }
            }
            else
            {
                Debug.LogWarning($"[GlobalEvent] 未找到事件: {eventId}");
            }
        }

        public void RemoveEvent(string eventId, EventHandle method)
        {
            if (eventDict == null) return;

            if (eventDict.TryGetValue(eventId, out var existing))
            {
                existing -= method;
                if (existing == null)
                {
                    eventDict.Remove(eventId);
                }
                else
                {
                    eventDict[eventId] = existing;
                }
            }
        }
        
    }
}