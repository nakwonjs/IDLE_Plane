using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GeoData : UpdatableData
{
	[HideInInspector]
	public float uniformScale = 2.5f;
	public float noiseScale;
	
	public int octaves;
	[Range(0, 1)]
	public float persistance;
	public float lacunarity;

	public int regionAmount;

	public int centroidValue = 20;

	public int seed;
	public Vector2 offset;
	[Range(0, 1)]
	public float alpha;


	public float meshHeightMultiplier;
	public AnimationCurve meshHeightCurve;

	public float minHeight
    {
        get
        {
			return meshHeightMultiplier * meshHeightCurve.Evaluate(0) * uniformScale;
        }
    }

	public float maxHeight
    {
        get
        {
			return meshHeightMultiplier * meshHeightCurve.Evaluate(1) * uniformScale;
		}
    }

	//public TerrainType[] regions;
#if UNITY_EDITOR
	protected override void OnValidate()
	{
		if (lacunarity < 1)
		{
			lacunarity = 1;
		}
		if (octaves < 0)
		{
			octaves = 0;
		}

		base.OnValidate();
	}
#endif
}
