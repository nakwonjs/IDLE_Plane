using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SeamlessVornoi
{
    public static float[,] GenerateSeamlessNoiseMap(int mapWidth, int mapHeight, int seed, int regionAmount, int centroidValue)
    {
		float[,] noiseMap = VornoiNoise.GenerateNoiseMap(mapWidth, mapHeight, seed, regionAmount, centroidValue);
		float[,] seam = VornoiSeam.getSeam();

		for (int y = 0; y < mapWidth; y++)
		{
			for (int x = 0; x < centroidValue; x++)
			{
				float alpha = Mathf.InverseLerp(0, 1, x);
				noiseMap[x - centroidValue + mapWidth, y] = alpha * seam[x, y + centroidValue] + (1f - alpha) * noiseMap[x, y];
			}
		}
		return noiseMap;
	}
        


}
