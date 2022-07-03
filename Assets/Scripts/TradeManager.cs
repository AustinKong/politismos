using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TradeManager : MonoBehaviour
{
    public static TradeManager instance;
    
    [HideInInspector]
    public float displeasureMeter = 0;

    public Slider displeasureSlider;

    private void Awake() => instance = this;

    public List<TradeNode> allTradeNodes = new List<TradeNode>();
    public List<Merchant> allMerchants = new List<Merchant>();

    public TMP_Text moneyText;

    public void FindAllTradeNodes() => allTradeNodes.AddRange(FindObjectsOfType<TradeNode>());
    public void FindAllMerchants() => allMerchants.AddRange(FindObjectsOfType<Merchant>());

    private int money = 70;

    private void Update()
    {
        displeasureSlider.value = displeasureMeter / 10000f;
        if (displeasureMeter >= 10000f) SceneMnger.LoadGameOverScene();
    }

    public int GetMoney() => money;

    public bool TryUseMoney(int value)
    {
        if (money - value < 0) return false;
        else
        {
            money -= value;
            UpdateMoneyUI();
            return true;
        }
    }

    public void AddMoney(int value)
    {
        money += value;
        UpdateMoneyUI();
    }

    private void UpdateMoneyUI() => moneyText.text = money.ToString();
}
