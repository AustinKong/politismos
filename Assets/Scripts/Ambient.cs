using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambient : MonoBehaviour
{
    public GameObject birdPrefab;
    private List<Transform> birdTransforms = new List<Transform>();

    private void Start()
    {

        SpawnBird();
    }

    private void Update()
    {
        try
        {
            foreach (Transform bird in birdTransforms) MoveAndCheckBird(bird);
        }
        catch{ }
    }

    private void SpawnBird()
    {
        birdTransforms.Add(Instantiate(birdPrefab, new Vector3(64f, Random.Range(0,32f), 0), Quaternion.identity).transform);
        Invoke("SpawnBird", Random.Range(12f, 32f));
    }

    private void MoveAndCheckBird(Transform trans)
    {
        if (trans.position.x < -32)
        {
            birdTransforms.Remove(trans);
            Destroy(trans.gameObject);
        }
        else
        {
            trans.Translate(new Vector3(-1.5f, -0.1f, 0) * Time.deltaTime);
        }

    }
}
