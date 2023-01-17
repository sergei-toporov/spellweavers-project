using System.Collections.Generic;
using UnityEngine;

namespace Spellweavers
{
    [CreateAssetMenu(fileName = "new_game_event", menuName = "Custom Assets/Game Event", order = 55)]
    public class GameEventSO :ScriptableObject
    {
        [SerializeField] protected List<GameEventListener> listeners = new List<GameEventListener>();

        public virtual void Dispatch()
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].OnDispatch();
            }
        }

        public virtual void AddListener(GameEventListener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public virtual void RemoveListener(GameEventListener listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
        }
    }
}