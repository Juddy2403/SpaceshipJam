using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] float _gravityStrength = 9.81f;
    [SerializeField] SpriteRenderer _resourceSprite;
    [SerializeField] SpriteRenderer _satteliteSprite;
    [SerializeField] private bool _hasResources = true;
    [SerializeField] private bool _isHomePlanet = false;
    public bool IsHomePlanet => _isHomePlanet;
    private CircleCollider2D _collider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _collider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        var playerRigidbody = other.GetComponent<Rigidbody2D>();
        if (playerRigidbody == null) return;
        Vector2 gravitationalPull = (Vector2)transform.position - playerRigidbody.position;
        float gravityStrengthMultiplier = Mathf.Abs(_collider.radius - gravitationalPull.magnitude) ;
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
