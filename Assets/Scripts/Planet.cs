using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] float _gravityStrength = 9.81f;
    [SerializeField] SpriteRenderer _resourceSprite;
    [SerializeField] SpriteRenderer _satteliteSprite;
    [SerializeField] SpriteRenderer _planetSprite;
    [SerializeField] List<Sprite> _possiblePlanetSprites;
    [SerializeField] private bool _hasResources = true;
    [SerializeField] private bool _isHomePlanet = false;
    [SerializeField] private CircleCollider2D _triggerCollider;
    [SerializeField] private CircleCollider2D _collider;
    private float _planetSpeed;
    public bool IsHomePlanet => _isHomePlanet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!_isHomePlanet)
        {
            _planetSprite.sprite = _possiblePlanetSprites[Random.Range(0, _possiblePlanetSprites.Count)];

            // Randomize scale
            float scale = Random.Range(0.06f, 0.15f);
            _planetSprite.transform.localScale = Vector3.one * scale;

            // Set collider radii
            if (_collider != null)
                _collider.radius = 4f * scale;
            if (_triggerCollider != null && _collider != null)
                _triggerCollider.radius = _collider.radius * 3f;
        }
        _planetSpeed = Random.Range(6f, 15f);

    }

    // Update is called once per frame
    void Update()
    {
        // rotate planet slowly
        _planetSprite.transform.Rotate(Vector3.forward * Time.deltaTime * _planetSpeed);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        var playerRigidbody = other.GetComponent<Rigidbody2D>();
        if (playerRigidbody == null) return;
        Vector2 gravitationalPull = (Vector2)transform.position - playerRigidbody.position;
        float gravityStrengthMultiplier = Mathf.Abs(_triggerCollider.radius - gravitationalPull.magnitude);
        playerRigidbody.AddForce(gravitationalPull.normalized * gravityStrengthMultiplier * _gravityStrength);
    }

    public bool HarvestResources()
    {
        if (!_hasResources) return false;
        _hasResources = false;
        _resourceSprite.enabled = false;
        return true;
    }

    public void AddSattelite(Sprite satteliteSprite)
    {
        _satteliteSprite.sprite = satteliteSprite;
        _satteliteSprite.enabled = true;
    }
}
