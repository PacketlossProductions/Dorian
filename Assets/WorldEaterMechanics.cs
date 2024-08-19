using UnityEngine;

public class WorldEaterMechanics : MonoBehaviour
{
    public GameObject eatable;
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
    }
}
