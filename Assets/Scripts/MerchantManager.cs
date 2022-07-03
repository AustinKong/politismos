using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantManager : MonoBehaviour
{
    public static MerchantManager instance;

    private void Awake() => instance = this;

    public GameObject merchantPrefab;

    public Camera mainCamera;

    public Transform indicator;

    [HideInInspector]
    public Vector2 dockPosition;

    List<Vector2> mouseNodePositions = new List<Vector2>();
    [HideInInspector]
    public List<Vector2> islandCrossableTiles = new List<Vector2>();

    bool isDraggingPath = false;
    Merchant selectedMerchant;

    public bool BuyNewMerchant()
    {
        if (TradeManager.instance.TryUseMoney(30))
        {
            Instantiate(merchantPrefab, dockPosition, Quaternion.identity);
            return true;
        }
        else return false;
    }

    private void Start()
    {
        BuyNewMerchant();
        TradeManager.instance.FindAllMerchants();
    }

    public GameObject ridTreeIndicator;

    private void Update()
    {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int mousePosSnapped = new Vector2Int(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));

        indicator.position = (Vector2)mousePosSnapped;

        //im lazy to make a new script
        if (IslandGeneration.instance.vegetationTileMap.GetTile((Vector3Int)mousePosSnapped) != null && MouseCursor.instance.mouseOverObject == null)
        {
            ridTreeIndicator.SetActive(true);
            if(Input.GetMouseButtonDown(0) && TradeManager.instance.TryUseMoney(5))
            {
                SoundManager.instance.woodChop.Play();
                IslandGeneration.instance.vegetationTileMap.SetTile((Vector3Int)mousePosSnapped, null);
                islandCrossableTiles.Add(mousePosSnapped);
            }
        }
        else
        {
            ridTreeIndicator.SetActive(false);
        }

        //this must run before next if
        if (isDraggingPath && Input.GetMouseButton(0)) //create path
        {   
            if (mouseNodePositions.Count >= 1 && Vector2.Distance(mousePosSnapped, mouseNodePositions[mouseNodePositions.Count - 1]) > 1)
            {   //break path if dragging too fast
                mouseNodePositions.Clear();
                isDraggingPath = false;
            }
            else
            {
                if (mouseNodePositions.Contains(mousePosSnapped)) //remove nodes up to the intersecting point
                {
                    int index = mouseNodePositions.IndexOf(mousePosSnapped);
                    mouseNodePositions.RemoveRange(index, mouseNodePositions.Count - index);
                }

                if (!islandCrossableTiles.Contains(mousePosSnapped))
                {   //break path if overlapping structure/destination EXCEPT IF YOU ARE ON
                    mouseNodePositions.Clear();
                    isDraggingPath = false;
                }
                else mouseNodePositions.Add(mousePosSnapped);
                PixelPath.instance.DrawPath(mouseNodePositions);
            }
        }

        if (isDraggingPath && Input.GetMouseButtonUp(0))
        {
            if (mouseNodePositions.Count > 1)
            {
                selectedMerchant.status = MerchantStatus.travelling;
                selectedMerchant.travelNodes.AddRange(mouseNodePositions);
                mouseNodePositions.Clear();
            }
            isDraggingPath = false;
            selectedMerchant = null;
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("Merchant"))
            {
                selectedMerchant = hit.collider.GetComponent<Merchant>();

                if (selectedMerchant.status == MerchantStatus.idle) isDraggingPath = true;
            }
        }
    }
}
