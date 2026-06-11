using System.Collections.Generic;
using SaintsField;
using SaintsField.Playa;
using UnityEngine;
using UnityEngine.Events;

namespace _Project._Common.Scripts.Infrastructure.MonoStateMachine
{
    public class MonoState : MonoBehaviour
    {
        [SerializeField, ReadOnly] 
        private MonoStateMachine monoStateMachine;
        
        [SerializeField]
        private MonoState[] transitions;
    
        public IReadOnlyList<MonoState> Transitions => transitions;
        
        [LayoutStart("Events", ELayout.Foldout)]
        public UnityEvent EventEnable;
        public UnityEvent EventStart;
        public UnityEvent EventUpdate;
        public UnityEvent EventFixedUpdate;
        public UnityEvent EventExit;

        private void OnValidate()
        {
            if (!monoStateMachine)
                monoStateMachine = GetComponentInParent<MonoStateMachine>();
        }

        private void Awake()
        {
            if (!monoStateMachine)
                monoStateMachine = GetComponentInParent<MonoStateMachine>();
        }

        public void Run() => monoStateMachine.Run(this);

        public void OnEnable() => EventEnable?.Invoke();
        public void StartState() => EventStart?.Invoke();
        public void UpdateState() => EventUpdate?.Invoke();
        public void FixedUpdateState() => EventFixedUpdate?.Invoke();
        public void ExitState() => EventExit?.Invoke();
    }
}
