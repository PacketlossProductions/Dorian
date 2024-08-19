using UnityEngine;

public class EasterEggDetector : MonoBehaviour
{

    SpriteRenderer sr;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag =="PickUpable")
        {
            sr.enabled = true;
        }
    }
}
