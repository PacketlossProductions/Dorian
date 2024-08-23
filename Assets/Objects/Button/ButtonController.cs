using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ButtonController : MonoBehaviour
{
    public ContactFilter2D castFilter;

    public float currentForce = 0.0f;
    public float triggerWeight = 1.0f;
    public bool state = false;
    Collider2D col;
    Animator animator;
    Light2D indicator;
    public Color enabledColor;
    public Color disabledColor;


    void Awake()
    {
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        indicator = transform.Find("indicator").GetComponent<Light2D>();
    }

    void Update()
    {
        if(currentForce > 0.0f)
        {
            updateForce();
        }
        state = currentForce >= triggerWeight;
        indicator.color = state ? enabledColor : disabledColor;
        animator.SetBool("IsTriggered", state);
    }

    private void updateForce()
    {
        RaycastHit2D[] hits = new RaycastHit2D[5];
        int count = col.Cast(Vector2.up, castFilter, hits, 0.1f);
        if (count == 0)
        {
            currentForce = 0.0f;
            return;
        }
        float result = 0.0f;
        for(int i=0;i<count;i++)
        {
            if(hits[i].rigidbody.name == "World")
            {
                continue;
            }
            float mass = hits[i].rigidbody.mass;
            float size = hits[i].transform.localScale.y;
            result += mass * size;
        }
        currentForce = result;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        updateForce();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        updateForce();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        updateForce();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        updateForce();
    }
}
