using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TradeNodeInventoryItem
{
    public TradeGood tradeGood;
    public bool isSell;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TradeNodeType", order = 2)]
public class TradeNodeType : ScriptableObject
{
    public List<TradeNodeInventoryItem> inventoryItems;
    public List<Sprite> sprites;
    public int slots = 3;
    public float[] goodTimer = new float[2];
}
