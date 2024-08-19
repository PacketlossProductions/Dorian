using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    public float bgscale = 0.5f;
    public float moveScale = 0.001f;
    public Vector2 levelOffset = new Vector2(0.0f, 0.0f);

    void Awake()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 pos = Camera.main.transform.position;
        levelOffset = new Vector2(pos.x * moveScale, pos.y * moveScale);
        transform.position = new Vector3(pos.x + (pos.x*moveScale), pos.y + (pos.y * moveScale), 20f);
        transform.localScale = new Vector3(Camera.main.orthographicSize / 5.0f * bgscale, Camera.main.orthographicSize / 5.0f * bgscale, 1.0f);
    }
}
