using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PixelPath : MonoBehaviour
{
    public static PixelPath instance;

    private void Awake() => instance = this;

    public GameObject dot;

    private List<GameObject> availableDotPool = new List<GameObject>();
    private List<GameObject> inUseDotPool = new List<GameObject>();

    //Call DrawPath() each update
    public void DrawPath(List<Vector2> nodePositions)
    {
        for(int i = 0; i < nodePositions.Count; i++)
        {
            if (availableDotPool.Count <= 0) availableDotPool.Add(Instantiate(dot));

            GameObject cog = availableDotPool[0];
            cog.transform.position = nodePositions[i];
            availableDotPool.Remove(cog);
            inUseDotPool.Add(cog);

            if (!(i == 0))
            {
                if (availableDotPool.Count <= 0) availableDotPool.Add(Instantiate(dot));

                GameObject bolt = availableDotPool[0];
                bolt.transform.position = (nodePositions[i] + nodePositions[i - 1]) / 2f;
                availableDotPool.Remove(bolt);
                inUseDotPool.Add(bolt);
            }
        }
    }

    private void Update()
    {
        if (availableDotPool.Count > 0)
        {
            foreach (GameObject cog in availableDotPool.Except(inUseDotPool).ToArray()) Destroy(cog);
        }
        availableDotPool = new List<GameObject>(inUseDotPool);
        inUseDotPool = new List<GameObject>();
    }
}
