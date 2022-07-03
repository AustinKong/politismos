using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MerchantStatus
{
    travelling,
    idle
}
public class Merchant : MonoBehaviour
{
    public Image tradeIcon;

    [HideInInspector]
    public TradeGood currentlyHolding;
    [HideInInspector]
    public MerchantStatus status;
    [HideInInspector]
    public List<Vector2> travelNodes = new List<Vector2>();

    SpriteRenderer spriteRenderer;

    private void Start() => spriteRenderer = GetComponent<SpriteRenderer>();

    private void Update()
    {
        if (travelNodes.Count > 0)
        {
            Vector2 direction = (travelNodes[1] - (Vector2)transform.position).normalized;
            transform.Translate(direction * Time.deltaTime * 0.5f);
            if (direction.x > 0) spriteRenderer.flipX = true;
            else if(direction.x < 0) spriteRenderer.flipX = false;

            if (Vector2.Distance(travelNodes[1], transform.position) <= 0.05f)
            {
                travelNodes.RemoveAt(0);
                if (travelNodes.Count == 1)
                {
                    travelNodes.Clear();
                    status = MerchantStatus.idle;
                }
            }
        }
        CheckUIToggle();
    }

    public void UpdateHoldingGood(TradeGood good)
    {
        if (good == null) tradeIcon.color = new Color(1, 1, 1, 0);
        else
        {
            tradeIcon.color = new Color(1, 1, 1, 1);
            tradeIcon.sprite = good.goodIcon;
        }
            
        currentlyHolding = good;
    }

    public void ShowTradeIcon()
    {
        if (tradeIcon.gameObject.activeInHierarchy) return;
        tradeIcon.gameObject.transform.parent.transform.gameObject.SetActive(true);
    }

    public void HideTradeIcon()
    {
        if (!tradeIcon.gameObject.activeInHierarchy) return;
        tradeIcon.gameObject.transform.parent.transform.gameObject.SetActive(false);
    }

    private void CheckUIToggle()
    {
        GameObject check = MouseCursor.instance.mouseOverObject;
        if (check == gameObject)
        {
            ShowTradeIcon();
            PixelPath.instance.DrawPath(travelNodes);
        }
        else HideTradeIcon();
        
    }
}
