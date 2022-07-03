using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : TradeNode
{
    public List<TradeUnit> units = new List<TradeUnit>();

    private void Update()
    {
        CheckPanelToggle();
    }

    public void ResetTrades()
    {
        foreach(TradeUnit unit in units)
        {
            TradeUI.instance.DestroyButton(unit, this);
        }

        for(int i = 0; i < 3; i++)
        {
            int index = Random.Range(0, tradeNodeType.inventoryItems.Count);
            TradeUnit unit = new TradeUnit
            {
                good = tradeNodeType.inventoryItems[index].tradeGood,
                isBuying = !tradeNodeType.inventoryItems[index].isSell
            };

            units.Add(TradeUI.instance.CreateButton(unit, this));
        }
    }
}
