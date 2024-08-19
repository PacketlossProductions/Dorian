using UnityEngine;
using UnityEngine.UI;

public class PowerBarController : MonoBehaviour
{
    GameObject player;
    RectTransform rectTransform;
    PlayerMovement pm;
    PlayerCollisions pc;
    RectTransform oneMarkTransform;
    RectTransform powerTransform;
    RectTransform requestTransform;
    Image powerImage;
    public Color powerBarColor;
    public Color powerBarBlockedColor;

    public float width = 0.0f;
    void Awake()
    {
        player = GameObject.Find("Player");
        rectTransform = transform.GetComponent<RectTransform>();
        pm = player.GetComponent<PlayerMovement>();
        pc = player.GetComponent<PlayerCollisions>();


        oneMarkTransform = transform.Find("One").GetComponent<RectTransform>();
        powerTransform = transform.Find("Power").GetComponent<RectTransform>();
        requestTransform = transform.Find("Request").GetComponent<RectTransform>();

        powerImage = powerTransform.gameObject.GetComponent<Image>();
    }

    void Update()
    {
        width = rectTransform.sizeDelta.x;
        float minScale = pm.minScale / 5.0f;
        float maxScale = pm.maxScale / 5.0f;
        float range = maxScale - minScale;
        float pixelsPerScale = width / range;

        float onePosition = (1.0f - minScale) * pixelsPerScale;
        oneMarkTransform.anchoredPosition = new Vector3((width * -0.5f) + onePosition, 0.0f, 0.0f);

        float currentReq = pm.scaleReq / 5.0f;
        float currentScale = pm.playerScale;

        float powerPosition = (currentScale - minScale) * pixelsPerScale;
        powerTransform.anchoredPosition = new Vector3((width * -0.5f) + ((onePosition+powerPosition)/2.0f), 0.0f, 0.0f);
        powerTransform.sizeDelta = new Vector2(Mathf.Abs(powerPosition - onePosition), 24.0f);
        if(pc.squishyness > 1.0f)
        {
            powerImage.color = powerBarBlockedColor;
        } else
        {
            powerImage.color = powerBarColor;
        }

        float requestPosition = (currentReq - minScale) * pixelsPerScale;
        requestTransform.anchoredPosition = new Vector3((width * -0.5f) + requestPosition, -24.0f, 0.0f);
    }
}
