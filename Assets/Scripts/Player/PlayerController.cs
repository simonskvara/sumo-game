using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool player1;
    public bool player2;

    public float rotationSpeed;
    public Vector2 RotationDirection { get; private set; }

    private bool _canControl = true;

    private bool _isCharging;
    private float _chargeTime;
    public float chargingSpeed;
    
    
    private InputSystem_Actions _input;

    private void Awake()
    {
        _input = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        if (player1)
        {
            _input.Player1.Enable();
            _input.Player1.Rotate.performed += ProcessInput;
            _input.Player1.Rotate.canceled += ProcessInput;

            _input.Player1.Charge.started += ChargeAttack;
            _input.Player1.Charge.canceled += ChargeAttack;
        }

        if (player2)
        {
            _input.Player2.Enable();
            
        }
    }

    private void OnDisable()
    {
        if (player1)
        {
            _input.Player1.Rotate.performed -= ProcessInput;
            _input.Player1.Rotate.canceled -= ProcessInput;
            
            _input.Player1.Charge.started -= ChargeAttack;
            _input.Player1.Charge.canceled -= ChargeAttack;
            _input.Player1.Disable();
        }

        if (player2)
        {
            
            _input.Player2.Disable();
        }
    }

    private void Update()
    {
        Rotate();

        if (_isCharging)
        {
            _chargeTime += Time.deltaTime * chargingSpeed;
            _chargeTime = Mathf.Clamp01(_chargeTime);
        }
    }
    
    void ProcessInput(InputAction.CallbackContext context)
    {
        if(!_canControl)
            return;
        
        if (player1)
            RotationDirection = _input.Player1.Rotate.ReadValue<Vector2>();

        if (player2)
            RotationDirection = _input.Player2.Rotate.ReadValue<Vector2>();
    }

    void Rotate()
    {
        transform.Rotate(new Vector3(0,0,1), rotationSpeed * -RotationDirection.x * Time.deltaTime);
    }

    void ChargeAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isCharging = true;
        }

        if (context.canceled)
        {
            _isCharging = false;
            // start charge attack
        }
    }

    public void DisableControls()
    {
        _canControl = false;
        RotationDirection = Vector2.zero;
    }

    public void EnableControls()
    {
        _canControl = true;
    }
    
    
}
