using UnityEngine;

public class WorldEaterMechanics : MonoBehaviour
{
    public GameObject eatable;
    public GameObject gravityCenter;

    public Vector2 gravity;
    public float angle;

    Animator animator;
    public float consumptionDistance = 10.0f;

    public Vector2 earth;
    public Vector2 me;

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
            earth = new Vector2(gravityCenter.transform.position.x, gravityCenter.transform.position.y);
            me = new Vector2(transform.position.x, transform.position.y);
            gravity = (me - earth).normalized * -9f;
            Physics2D.gravity = gravity;

            angle = GetAngle(earth, me);
            this.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle - 90);
        }
    }

    public float GetAngle(Vector2 A, Vector2 B)
    {
        //difference
        var Delta = B - A;
        //use atan2 to get the angle; Atan2 returns radians
        var angleRadians = Mathf.Atan2(Delta.y, Delta.x);

        //convert to degrees
        var angleDegrees = angleRadians * Mathf.Rad2Deg;

        //angleDegrees will be in the range (-180,180].
        //I like normalizing to [0,360) myself, but this is optional..
        if (angleDegrees < 0)
            angleDegrees += 360;

        return angleDegrees;
    }
}
