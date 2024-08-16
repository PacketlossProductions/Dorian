using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    PlayerCollisions pc;

    private float horizontal;
    private float speed = 8.0f;
    private float jumpingPower = 4.0f;
    private bool facingRight = true;

    private float minScale = 2.0f;
    private float maxScale = 20.0f;
    private float playerScale = 1.0f;

    // Scale smoothing
    private float scaleSpeed = 0.1f;
    public float scaleReq = 5.0f;
    public float scaleVal = 5.0f;

    // Animation state
    public bool IsMoving { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PlayerCollisions>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed * playerScale, rb.velocity.y);
        if (!facingRight && horizontal > 0)
        {
            Flip();
        }
        else if (facingRight && horizontal < 0)
        {
            Flip();
        }

        // Exponential smoothing for the scale
        float baseScale = 5.0f;
        float scale = Camera.main.orthographicSize;
        float newScale = scale + ((scaleReq - scale) * (1.0f - Mathf.Exp(-scaleSpeed)));

        scaleVal = newScale;
        playerScale = newScale / baseScale;
        transform.localScale = new Vector3(playerScale, playerScale, playerScale);

        Camera.main.orthographicSize = newScale;
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
        if(context.started)
        {
            if(pc.IsGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            }
        }
    }

    public void Scale(InputAction.CallbackContext context)
    {
        float newScale = scaleReq + (context.ReadValue<float>() * -0.005f);
        scaleReq = Mathf.Clamp(newScale, minScale, maxScale);
    }
}
