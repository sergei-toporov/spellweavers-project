using UnityEngine;
using UnityEngine.Events;

namespace Spellweavers
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] protected GameEventSO Event;
        [SerializeField] protected UnityEvent Response;

        protected void OnEnable()
        {
            if (Event != null)
            {
                Event.AddListener(this);
            }
        }

        protected void OnDisable()
        {
            if (Event != null)
            {
                Event.RemoveListener(this);
            }            
        }

        public void OnDispatch()
        {
            if (Response != null)
            {
                Response.Invoke();
            }            
        }
    }
}
