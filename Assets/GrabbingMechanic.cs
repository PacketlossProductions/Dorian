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
    private Rigidbody2D hrb;

    private int previousLayer = 0;

    void Awake()
    {
        animator = GetComponent<Animator>();
        handPosition = transform.Find("GrabPos").gameObject;
    }

    void Update()
    {
        if(HeldItem != null)
        {
            Vector3 grabPos = (handPosition.transform.position - transform.position) * 3;
            Vector3 offset = grabPos * (HeldItem.transform.localScale.y / 2);
            hrb.MovePosition(new Vector2(offset.x + transform.position.x, offset.y + transform.position.y));
        }
    }

    public void GrabItem(GameObject item)
    {
        Debug.Log("Putting " + item.name + " in hands");
        previousLayer = item.layer;
        previousScale = item.transform.localScale.y;
        item.layer = (int)Mathf.Log(heldItemsLayer.value, 2);
        hrb = item.GetComponent<Rigidbody2D>();
        hrb.gravityScale = 0.0f;
        HeldItem = item;
    }

    public void ReleaseItem()
    {
        if (HeldItem == null)
            return;

        Debug.Log("Dropping " + HeldItem.name);

        HeldItem.layer = previousLayer;
        hrb.gravityScale = 1.0f;
        HeldItem = null;
        hrb = null;
    }
}
