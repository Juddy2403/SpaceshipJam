using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] float _gravityStrength = 9.81f;
    [SerializeField] SpriteRenderer _resourceSprite;
    [SerializeField] private bool _hasResources = true;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;
        var player = collision.collider.GetComponent<Player>();
        //if the player is facing the planet, explode
        if (player == null) return;
        // Get player's facing direction (assuming transform.up is forward)
        Vector2 playerForward = player.transform.up.normalized;
        // Get direction to planet
        Vector2 toPlanet = ((Vector2)transform.position - (Vector2)player.transform.position).normalized;

        // Dot product: >0 means facing, <0 means facing away
        float dot = Vector2.Dot(playerForward, toPlanet);
        if (dot > 0.5f) // Adjust threshold as needed (0.5 means roughly within 60 degrees)
        {
            player.Explode();
        }
    }

    public void HarvestResources()
    {
        if (!_hasResources) return;
        _hasResources = false;
        _resourceSprite.enabled = false;
    }
}
