using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTrade : MonoBehaviour
{
    public TradeNode ship;

    private Vector3 shipTargetPosition;

    private SpriteRenderer rend;

    private void Start()
    {
        rend = ship.GetComponent<SpriteRenderer>();
        Invoke("Init", 0.1f);
    }

    private void Init()
    {
        ship.transform.position = MerchantManager.instance.dockPosition + new Vector2(32, 0);
        ShipLeaveDock();
    }

    private void Update()
    {
        if ((shipTargetPosition - ship.transform.position).magnitude < 0.1f) return;
        ship.transform.Translate((shipTargetPosition - ship.transform.position).normalized * Time.deltaTime * 1f);
        if ((shipTargetPosition - ship.transform.position).normalized.x < 0)
        {
            rend.flipX = false;
        }
        else if((shipTargetPosition - ship.transform.position).normalized.x > 0)
        {
            rend.flipX = true;
        }
    }

    private void ShipMoveToDock()
    {
        ship.GetComponent<Ship>().ResetTrades();
        shipTargetPosition = MerchantManager.instance.dockPosition + new Vector2(1, 0);
        Invoke("ShipWait", 10f);
    }

    private void ShipLeaveDock()
    {
        shipTargetPosition = MerchantManager.instance.dockPosition + new Vector2(32, Random.Range(-1f,1f));
        Invoke("ShipMoveToDock", 10f + Random.Range(20f,30f));
    }

    private void ShipWait()
    {
        Invoke("ShipLeaveDock", 50f);
    }
}
