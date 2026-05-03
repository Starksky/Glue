using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D), 
    typeof(CircleCollider2D), 
    typeof(PlayerInput))]
public class OldSlingshotController : MonoBehaviour
{
    [Header("Визуал")]
    [SerializeField] private Transform hand;
    [SerializeField] private LineRenderer line;
    [SerializeField] private LineRenderer direct;
    [SerializeField] private float minWidth = 0.2f;
    [SerializeField] private float directHeight = 1f;
    
    [Space]
    [Header("Настройки натяжения")]
    [SerializeField] private float maxDragDistanceBody = 0.1f;
    [SerializeField] private float maxDragDistance = 2f;
    [SerializeField] private float forceMultiplier = 15f;
    [SerializeField] private float time = 0.5f;
    [SerializeField] private float timeToKinematic = 0.05f;
    
    
    private Rigidbody2D rb;
    private CircleCollider2D col;
    private Camera mainCamera;
    
    // Input
    private PlayerInput playerInput;
    private InputAction dragAction;
    private InputAction dragPositionAction;
    
    // Состояние
    private bool isDragging = false;
    private Vector2 _lastDragPoint;
    private Vector2 startDragPoint;
    private Vector2 dragVector;
    private Vector2 startDragMouse;
    private float currentStrength = 0f;
    private float _timer;
    private bool _isGravity = true;
    private Vector2 _lastContactPosition;
    private Vector2 _currentAddForce;
    private Coroutine _toKinematic;

