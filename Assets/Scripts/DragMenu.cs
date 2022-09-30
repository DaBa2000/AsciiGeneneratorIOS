using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragMenu : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;

    private float yPosMax;
    private float yPosPref;

    private float velocity;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        yPosMax = rectTransform.anchoredPosition.y + 100; 
        yPosPref = -3100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        velocity = 0;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (velocity > 0)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, yPosMax);
        }

        if (velocity < 0)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, yPosPref);
        }

        /*
        if (Mathf.Abs(rectTransform.anchoredPosition.y - yPosMax) > Mathf.Abs(rectTransform.anchoredPosition.y - yPosPref))
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, yPosPref);
        }
        else
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, yPosMax);
        }
        */
    }

    public void OnDrag(PointerEventData eventData)
    {
        velocity += eventData.delta.y;

        rectTransform.anchoredPosition += new Vector2(0, eventData.delta.y);

        if (rectTransform.anchoredPosition.y > yPosMax)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, yPosMax);
        }
        
    }
}
