using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseCursor : MonoBehaviour
{
    public static MouseCursor instance;

    private void Awake() => instance = this;

    public Camera mainCamera;
    public SpriteRenderer indicator;

    [HideInInspector]
    public GameObject mouseOverObject;

    private void Update()
    {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1,
        };

        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        if (results.Count > 0)
        {
            foreach(RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("Trade Panel"))
                {
                    mouseOverObject = result.gameObject;
                    indicator.enabled = false;
                    break;
                }
            }
        }
        else
        {
            indicator.enabled = true;
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null) mouseOverObject = hit.collider.gameObject;
            else mouseOverObject = null;
        }
    }
}
