using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeUnit
{
    public bool isBuying; //true -> buying || false -> selling
    public TradeGood good;
    public GameObject button;
}
public class TradeUI : MonoBehaviour
{
    public static TradeUI instance;

    private void Awake() => instance = this;

    public Canvas worldCanvas;
    public GameObject buttonPrefab;
    public GameObject panelPrefab;

    [Header("Icons")]
    public Sprite buyIcon;
    public Sprite sellIcon;

    public TradeUnit CreateButton(TradeUnit unit, TradeNode node)
    {
        if(node.tradePanel == null)
        {
            GameObject panel = Instantiate(panelPrefab, node.transform.position + Vector3.up * 2f, Quaternion.identity);
            panel.transform.SetParent(worldCanvas.transform);
            node.tradePanel = panel;
        }

        GameObject button = Instantiate(buttonPrefab, node.transform.position, Quaternion.identity);
        button.transform.SetParent(node.tradePanel.transform);
        button.GetComponent<Button>().onClick.AddListener(() => TradeButtonClick(unit, node));
        button.GetComponent<Image>().sprite = unit.good.goodIcon;
        button.transform.GetChild(0).GetComponent<Image>().sprite = unit.isBuying ? buyIcon : sellIcon;

        unit.button = button;
        node.tradeList.Add(unit);

        return unit;
    }

    public void DestroyButton(TradeUnit unit, TradeNode node)
    {
        Destroy(unit.button);
        node.tradeList.Remove(unit);
    }

    public void TradeButtonClick(TradeUnit unit, TradeNode node)
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(node.transform.position, Vector2.one * 3f, 0, Vector3.forward);

        foreach (RaycastHit2D hit in hits)
        {
            Merchant merchant;
            if (hit.collider.TryGetComponent(out merchant))
            {
                if (unit.isBuying && merchant.currentlyHolding == unit.good) //buy from merchant
                {
                    TradeManager.instance.AddMoney(Mathf.RoundToInt(unit.good.goodPrice * 1.3f));
                    DestroyButton(unit, node);
                    merchant.UpdateHoldingGood(null);
                    SoundManager.instance.sellFX.Play();
                    node.ResetDispleasure();
                    break;
                }

                if (!unit.isBuying && merchant.currentlyHolding == null && TradeManager.instance.TryUseMoney(unit.good.goodPrice)) //sell to merchant
                {
                    DestroyButton(unit, node);
                    merchant.UpdateHoldingGood(unit.good);
                    SoundManager.instance.buyFX.Play();
                    node.ResetDispleasure();
                    break;
                }
            }
        }
    }

    bool allUIIsEnabled = false;

    public void ToggleAllUI()
    {
        if (allUIIsEnabled)
        {
            foreach (Merchant merchant in TradeManager.instance.allMerchants) merchant.HideTradeIcon();
            foreach (TradeNode tradeNode in TradeManager.instance.allTradeNodes) tradeNode.HideTradePanel();
        }
        else
        {
            foreach (Merchant merchant in TradeManager.instance.allMerchants) merchant.ShowTradeIcon();
            foreach (TradeNode tradeNode in TradeManager.instance.allTradeNodes) tradeNode.ShowTradePanel();
        }
        allUIIsEnabled = !allUIIsEnabled;
    }

}
