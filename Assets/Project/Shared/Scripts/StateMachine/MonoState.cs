using SaintsField;
using SaintsField.Playa;
using UnityEngine;
using UnityEngine.Events;

public class MonoState : MonoBehaviour
{
    [SerializeField, ReadOnly, GetComponentInParent(typeof(MonoStateMachine))] 
    private MonoStateMachine monoStateMachine;
    
    [LayoutStart("Events", ELayout.Foldout)]
    public UnityEvent EventStart;
    public UnityEvent EventUpdate;
    public UnityEvent EventFixedUpdate;
    public UnityEvent EventExit;

    private void Awake()
    {
        if (!monoStateMachine)
            monoStateMachine = GetComponentInParent<MonoStateMachine>();
    }

    public void Run() => monoStateMachine.Run(this);
    
    public void StartState() => EventStart?.Invoke();
    public void UpdateState() => EventUpdate?.Invoke();
    public void FixedUpdateState() => EventFixedUpdate?.Invoke();
    public void ExitState() => EventExit?.Invoke();
}
