using UnityEngine;

public class SizeClamper : MonoBehaviour
{
    public float minSize = 0.75f;
    public float maxSize = 4.00f;

    public float currentSize = 1.0f;
    public float factor = 1.0f;

    // Update is called once per frame
    void Update()
    {
        factor = 1.0f;
        if(transform.parent != null)
        {
            factor = transform.parent.localScale.y;
        }
        currentSize = transform.localScale.y * factor;
        if(transform.localScale.y * factor < minSize)
        {
            transform.localScale = new Vector3(minSize/factor, minSize/factor, 1.0f);
        }

        if (transform.localScale.y * factor > maxSize)
        {
            transform.localScale = new Vector3(maxSize/factor, maxSize/factor, 1.0f);
        }
    }
}
