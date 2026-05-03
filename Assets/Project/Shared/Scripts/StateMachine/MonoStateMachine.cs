using UnityEngine;

public class MonoStateMachine : MonoBehaviour
{
    [SerializeField] private MonoState current;
    [SerializeField] private bool runOnEnable;
    [SerializeField] private bool runOnStart;
    private MonoState _default;

    public MonoState Current => current;

    public void Run(MonoState state) {
        if (current == state)
            return;
        if (current)
            current.ExitState();
        current = state;
        current.StartState();
    }

    private void Awake()
    {
        _default = current;
    }

    private void Start()
    {
        if (runOnStart && !runOnEnable)
        {
            current = null;
            Run(_default);
        }
    }

    private void OnEnable()
    {
        if (runOnEnable)
        {
            current = null;
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

