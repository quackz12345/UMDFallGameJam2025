using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1.2f); // bigger size
    private Vector3 originalScale;

    public float speed = 10f; // smoothness

    private bool isHovering = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isHovering)
            transform.localScale = Vector3.Lerp(transform.localScale, hoverScale, Time.deltaTime * speed);
        else
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * speed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }
}
