using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    Animator animator;
    CapsuleCollider2D rb;
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;

    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];
    RaycastHit2D[] frontHits = new RaycastHit2D[5];
    RaycastHit2D[] backHits = new RaycastHit2D[5];
    RaycastHit2D[] distanceHits = new RaycastHit2D[5];

    Vector2 frontDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    Vector2 backDirection => gameObject.transform.localScale.x > 0 ? Vector2.left : Vector2.right;

    public float squishyness = 0.0f;

    
    [SerializeField]
    private bool _isGrounded;
    public bool IsGrounded
    {
        get { return _isGrounded; }
        set { _isGrounded = value; animator.SetBool("IsGrounded", value); }
    }

    [SerializeField]
    private bool _isCeiling;
    public bool IsCeiling
    {
        get { return _isCeiling; }
        set { _isCeiling = value; }
    }

    [SerializeField]
    private bool _isWallFront;
    public bool IsWallFront
    {
        get { return _isWallFront; }
        set { _isWallFront = value; }
    }

    [SerializeField]
    private bool _isWallBack;
    public bool IsWallBack
    {
        get { return _isWallBack; }
        set { _isWallBack = value; }
    }

    private void Awake()
    {
        rb = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IsGrounded = rb.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        IsCeiling = rb.Cast(Vector2.up, castFilter, ceilingHits, groundDistance) > 0;
        IsWallFront = rb.Cast(frontDirection, castFilter, frontHits, groundDistance) > 0;
        IsWallBack = rb.Cast(backDirection, castFilter, backHits, groundDistance) > 0;

        float frontDist = distanceTest(frontDirection, castFilter);
        float backDist = distanceTest(frontDirection, castFilter);

        IsWallBack = frontDist < (rb.size.x / 2.0f * transform.localScale.y) +groundDistance;
        IsWallFront = backDist < (rb.size.x / 2.0f * transform.localScale.y) + groundDistance;

        float hspace = frontDist + backDist;
        float vspace = distanceTest(Vector2.up, castFilter) + distanceTest(Vector2.down, castFilter);
        float vsquish = (rb.size.y * transform.localScale.y) / vspace;
        float hsquish = (rb.size.x * transform.localScale.y) / hspace;
        squishyness = Mathf.Max(vsquish, hsquish);
    }

    float distanceTest(Vector2 direction, ContactFilter2D contactFilter)
    {
        float maxdist = Mathf.Max(rb.size.x, rb.size.y) * transform.localScale.y * 2;
        float result = float.PositiveInfinity;
        if(Physics2D.Raycast(transform.position, direction, contactFilter, distanceHits, maxdist) > 0)
        {
            result = Vector2.Distance(distanceHits[0].point, transform.position);
        }
        return result;
    }
}
