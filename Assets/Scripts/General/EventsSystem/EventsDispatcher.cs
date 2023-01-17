using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spellweavers
{
    public class EventsDispatcher : MonoBehaviour
    {

        protected static EventsDispatcher dispatcher;
        public static EventsDispatcher Dispatcher { get => dispatcher; }

        [SerializeField] GenericDictionary<string, GameEventSO> collection = new GenericDictionary<string, GameEventSO>();
        public GenericDictionary<string, GameEventSO> Collection { get => collection; }

        protected void Awake()
        {
            if (dispatcher != this && dispatcher != null)
            {
                Destroy(this);
            }
            else
            {
                dispatcher = this;
            }
        }
        
        public void Dispatch(string key)
        {
            if (collection.ContainsKey(key) && collection[key] != null)
            {
                collection[key].Dispatch();
            }
        }
    }
}

