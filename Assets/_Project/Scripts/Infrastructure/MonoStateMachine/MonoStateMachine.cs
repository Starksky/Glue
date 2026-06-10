using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.MonoStateMachine
{
    public class MonoStateMachine : MonoBehaviour
    {
        [SerializeField] private MonoState current;
        [SerializeField] private bool runOnEnable;
        [SerializeField] private bool runOnStart;
        private MonoState _default;

        public MonoState Current => current;

        public void Run(MonoState state) 
        {
            if (current == state)
                return;

            if (current != null
                && !current.Transitions.Contains(state))
            {
                Debug.Log($"State {current.name} don't transition to {state.name}");
                return;
            }
            
            if (current)
                current.ExitState();
            
            current = state;
            Debug.Log($"Current state: {current.name}");
            current.StartState();
        }

        private void Awake()
        {
            _default = current;
            current = null;
        }

        private void Start()
        {
            if (runOnStart && !runOnEnable)
            {
                current = current == _default ? null : current;
                Run(_default);
            }
        }

        private void OnEnable()
        {
            if (runOnEnable)
            {
                current = current == _default ? null : current;
                Run(_default);
            }
        }

        private void Update()
        {
            if (current)
                current.UpdateState();
        }

        private void FixedUpdate()
        {
            if (current)
                current.FixedUpdateState();
        }

        private void OnDestroy()
        {
            if (current)
                current.ExitState();
        }
    }
}

