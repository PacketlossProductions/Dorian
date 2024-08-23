using UnityEngine;

public class GrabbingMechanic : MonoBehaviour
{
    Animator animator;

    // Property for grabbing
    private GameObject _heldObject;
    public GameObject HeldItem

    {
        get => _heldObject;
        private set { _heldObject = value; animator.SetBool("IsHolding", value != null); }
    }

    [SerializeField] private LayerMask heldItemsLayer;


    private GameObject handPosition;
    private float previousScale = 1.0f;
    private float previousGravity = 1.0f;
    private float previousAngularDrag = 0.05f;
    private float pickupSelfScale = 1.0f;
    private Rigidbody2D hrb;

    private int previousLayer = 0;

    void Awake()
    {
        animator = GetComponent<Animator>();
        handPosition = transform.Find("GrabPos").gameObject;
    }

    void Update()
    {
        if (HeldItem != null)
        {
            Vector3 grabPos = (handPosition.transform.position - transform.position);
            float itemSize = HeldItem.transform.localScale.y / 4;
            Vector3 offset = grabPos + new Vector3(transform.localScale.x > 0 ? itemSize : -itemSize, itemSize, 1.0f);

            hrb.MovePosition(new Vector2(offset.x + transform.position.x, offset.y + transform.position.y));

            float newScale = transform.localScale.y / pickupSelfScale * previousScale;
            HeldItem.transform.localScale = new Vector3(newScale, newScale, 1.0f);
        }
    }

    public void GrabItem(GameObject item)
    {
        Debug.Log("Putting " + item.name + " in hands");
        previousLayer = item.layer;
        previousScale = item.transform.localScale.y;
        item.layer = (int)Mathf.Log(heldItemsLayer.value, 2);
        pickupSelfScale = transform.localScale.y;
        hrb = item.GetComponent<Rigidbody2D>();
        previousGravity = hrb.gravityScale;
        previousAngularDrag = hrb.angularDrag;
        hrb.gravityScale = 0.0f;
        hrb.angularDrag = 5.0f;
        HeldItem = item;
    }

    public void ReleaseItem()
    {
        if (HeldItem == null)
            return;

        Debug.Log("Dropping " + HeldItem.name);

        HeldItem.layer = previousLayer;
        hrb.gravityScale = previousGravity;
        hrb.angularDrag = previousAngularDrag;
        HeldItem = null;
        hrb = null;
    }
}
