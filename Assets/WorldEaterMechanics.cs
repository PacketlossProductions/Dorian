using UnityEngine;

public class WorldEaterMechanics : MonoBehaviour
{
    public GameObject eatable;
    public GameObject gravityCenter;

    public Vector2 gravity;
    public float angle;

    Animator animator;
    public float consumptionDistance = 10.0f;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(eatable != null)
        {
            float distance = Vector3.Distance(eatable.transform.position, transform.position);
            animator.SetBool("IsConsuming", distance < consumptionDistance);
        }

        if(gravityCenter != null)
        {
            Vector2 earth = new Vector2(gravityCenter.transform.position.x, gravityCenter.transform.position.y);
            Vector2 me = new Vector2(transform.position.x, transform.position.y);
            gravity = (me - earth).normalized * -9.81f;
            Physics2D.gravity = gravity;

            angle = Vector2.Angle(me, earth);
            this.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -angle);
        }
    }
}
