using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{
    private static float[,] SquareBump(int size)
    {
        int centerX = size / 2;
        int centerY = size / 2;

        float[,] value = new float[size, size];

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                float xScaled = (x-centerX) / (float)size;
                float yScaled = (y-centerY) / (float)size;
                float m = (1f - xScaled * xScaled) * (1f - yScaled * yScaled);

                value[x, y] = 1 - m;
            }
        }

        return value;
    }

    public static float[,] IslandMap(int size)
    {
        float[,] value = new float[size, size];

        //tweakable variables
        float[,] dropoffMap = SquareBump(size);
        float seed = Random.Range(0, 999999f);
        float scale = 0.1f;
        float dropoffWeight = 2.5f;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                value[x, y] = Mathf.PerlinNoise(x * scale + seed, y * scale + seed) - dropoffMap[x,y] * dropoffWeight;
            }
        }

        return value;
    }

    public static float[,] ForestMap(int size)
    {
        float[,] value = new float[size, size];

        //tweakable variables
        float seed = Random.Range(0, 999999f);
        float scale = 0.3f;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                value[x, y] = Mathf.PerlinNoise(x * scale + seed, y * scale + seed);
            }
        }

        return value;
    }
}
