using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 1.5f;
    [SerializeField] float _maxSpeed = 5f;
    [SerializeField] float _friction = 2f;

    private Player_IA playerInput;
    private Rigidbody2D _rigidBody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        playerInput = new Player_IA();
        playerInput.Enable();
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        ManageInput();
    }

    private void ManageInput()
    {
        // fetch the Gamepad list from InputManager
        var gamepads = InputManager.Instance.ConnectedGamepads;
        // assign the first gamepad to the player if available
        var availableGamepads = InputManager.Instance.GamepadConnectedStatus;
        if (availableGamepads[0] == false)
        {
            availableGamepads[0] = true;
            playerInput.devices = new[] { gamepads[0] };
        }
        else if (availableGamepads[1] == false && gamepads.Count > 1)
        {
            availableGamepads[1] = true;
            playerInput.devices = new[] { gamepads[1] };
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 moveInput = playerInput.Movement.Move.ReadValue<Vector2>();

        if (moveInput.magnitude >= 0.1f)
        {
            // Accelerate
            _rigidBody.AddForce(moveInput.normalized * _speed);

            // Clamp velocity to max speed
            if (_rigidBody.linearVelocity.magnitude > _maxSpeed)
            {
                _rigidBody.linearVelocity = _rigidBody.linearVelocity.normalized * _maxSpeed;
            }
        }
        else
        {
            // Apply friction to simulate sliding
            if (_rigidBody.linearVelocity.magnitude > 0.1f)
            {
                _rigidBody.linearVelocity = Vector2.Lerp(_rigidBody.linearVelocity, Vector2.zero, _friction * Time.deltaTime);
            }
            else
            {
                _rigidBody.linearVelocity = Vector2.zero;
            }
        }

        // Rotate to face movement direction
        if (_rigidBody.linearVelocity.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(_rigidBody.linearVelocity.y, _rigidBody.linearVelocity.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
