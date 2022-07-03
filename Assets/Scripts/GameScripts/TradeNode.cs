using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TradeNode : MonoBehaviour
{
    public GameObject tradePanel;

    public List<TradeUnit> tradeList = new List<TradeUnit>(); //not used as of now
    public TradeNodeType tradeNodeType;

    public GameObject displeasureIndicator;
    private bool displeased = false;
    private float timeSinceLastTrade;

    [HideInInspector]
    public float nextTradeTimer;
    private void Start()
    {
        nextTradeTimer = Random.Range(2f, 15f);
        nextTradeTimer = Random.Range(-120f, -60f);
    }

    private void Update()
    {
        CheckPanelToggle();
        CheckDispleasure();

        if (tradeList.Count >= tradeNodeType.slots) return;

        nextTradeTimer -= Time.deltaTime;

        if (nextTradeTimer <= 0)
        {
            if (tradeList.Count < tradeNodeType.slots)
            {
                int index = Random.Range(0, tradeNodeType.inventoryItems.Count);
                TradeUnit unit = new TradeUnit
                {
                    good = tradeNodeType.inventoryItems[index].tradeGood,
                    isBuying = !tradeNodeType.inventoryItems[index].isSell
                };

                TradeUI.instance.CreateButton(unit, this);
            }

            nextTradeTimer = Random.Range(tradeNodeType.goodTimer[0], tradeNodeType.goodTimer[1]);
        }
    }

    public void CheckPanelToggle()
    {
        GameObject check = MouseCursor.instance.mouseOverObject;
        if (check == gameObject || check == tradePanel) ShowTradePanel();
        else HideTradePanel();
    }

    public void ShowTradePanel()
    {
        if (tradePanel == null) return;
        if (tradePanel.activeInHierarchy) return;
        tradePanel.SetActive(true);
        SoundManager.instance.Click.Play();
    }

    public void HideTradePanel()
    {
        if (tradePanel == null) return;
        if (!tradePanel.activeInHierarchy) return;
        tradePanel.SetActive(false);
    }

    public void ResetDispleasure()
    {
        displeasureIndicator.SetActive(false);
        displeased = false;
        timeSinceLastTrade = 0;
    }

    private void CheckDispleasure()
    {
        timeSinceLastTrade += Time.deltaTime;

        if (timeSinceLastTrade >= 100f)
        {
            if (!displeased)
            {
                CameraController.instance.ShakeCamera();
                SoundManager.instance.error.Play();
                displeased = true;
                displeasureIndicator.SetActive(true);
            }
            TradeManager.instance.displeasureMeter += Time.deltaTime;
        }
    }
}
