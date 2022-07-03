using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dock : TradeNode
{
    private void Start() { }

    private void Update()
    {
        CheckPanelToggle();
    }

    public void OnDockButtonClick()
    {
        MerchantManager.instance.BuyNewMerchant();
    }
}
