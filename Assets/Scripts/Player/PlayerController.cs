using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool player1;
    public bool player2;

    public float rotationSpeed;
    public Vector2 RotationDirection { get; private set; }

    public bool CanControl { get; private set; } = true;
    
    
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
        }

        if (player2)
        {
            _input.Player2.Enable();
            _input.Player2.Rotate.performed += ProcessInput;
            _input.Player2.Rotate.canceled += ProcessInput;
        }
    }

    private void OnDisable()
    {
        if (player1)
        {
            _input.Player1.Rotate.performed -= ProcessInput;
            _input.Player1.Rotate.canceled -= ProcessInput;
            
            _input.Player1.Disable();
        }

        if (player2)
        {
            _input.Player2.Rotate.performed -= ProcessInput;
            _input.Player2.Rotate.canceled -= ProcessInput;
            
            _input.Player2.Disable();
        }
    }

    private void Update()
    {
        Rotate();
    }
    
    void ProcessInput(InputAction.CallbackContext context)
    {
        if (player1)
            RotationDirection = _input.Player1.Rotate.ReadValue<Vector2>();

        if (player2)
            RotationDirection = _input.Player2.Rotate.ReadValue<Vector2>();
    }

    void Rotate()
    {
        if(!CanControl)
            return;
        
        transform.Rotate(new Vector3(0,0,1), rotationSpeed * -RotationDirection.x * Time.deltaTime);
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
