using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    PlayerCollisions pc;
    Animator animator;
    GameObject gp;

    private float horizontal;
    private float speed = 8.0f;
    private float jumpingPower = 6.0f;
    private bool facingRight = true;

    public float minScale = 2.0f;
    public float maxScale = 20.0f;
    private float playerScale = 1.0f;

    // Scale smoothing
    private float scaleSpeed = 0.1f;
    public float scaleReq = 5.0f;
    public float scaleVal = 5.0f;

    // Properties
    private bool _isMoving;


    // Animation state
    public bool IsMoving
    {
        get => _isMoving;
        private set { _isMoving = value; animator.SetBool("IsMoving", value); }
    }

    // Property for grabbing
    private GameObject _heldObject;
    public GameObject HeldItem

    {
        get => _heldObject;
        private set { _heldObject = value; animator.SetBool("IsHolding", value != null); }
    }

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PlayerCollisions>();
        animator = GetComponent<Animator>();
        gp = transform.Find("GrabPos").gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float direction = pc.IsWallFront ? 0.0f : horizontal;
        rb.velocity = new Vector2(direction * speed * playerScale, rb.velocity.y);
        if (!facingRight && horizontal > 0)
        {
            Flip();
        }
        else if (facingRight && horizontal < 0)
        {
            Flip();
        }
    }

    private void Update()
    {
        animator.SetFloat("VerticalSpeed", rb.velocity.y);

        float baseScale = 5.0f;
        float scale = Camera.main.orthographicSize;
        if (pc.squishyness < 1.0f || scaleReq < scaleVal)
        {
            // Exponential smoothing for the scale
            float newScale = scale + ((scaleReq - scale) * (1.0f - Mathf.Exp(-scaleSpeed)));
            scaleVal = newScale;
            playerScale = newScale / baseScale;

        }

        transform.localScale = new Vector3(facingRight ? playerScale : -playerScale, playerScale, 1.0f);

        Camera.main.orthographicSize = scaleVal;
        Camera.main.transform.position = transform.position + new Vector3(0.0f, 0.0f, -10.0f);
    }


    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        horizontal = input.x;
        IsMoving = input != Vector2.zero;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (pc.IsGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower * scaled(0.5f));
            }
        }
    }

    private float scaled(float factor)
    {
        if(playerScale > 1.0f)
        {
            return Mathf.Log(playerScale,1/factor) + 1.0f;
        }
        if(playerScale < 1.0f)
        {
            return playerScale;
        }
        return playerScale;
    }

    public void Scale(InputAction.CallbackContext context)
    {
        float newScale = scaleReq + (context.ReadValue<float>() * -0.005f);
        scaleReq = Mathf.Clamp(newScale, minScale, maxScale);
    }

    public void ItemPickUp(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (HeldItem == null)
            {
                PickUp();
            }
            else
            {
                Drop();
                HeldItem = null;
            }
        }
    }

    public void PickUp()
    {
        RaycastHit2D[] frontHits = new RaycastHit2D[5];
        Vector2 frontDirection = gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        int count = rb.Cast(frontDirection, frontHits, 1.0f);
        for (int i = 0; i < count; i++)
        {
            if (frontHits[i].transform.gameObject.tag == "PickUpable")
            {
                HeldItem = frontHits[i].transform.gameObject;
                Vector3 offset = gp.transform.localPosition * (HeldItem.transform.localScale.y / 2);

                if (gameObject.transform.localScale.x < 0)
                {
                    offset = new Vector3(offset.x * -1, offset.y, offset.z);
                }

                HeldItem.transform.position = gp.transform.position + offset;
                HeldItem.transform.SetParent(transform, true);
                HeldItem.GetComponent<Rigidbody2D>().isKinematic = true;
                break;
            }
        }
    }

    public void Drop()
    {
        HeldItem.transform.SetParent(null, true);
        HeldItem.GetComponent<Rigidbody2D>().isKinematic = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (HeldItem == null)
            return;

        if (collision.gameObject != HeldItem)
            return;

        collision.gameObject.transform.localPosition = collision.gameObject.transform.localPosition * 1.1f;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (HeldItem == null)
            return;

        if (collision.gameObject != HeldItem)
            return;

        collision.gameObject.transform.localPosition = collision.gameObject.transform.localPosition * 1.1f;
    }
}