    private bool IsGravity
    {
        get => _isGravity;
        set
        {
            _isGravity = value;
            if (!_isGravity)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0;
                rb.linearDamping = 0f;
                _currentAddForce = Vector2.zero;
                
                
                if (_toKinematic != null)
                    StopCoroutine(_toKinematic);
                _toKinematic = StartCoroutine(StartForKinematic());
                //rb.constraints = RigidbodyConstraints2D.FreezeAll;
                //rb.bodyType = RigidbodyType2D.Kinematic;
            }
            else
            {
                if (_toKinematic != null)
                    StopCoroutine(_toKinematic);
                
                rb.linearDamping = 0f;
                _currentAddForce = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.constraints = RigidbodyConstraints2D.None;
            }
        }
    }

    private IEnumerator StartForKinematic()
    {
        yield return new WaitForSeconds(timeToKinematic);
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        playerInput = GetComponent<PlayerInput>();
        mainCamera = Camera.main;
    }
    
    void OnEnable()
    {
        dragAction = playerInput.actions["Drag"];
        dragPositionAction = playerInput.actions["DragPosition"];
        
        if (hand)
            hand.gameObject.SetActive(false);
        if (direct)
            direct.gameObject.SetActive(false);
        
        if (dragAction != null)
        {
            dragAction.started += OnDragStarted;
            dragAction.canceled += OnDragCanceled;
            dragAction.Enable();
        }
        
        if (dragPositionAction != null)
            dragPositionAction.Enable();
    }
    
    void OnDisable()
    {
        if (dragAction != null)
        {
            dragAction.started -= OnDragStarted;
            dragAction.canceled -= OnDragCanceled;
            dragAction.Disable();
        }
        
        if (dragPositionAction != null)
            dragPositionAction.Disable();
    }
    private void Update()
    {
        if (_isGravity)
            _timer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        rb.AddForce(_currentAddForce, ForceMode2D.Force);
        OnDragPerformed();
    }

    #region Input Handlers
    
    private void OnDragStarted(InputAction.CallbackContext context)
    {
        if (rb.linearVelocity.magnitude > 0.2f && _currentAddForce.magnitude == 0f)
            return;

        Vector2 mousePos = GetMousePosition();
        if (!col.OverlapPoint(mousePos)) return;
        
        if (hand)
            hand.gameObject.SetActive(true);
        if (direct)
            direct.gameObject.SetActive(true);
        
        isDragging = true;
        startDragPoint = rb.position;
        startDragMouse = mousePos;
        _lastDragPoint = Vector2.zero;

        //if (_currentAddForce.magnitude == 0f)
        //    IsGravity = false;
    }
    
    private void OnDragPerformed()
    {
        if (!isDragging) return;
        
        Vector2 currentMouse = GetMousePosition();
        var position = rb.position;
        position += _lastDragPoint;
        
        dragVector = position - currentMouse;
        var dragHand = dragVector;
        
        float distance = dragVector.magnitude;
        if (distance > maxDragDistanceBody)
            dragVector = dragVector.normalized * maxDragDistanceBody;
        if (distance > maxDragDistance)
        {
            distance = maxDragDistance;
            dragHand = dragHand.normalized * maxDragDistance;
        }
        
        currentStrength = distance / maxDragDistance;
        
        // Перемещаем тело
        _lastDragPoint = dragVector;
        position -= _lastDragPoint;
        rb.position = position;

        if (hand)
            hand.position = position - dragHand;

        if (direct)
            direct.SetPosition(0, dragHand.normalized * directHeight);
        
        if (line)
        {
            var width = Mathf.Clamp(1f - (currentStrength - minWidth), 0f, 1f);
            var curve = line.widthCurve;
            var keys = curve.keys;
            keys[1].value = width;
            curve.SetKeys(keys);
            line.widthCurve = curve;
            
            var pos = -dragHand + dragHand.normalized * 0.25f;
            for (int i = 0; i < line.positionCount; i++)
            {
                float t = (float)i / (line.positionCount - 1);
                float centered = t - 0.5f;
                float sign = Mathf.Sign(centered);
                float absPowered = (float)Mathf.Pow(Mathf.Abs(centered) * 2, 3f);
                float tNew = 0.5f + sign * absPowered / 2f;
                line.SetPosition(i, Vector3.Lerp(Vector3.zero, pos, tNew));
            }
            //line.SetPosition(line.positionCount - 1, pos);
        }
    }
    
    private void OnDragCanceled(InputAction.CallbackContext context)
    {
        if (!isDragging) return;

        rb.position += _lastDragPoint;

        _timer = 0;
        isDragging = false;
        IsGravity = true;
        
        if (hand)
        {
            hand.position = startDragPoint;
            hand.gameObject.SetActive(false);
        }
        
        if (direct)
            direct.gameObject.SetActive(false);
        
        if (line)
        {
            for (int i = 0; i < line.positionCount; i++)
                line.SetPosition(i, Vector3.zero);
        }
        
        // Бросок в противоположную сторону
        Vector2 throwForce = dragVector.normalized * currentStrength * forceMultiplier;
        rb.AddForce(throwForce, ForceMode2D.Impulse);
        rb.AddTorque(-Mathf.Sign(throwForce.x) * throwForce.magnitude, ForceMode2D.Impulse);

        dragVector = Vector2.zero;
        currentStrength = 0f;
    }
    
    #endregion
    
    
    #region Вспомогательное
    
    private Vector2 GetMousePosition()
    {
        Vector2 screenPos = dragPositionAction.ReadValue<Vector2>();
        return mainCamera.ScreenToWorldPoint(screenPos);
    }
    
    public void ResetPosition(Vector2 newPosition)
    {
        if (_toKinematic != null)
            StopCoroutine(_toKinematic);
        
        if (hand)
            hand.gameObject.SetActive(false);
        if (direct)
            direct.gameObject.SetActive(false);
        
        _isGravity = true;
        _timer = 0;
        _currentAddForce = Vector2.zero;
        isDragging = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.linearDamping = 0f;
        rb.position = newPosition;
        transform.position = newPosition;
        dragVector = Vector2.zero;
        currentStrength = 0f;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.None;
    }

    public void Detach()
    {
        if (_isGravity)
            return;
        
        if (_toKinematic != null)
            StopCoroutine(_toKinematic);
        
        _isGravity = true;
        _timer = 0;
        //rb.linearVelocity = Vector2.zero;
        //rb.angularVelocity = 0f;
        rb.linearDamping = 10f;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.None;
        
        _currentAddForce = (-(Vector2)transform.position - _lastContactPosition).normalized * Physics2D.gravity.y * 0.5f;
    }

    public bool IsDragging => isDragging;
    public float CurrentStrength => currentStrength;
    
    #endregion
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.isTrigger || isDragging)
            return;
        
        if (_timer > time)
            IsGravity = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger || isDragging)
            return;

        _lastContactPosition = other.ClosestPoint(transform.position);
        
        if (_timer > time)
            IsGravity = false;
    }
}