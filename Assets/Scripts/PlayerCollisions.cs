using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    CapsuleCollider2D rb;
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;

    RaycastHit2D[] groundHits = new RaycastHit2D[5];

    private bool _isGrounded;
    public bool IsGrounded
    {
        get { return _isGrounded; }
        set { _isGrounded = value; }
    }

    private void Awake()
    {
        rb = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IsGrounded = rb.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
    }
}
