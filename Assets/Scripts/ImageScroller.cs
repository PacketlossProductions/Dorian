using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class ImageScroller : MonoBehaviour
{

    RawImage img;
    public float speed = 0.01f;
    int counter = 0;

    void Awake()
    {
        img = GetComponent<RawImage>();
    }

    void Update()
    {
        counter++;
        if (counter > 6)
        {
            img.uvRect = new Rect(img.uvRect.x + speed, img.uvRect.y, img.uvRect.width, img.uvRect.height);
            counter = 0;
        }
    }
}
