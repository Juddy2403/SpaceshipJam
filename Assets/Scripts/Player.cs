using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 1.5f;
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _friction = 2f;
    [SerializeField] private SatteliteManager _satteliteManager;
    [SerializeField] private float _minLoseControlDistance = 2f;
    [SerializeField] private float _maxLoseControlDistance = 5f;
    [SerializeField] private float _craftingTime = 1f;
    [SerializeField] private float _miningTime = 1f;
    [SerializeField] private SpriteRenderer _resourceSprite;
    [SerializeField] private SpriteRenderer _satteliteSprite;
    private int _playerNumber = 0;
    public int PlayerNumber => _playerNumber;
    private bool _hasResources = false;
    private bool _hasSattelite = false;
    private Player_IA _playerInput;
    private Rigidbody2D _rigidBody;
    private Vector2 _startPosition;
    private Vector2 _lastInputDirection = Vector2.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _playerInput = new Player_IA();
        _playerInput.Enable();
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        ManageInput();
        _startPosition = transform.position;
        _playerInput.Movement.Reset.performed += ctx => { Explode(); };
    }

    private void ManageInput()
    {
        // fetch the Gamepad list from InputManager
        var gamepads = InputManager.Instance.ConnectedGamepads;
        // assign the first gamepad to the player if available
        var usedGamepads = InputManager.Instance.GamepadConnectedStatus;
        if (usedGamepads[0] == false)
        {
            usedGamepads[0] = true;
            _playerInput.devices = new[] { gamepads[0] };
        }
        else if (usedGamepads[1] == false && gamepads.Count > 1)
        {
            usedGamepads[1] = true;
            _playerNumber = 1;
            _playerInput.devices = new[] { gamepads[1] };
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 moveInput = _playerInput.Movement.Move.ReadValue<Vector2>();
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
        if (distanceToSatelite <= _minLoseControlDistance && _lastInputDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(_lastInputDirection.y, _lastInputDirection.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

    }
    public void Explode()
    {
        transform.position = _startPosition;
        _hasResources = false;
        _resourceSprite.enabled = false;
        _hasSattelite = false;
        _satteliteSprite.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float dot;
        if (collision.collider.CompareTag("Player"))
        {
            var direction = transform.up.normalized;
            var otherDirection = collision.transform.up.normalized;
            // Dot product: >0 means facing, <0 means facing away
            dot = Vector2.Dot(direction, otherDirection);
            if (dot < -0.5f) // Adjust threshold as needed (0.5 means roughly within 60 degrees)
            {
                Explode();
            }
        }
        Planet planet = collision.collider.GetComponent<Planet>();
        if (planet == null) return;
        Vector2 playerForward = transform.up.normalized;
        // Get direction to planet
        Vector2 toPlanet = ((Vector2)planet.transform.position - (Vector2)transform.position).normalized;

        // Dot product: >0 means facing, <0 means facing away
        dot = Vector2.Dot(playerForward, toPlanet);
        if (dot > 0.5f) // Adjust threshold as needed (0.5 means roughly within 60 degrees)
        {
            Explode();
            return;
        }
        if (planet.IsHomePlanet && _hasResources)
        {
            _playerInput.Movement.Disable();
            Invoke(nameof(DoneCraftingSattelite), _craftingTime);
            return;
        }
        if (!planet.IsHomePlanet && _hasSattelite)
        {
            _hasSattelite = false;
            _satteliteSprite.enabled = false;
            _satteliteManager.AddSatelite(collision.transform);
            planet.AddSattelite();
            return;
        }
        if (_hasResources == false)
        {
            if (!planet.HarvestResources()) return;
            Invoke(nameof(DonePickingUpResources), _miningTime);
            _playerInput.Movement.Disable();
        }
    }

    private void DonePickingUpResources()
    {
        _hasResources = true;
        _resourceSprite.enabled = true;
        _playerInput.Movement.Enable();
    }

    private void DoneCraftingSattelite()
    {
        _hasResources = false;
        _resourceSprite.enabled = false;
        _hasSattelite = true;
        _satteliteSprite.enabled = true;
        _playerInput.Movement.Enable();
    }
}
