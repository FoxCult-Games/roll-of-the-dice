using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    private Rigidbody2D rb;
    private float baseGravityScale;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        baseGravityScale = rb.gravityScale;
        canvas = GameObject.FindGameObjectWithTag("OriginCanvas").GetComponent<Canvas>();
    }

    public void DragHandler(BaseEventData data){
        PointerEventData pointerData = (PointerEventData)data;
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            pointerData.position,
            canvas.worldCamera,
            out position
        );

        transform.position = canvas.transform.TransformPoint(position);
    }

    public void DragStart(BaseEventData data){
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
    }

    public void DragEnd(BaseEventData data){
        rb.gravityScale = baseGravityScale;
    }
}
