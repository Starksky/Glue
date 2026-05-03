using SaintsField;
using UnityEngine;

namespace Project.Shared.Scripts.StateMachine
{
    [RequireComponent(typeof(MonoState))]
    public abstract class BaseState : MonoBehaviour
    {
        [SerializeField, ReadOnly, GetComponent(typeof(MonoState))] 
        private MonoState monoState;

        private void OnEnable()
        {
            monoState.EventStart.AddListener(OnStateStart);
            monoState.EventUpdate.AddListener(OnStateUpdate);
            monoState.EventFixedUpdate.AddListener(OnStateFixedUpdate);
            monoState.EventExit.AddListener(OnStateExit);
        }
        
        protected virtual void OnStateStart(){}
        protected virtual void OnStateUpdate(){} 
        protected virtual void OnStateFixedUpdate(){} 
        protected virtual void OnStateExit(){} 
    }
}