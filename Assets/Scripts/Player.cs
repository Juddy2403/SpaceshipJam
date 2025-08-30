using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 1.5f;
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _friction = 2f;
    [SerializeField] private SatteliteManager _satteliteManager;
    [SerializeField] private float _minLoseControlDistance = 2f;
    [SerializeField] private float _maxLoseControlDistance = 5f;
    private Player_IA playerInput;
    private Rigidbody2D _rigidBody;
    private Vector2 _startPosition;
    private Vector2 _lastInputDirection = Vector2.up;
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
        _startPosition = transform.position;
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
        var closestSatelite = _satteliteManager.GetClosestSatelite(transform.position);
        float distanceToSatelite = Vector2.Distance(transform.position, closestSatelite.position);

        if (moveInput.magnitude >= 0.5f)
        {
            // Lose control if too far from sattelite
            if (distanceToSatelite > _minLoseControlDistance)
            {
                float t = Mathf.InverseLerp(_minLoseControlDistance, _maxLoseControlDistance, distanceToSatelite);
                if (Random.value < t)
                {
                    // Replace moveInput with a random direction
                    float randomAngle = Random.Range(0f, 360f);
                    moveInput = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
                }
            }
            // Accelerate
            _rigidBody.AddForce(moveInput.normalized * _speed);

            // Clamp velocity to max speed
            if (_rigidBody.linearVelocity.magnitude > _maxSpeed)
            {
                _rigidBody.linearVelocity = _rigidBody.linearVelocity.normalized * _maxSpeed;
            }
            _lastInputDirection = moveInput.normalized;
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
        if (distanceToSatelite <= _minLoseControlDistance)
        {
            float angle = Mathf.Atan2(_lastInputDirection.y, _lastInputDirection.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

    }
    public void Explode()
    {
        transform.position = _startPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;
        var direction = transform.up.normalized;
        var otherDirection = collision.transform.up.normalized;
        // Dot product: >0 means facing, <0 means facing away
        float dot = Vector2.Dot(direction, otherDirection);
        if (dot < -0.5f) // Adjust threshold as needed (0.5 means roughly within 60 degrees)
        {
            Explode();
        }
    }
}
