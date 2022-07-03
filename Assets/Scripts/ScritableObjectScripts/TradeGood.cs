using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TradeGood", order = 1)]
public class TradeGood : ScriptableObject
{
    public string goodName;
    public Sprite goodIcon;
    public int goodPrice;
}
