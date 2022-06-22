using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VornoiNoise
{
	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, int regionAmount, int centroidValue)
	{

		int mapW = mapHeight +  2 * centroidValue;
		int mapH = mapHeight + 2 * centroidValue;
		float[,] noiseMap = new float[mapW, mapH];
		float[,] r_noiseMap = new float[mapWidth, mapHeight];
		float [,] seam = new float[centroidValue, mapW];


		System.Random prng = new System.Random();

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		float halfWidth = mapW / 2f;
		float halfHeight = mapH / 2f;


		Vector2Int[] centroids = new Vector2Int[regionAmount];

		for (int i = 0; i < regionAmount; i++)
		{
			centroids[i] = new Vector2Int(prng.Next(centroidValue, mapWidth + centroidValue), prng.Next(centroidValue, mapHeight + centroidValue));
		}
		float[,] distances = new float[mapW, mapH];

		for (int x = 0; x < mapW; x++)
		{
			for (int y = 0; y < mapH; y++)
			{
				distances[x, y] = Vector2.Distance(new Vector2Int(x, y), centroids[GetClosestCentroidIndex(new Vector2Int(x, y), centroids)]);
			}
		}
		float maxDst = GetMaxDistance(distances, mapW, mapH);


		float noiseHeight;
		for (int y = 0; y < mapW; y++)
		{
			for (int x = 0; x < mapH; x++)
			{
				noiseHeight = distances[x, y] / maxDst;
				if (noiseHeight > maxNoiseHeight)
				{
					maxNoiseHeight = noiseHeight;
				}
				else if (noiseHeight < minNoiseHeight)
				{
					minNoiseHeight = noiseHeight;
				}
				noiseMap[x, y] = noiseHeight;
			}
		}

		for (int y = 0; y < mapWidth; y++)
		{
			for (int x = 0; x < mapHeight; x++)
			{
				r_noiseMap[x, y] = Mathf.InverseLerp(maxNoiseHeight, minNoiseHeight, noiseMap[x + centroidValue, y + centroidValue]);
			}
		}

		seam = VornoiSeam.getSeam();

		for (int y = 0; y < mapWidth; y++)
		{
			for (int x = 0; x < centroidValue; x++)
			{
				float alpha = Mathf.InverseLerp(0, centroidValue, x);
				r_noiseMap[x - centroidValue + mapWidth, y] = alpha * seam[x, y + centroidValue] + (1f - alpha) * r_noiseMap[x - centroidValue + mapWidth, y];
			}
		}

		for (int y = 0; y < mapW; y++)
		{
			for (int x = 0; x < centroidValue; x++)
			{
				seam[x, y] = Mathf.InverseLerp(maxNoiseHeight, minNoiseHeight, noiseMap[x, y]);
			}
		}
		VornoiSeam.setSeam(seam);

		return r_noiseMap;
	}

	public static void setInit(int mapWidth, int mapHeight, int seed, int regionAmount, int centroidValue)
    {
		int mapW = mapHeight + 2 * centroidValue;
		int mapH = mapHeight + 2 * centroidValue;
		float[,] noiseMap = new float[mapW, mapH];
		float[,] r_noiseMap = new float[mapWidth, mapHeight];
		float[,] seam = new float[centroidValue, mapW];


		System.Random prng = new System.Random();

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		float halfWidth = mapW / 2f;
		float halfHeight = mapH / 2f;


		Vector2Int[] centroids = new Vector2Int[regionAmount];

		for (int i = 0; i < regionAmount; i++)
		{
			centroids[i] = new Vector2Int(prng.Next(centroidValue, mapWidth + centroidValue), prng.Next(centroidValue, mapHeight + centroidValue));
		}
		float[,] distances = new float[mapW, mapH];

		for (int x = 0; x < mapW; x++)
		{
			for (int y = 0; y < mapH; y++)
			{
				distances[x, y] = Vector2.Distance(new Vector2Int(x, y), centroids[GetClosestCentroidIndex(new Vector2Int(x, y), centroids)]);
			}
		}
		float maxDst = GetMaxDistance(distances, mapW, mapH);


		float noiseHeight;
		for (int y = 0; y < mapW; y++)
		{
			for (int x = 0; x < mapH; x++)
			{
				noiseHeight = distances[x, y] / maxDst;
				if (noiseHeight > maxNoiseHeight)
				{
					maxNoiseHeight = noiseHeight;
				}
				else if (noiseHeight < minNoiseHeight)
				{
					minNoiseHeight = noiseHeight;
				}
				noiseMap[x, y] = noiseHeight;
			}
		}

		for (int y = 0; y < mapW; y++)
		{
			for (int x = 0; x < centroidValue; x++)
			{
				seam[x, y] = Mathf.InverseLerp(maxNoiseHeight, minNoiseHeight, noiseMap[x, y]);
			}
		}
		VornoiSeam.setSeam(seam);
	}

	static float GetMaxDistance(float[,] distances, int width, int height)
	{
		float maxDst = float.MinValue;
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if (distances[i, j] > maxDst)
				{
					maxDst = distances[i, j];
				}
			}
		}
		return maxDst;
	}

	static int GetClosestCentroidIndex(Vector2Int pixelPos, Vector2Int[] centroids)
	{
		float smallestDst = float.MaxValue;
		int index = 0;
		for (int i = 0; i < centroids.Length; i++)
		{
			if (Vector2.Distance(pixelPos, centroids[i]) < smallestDst)
			{
				smallestDst = Vector2.Distance(pixelPos, centroids[i]);
				index = i;
			}
		}
		return index;
	}

}