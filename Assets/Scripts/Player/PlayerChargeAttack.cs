using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerChargeAttack : MonoBehaviour
{
    public PlayerController playerController;

    [Header("Dash Settings")]
    [Tooltip("How long it takes to get the highest dash power")]
    public float maxChargeTime;
    public float maxDashDistance;
    public float dashSpeed;
    public LineRenderer dashLine;

    public float maxKnockbackForce;
    
    private bool _isCharging;
    private float _currentCharge;
    private bool _isDashing;
    private Vector2 _dashDirection;

    public event Action OnDashStart;
    
    private InputSystem_Actions _input;
    
    public bool CanControl { get; private set; } = true;

    private void Awake()
    {
        _input = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        if (playerController.player1)
        {
            _input.Player1.Charge.Enable();
            _input.Player1.Charge.started += StartCharging;
            _input.Player1.Charge.canceled += ExecuteDash;
        }
        
        if (playerController.player2)
        {
            _input.Player2.Charge.Enable();
            _input.Player2.Charge.started += StartCharging;
            _input.Player2.Charge.canceled += ExecuteDash;
        }
    }

    private void OnDisable()
    {
        if (playerController.player1)
        {
            _input.Player1.Charge.started -= StartCharging;
            _input.Player1.Charge.canceled -= ExecuteDash;
            _input.Player1.Charge.Disable();
        }
        
        if (playerController.player2)
        {
            _input.Player2.Charge.started -= StartCharging;
            _input.Player2.Charge.canceled -= ExecuteDash;
            _input.Player2.Charge.Disable();
        }
    }

    private void Start()
    {
        dashLine.enabled = false;
    }

    private void Update()
    {
        _dashDirection = transform.up;
        
        if (_isCharging)
        {
            _currentCharge = Mathf.Clamp(_currentCharge + Time.deltaTime, 0, maxChargeTime);
            
            UpdateDashLine();
        }
    }

    void StartCharging(InputAction.CallbackContext context)
    {
        if(!playerController.CanControl)
            return;
        
        if(_isDashing)
            return;
        
        _isCharging = true;
        _currentCharge = 0f;
        dashLine.enabled = true;
    }

    void ExecuteDash(InputAction.CallbackContext context)
    {
        if(!playerController.CanControl)
            return;
        
        if (_isCharging)
        {
            OnDashStart?.Invoke();
            
            _isCharging = false;
            dashLine.enabled = false;
            StartCoroutine(Dashing());
        }
    }

    /// <summary>
    /// Get start and end position and lerp the position based on the duration set by the speed
    /// Makes the dashing speed consistent no matter the distance
    /// </summary>
    /// <returns></returns>
    IEnumerator Dashing()
    {
        _isDashing = true;
        playerController.DisableControls();

        float actualDistance = Mathf.Lerp(0, maxDashDistance, _currentCharge / maxChargeTime);
        Vector2 startPosition = transform.position;
        Vector2 endPosition = (Vector2)transform.position + _dashDirection * actualDistance;

        float duration = actualDistance / dashSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector2.Lerp(startPosition, endPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
        playerController.EnableControls();
        _currentCharge = 0f;
        _isDashing = false;
    }

    void UpdateDashLine()
    {
        if (!dashLine)
            return;
        
        

        float calculatedDistance = Mathf.Lerp(0, maxDashDistance, _currentCharge / maxChargeTime);

        var position = transform.position;
        dashLine.SetPosition(0, position);
        dashLine.SetPosition(1, (Vector2)position + _dashDirection * calculatedDistance);
    }
    
    public void CancelChargingAndDashing()
    {
        _isCharging = false;
        _isDashing = false;
        StopAllCoroutines();
        dashLine.enabled = false;
        playerController.EnableControls();
    }

    public void ApplyKnockBack(Rigidbody2D targetRb)
    {
        float chargeRatio = _currentCharge / maxChargeTime;
        float knockbackForce = Mathf.Lerp(0, maxKnockbackForce, chargeRatio);
        
        Vector2 forceDirection = (targetRb.transform.position - transform.position).normalized;
        
        targetRb.AddForce(forceDirection * knockbackForce, ForceMode2D.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * maxDashDistance);
    }

    public bool IsDashing()
    {
        return _isDashing;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && _isDashing)
        {
            Rigidbody2D targetRb = other.gameObject.GetComponent<Rigidbody2D>();
            
            ApplyKnockBack(targetRb);
            CancelChargingAndDashing();
        }
    }

    public void DisableControls()
    {
        CanControl = false;
    }

    public void EnableControls()
    {
        CanControl = true;
    }
}
