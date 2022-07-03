using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class IslandGeneration : MonoBehaviour
{
    public static IslandGeneration instance;

    private void Awake()
    {
        instance = this;
    }

    private List<Vector2Int> availableGrassTiles = new List<Vector2Int>();
    private List<Vector2> structureTiles = new List<Vector2>();
    private List<Vector2> vegetationTiles = new List<Vector2>();
    private List<Vector2> crossableTiles = new List<Vector2>();

    [Header("Grid and Tile map")]
    public Grid grid;
    public Tilemap grassTileMap;
    public Tilemap sandTileMap;
    public Tilemap foamTileMap;
    public Tilemap vegetationTileMap;
    public Tilemap structuresTileMap;

    [Header("Terrain")]
    public Color[] grassColors = new Color[3];
    public Tile grassTile;

    public Color[] sandColors = new Color[2];
    public TileBase sandTile;

    public TileBase foamTile;

    [Header("Vegetation")]
    public Tile treeTile;

    private void Start()
    {
        GenerateIsland(32);
    }

    /*
    private void DisplayMap(float[,] map)
    {
        Texture2D texture = new Texture2D(map.GetLength(0), map.GetLength(0));

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(0); y++)
            {
                Color color = new Color(map[x, y], map[x, y], map[x, y]);
                texture.SetPixel(x, y, color);
            }
        }
        
        mapRenderer.material.mainTexture = texture;
        texture.Apply();
    }
    */
    private void GenerateIsland(int size)
    {
        float[,] map = Noise.IslandMap(size);

        //1st pass ground
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);

                if (map[x, y] >= 0.2f)
                {
                    grassTileMap.SetTile(position, grassTile);
                    grassTileMap.SetColor(position, grassColors[Random.Range(0, grassColors.Length)]);

                    availableGrassTiles.Add(new Vector2Int(x, y));

                    crossableTiles.Add((Vector2Int)position);
                }
                if(map[x, y] >= 0.1f)
                {
                    sandTileMap.SetTile(position, sandTile);
                    sandTileMap.SetColor(position, sandColors[Random.Range(0, sandColors.Length)]);
                    crossableTiles.Add((Vector2Int)position);

                    for (int j = -1; j <= 1; j++)
                    {
                        for (int k = -1; k <= 1; k++)
                        {
                            foamTileMap.SetTile(position + new Vector3Int(j, k, 0), foamTile);
                        }
                    }
                }
            }
        }

        float[,] forestMap = Noise.ForestMap(size);

        //2nd pass vegetation
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);

                if (forestMap[x, y] >= 0.6f && grassTileMap.GetTile(position) != null)
                {
                    vegetationTileMap.SetTile(position, treeTile);
                    
                    availableGrassTiles.Remove(new Vector2Int(x, y));

                    vegetationTiles.Add((Vector2Int)position);
                }
            }
        }

        GenerateStructuresDefaultIsland();
        TradeManager.instance.FindAllTradeNodes();
        TradeManager.instance.FindAllMerchants();

        crossableTiles = new List<Vector2>(crossableTiles.Except(structureTiles));
        crossableTiles = new List<Vector2>(crossableTiles.Except(vegetationTiles));

        CreateDock();

        MerchantManager.instance.islandCrossableTiles = new List<Vector2>(crossableTiles);
    }

    [Header("Structures")]
    public GameObject tradeStructure;
    public GameObject dock;

    [Header("Default Island Structures")]
    public TradeNodeType defaultWheatFarm;
    public TradeNodeType defaultHouseSmall;
    public TradeNodeType defaultHouseMedium;
    public TradeNodeType defaultButcher;
    public TradeNodeType defaultMines;

    private void CreateDock()
    {
        int lastMaxX = 0;
        int lastMaxY = 0;
        for(int x = 0; x < 48; x++)
        {
            for (int y = 16; y < 48; y++)
            {
                if (sandTileMap.GetTile(new Vector3Int(x, y, 0)) == null && sandTileMap.GetTile(new Vector3Int(x-1, y, 0)) != null)
                {
                    lastMaxX = x;
                    lastMaxY = y;
                }
            }
        }

        dock.transform.position = new Vector3(lastMaxX, lastMaxY, 1);
        MerchantManager.instance.dockPosition = new Vector2(lastMaxX, lastMaxY);
        crossableTiles.Add(new Vector3(lastMaxX, lastMaxY));
    }

    private Vector3Int RecursivelyCreateVillages(TradeNodeType hub, TradeNodeType branches, int branchesCount)
    {
        if(branchesCount > 0)
        {
            Vector3Int position = RecursivelyCreateVillages(hub, branches, branchesCount-1);

            Vector3Int offset = new Vector3Int(Random.Range(-2, 2), Random.Range(-2, 2), 0);

            for(int tries = 0; tries < 10; tries++)
            {
                if (availableGrassTiles.Contains((Vector2Int)(position + offset)))
                {
                    availableGrassTiles.Remove((Vector2Int)(position + offset));

                    GameObject structure = Instantiate(tradeStructure, position + offset, Quaternion.identity);
                    structure.GetComponent<SpriteRenderer>().sprite = branches.sprites[Random.Range(0, branches.sprites.Count)];
                    structure.GetComponent<TradeNode>().tradeNodeType = branches;

                    structureTiles.Add((Vector2Int)(position + offset));

                    return position + offset;
                }
            }
            return position;
        }
        else
        {
            int index = Random.Range(0, availableGrassTiles.Count);
            Vector3Int position = (Vector3Int)availableGrassTiles[index];
            availableGrassTiles.RemoveAt(index);

            GameObject structure = Instantiate(tradeStructure, position, Quaternion.identity);
            structure.GetComponent<SpriteRenderer>().sprite = hub.sprites[Random.Range(0, hub.sprites.Count)];
            structure.GetComponent<TradeNode>().tradeNodeType = hub;

            structureTiles.Add((Vector3)position);

            return position;
        }
    }

    private void GenerateStructuresDefaultIsland()
    {
        //1st pass farms
        for(int i = 0; i < 2; i++)
        {
            RecursivelyCreateVillages(defaultButcher, defaultWheatFarm, 3);
        }

        for (int i = 0; i < 4; i++)
        {
            RecursivelyCreateVillages(defaultHouseMedium, defaultHouseSmall, 3);
        }

        for (int i = 0; i < 2; i++)
        {
            RecursivelyCreateVillages(defaultMines, defaultMines, 0);
        }

        /*
         * //1st pass villages
        for (int i = 0; i < 3; i++)// i < numOfVillages
        {
            int index = Random.Range(0, availableGrassTiles.Count);
            Vector3Int position = (Vector3Int)availableGrassTiles[index];
            availableGrassTiles.RemoveAt(index);

            Instantiate(houseMedium, position, Quaternion.identity);
            MerchantManager.instance.structureNodes.Add((Vector3)(position));

            for (int m = 0; m < 3; m++)// m < maxNumofHousesInVillage
            {
                Vector3Int offset = new Vector3Int(Random.Range(-2, 2), Random.Range(-2, 2), 0);

                if (availableGrassTiles.Contains((Vector2Int)(position + offset)))
                {
                    availableGrassTiles.Remove((Vector2Int)(position + offset));
                    SpriteRenderer rend = Instantiate(houseSmall, position, Quaternion.identity).GetComponent<SpriteRenderer>();
                    MerchantManager.instance.structureNodes.Add((Vector3)(position + offset));
                }
            }
        }
         */

    }
}
