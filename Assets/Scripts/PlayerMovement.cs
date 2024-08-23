using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    PlayerCollisions pc;
    GrabbingMechanic hands;
    Animator animator;
    GameObject gp;

    AudioSource audioSource;
    public AudioClip audioClipWalk;

    private float horizontal;
    private float speed = 8.0f;
    private float jumpingPower = 9.0f;
    public float scrollSpeed = 50.0f;
    private bool facingRight = true;

    public float minScale = 2.0f;
    public float maxScale = 20.0f;
    public float playerScale = 1.0f;

    // Scale smoothing
    private float scaleSpeed = 0.1f;
    public float scaleReq = 5.0f;
    public float scaleVal = 5.0f;

    // Properties
    private bool _isMoving;

    public bool isVeryHungry = false;

    public float acceleration = 0.0f;

    // Animation state
    public bool IsMoving
    {
        get => _isMoving;
        private set { _isMoving = value; animator.SetBool("IsMoving", value); }
    }

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PlayerCollisions>();
        animator = GetComponent<Animator>();
        gp = transform.Find("GrabPos").gameObject;
        audioSource = GetComponent<AudioSource>();
        hands = GetComponent<GrabbingMechanic>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(horizontal != 0 && acceleration < 1.0f)
        {
            acceleration += 0.1f;
        }
        float direction = pc.IsWallFront ? 0.0f : horizontal;
        rb.velocity = new Vector2(direction * speed * playerScale * acceleration, rb.velocity.y);
        if (!facingRight && horizontal > 0)
        {
            Flip();
        }
        else if (facingRight && horizontal < 0)
        {
            Flip();
        }

        if(horizontal != 0 && pc.IsGrounded)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = audioClipWalk;
                audioSource.Play();
            }
        } else if (audioSource.clip == audioClipWalk)
        {
            audioSource.Stop();
            audioSource.clip = null;
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
        if(!IsMoving)
        {
            acceleration = 0.0f;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (pc.IsGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower * scaled(0.6f));
            }
        }
    }

    private float scaled(float factor)
    {
        if(isVeryHungry && playerScale > 1)
        {
            return playerScale * 0.57f;
        }
        if (playerScale > 1 / factor)
        {
            return Mathf.Log(playerScale, 1 / factor);
        }
        if (playerScale < 1 / factor)
        {
            return playerScale;
        }
        return playerScale;
    }

    public void Scale(InputAction.CallbackContext context)
    {
        float newScale = scaleReq + (context.ReadValue<float>() * -0.005f * scrollSpeed);
        scaleReq = Mathf.Clamp(newScale, minScale, maxScale);
    }

    public void ItemPickUp(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (hands.HeldItem == null)
            {
                PickUp();
            }
            else
            {
                hands.ReleaseItem();
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
                hands.GrabItem(frontHits[i].transform.gameObject);
                break;
            }
        }
    }

    /// <summary>
    /// Find a GO. It might be inactive.
    /// </summary>
    /// <param name="parentName">Name of parent GO. Must be active.</param>
    /// <param name="gameObjectName">Name of GO to find.</param>
    /// <returns>GO, or null.</returns>
    public static GameObject FindMaybeDisabledGameObjectByName(string parentName, string gameObjectName)
    {
        GameObject go = GameObject.Find(parentName);
        if (go == null)
        {
            return null;
        }
        Transform childTransform
            = go.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(
                t => t.name == gameObjectName
            );
        if (childTransform == null)
        {
            return null;
        }
        return childTransform.gameObject;
    }

}
