using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

	public enum DrawMode {Perlin_Mesh, Vornoi_Mesh, blend_NoiseMap, blend_Mesh };
	public enum NoiseType { Perlin, Vornoi, Blend };
	public DrawMode drawMode;

	public NoiseType noiseTypeDraw;

	public perlinNoise.NormalizeMode normalizeMode;

	public Geography[] geographies;
	
	public GeoData geoData;
	public TextureData textureData;

	public Material terrainMaterial;


	public GameObject water;
	public const int mapChunkSize = 241;
	[Range(0,6)]
	int editorPreviewLOD = 0;


	public bool autoUpdate;


	
	Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
	Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

	float[,] blendMap(float[,] map1, float[,] map2, float alpha)
	{
		float[,] noisemap = new float[mapChunkSize, mapChunkSize];
		for (int y = 0; y < mapChunkSize; y++)
		{
			for (int x = 0; x < mapChunkSize; x++)
			{
				noisemap[x, y] = map1[x, y] * alpha + map2[x, y] * (1f - alpha);
				if(noisemap[x, y] < 0)
                {
					noisemap[x, y] = 0;
				}
			}
		}
		return noisemap;
	}

    private void Awake()
    {
		setGeograpyData();
		VornoiNoise.setInit(mapChunkSize, mapChunkSize, geoData.seed, geoData.regionAmount, geoData.centroidValue);
		
	}

    private void Start()
    {
		GameController.Instance.maxIdx = geographies.Length;
    }

    public void setGeograpyData()
    {
		geoData = geographies[GameController.Instance.geograpyIndex].geoData;
		for (int i =0; i< geographies.Length; i++)
        {
			geographies[i].textureData.UpdateMeshHeights(geographies[i].matData, geoData.minHeight, geoData.maxHeight);
			geographies[i].textureData.ApplyToMaterial(geographies[i].matData);
		}
		textureData = geographies[GameController.Instance.geograpyIndex].textureData;
	}

	void OnValuesUpdated()
    {
        if (!Application.isPlaying)
        {
			DrawMapInEditor();
        }
    }

	void OnTextureValuesUpdated()
    {
		geographies[GameController.Instance.geograpyIndex].textureData.ApplyToMaterial(terrainMaterial);
    }


	void OnValidate()
    {
		if (geoData != null)
        {
			geoData.OnValuesUpdated -= OnValuesUpdated;
			geoData.OnValuesUpdated += OnValuesUpdated;
        }

		if( textureData != null)
        {
			textureData.OnValuesUpdated -= OnTextureValuesUpdated;
			textureData.OnValuesUpdated += OnTextureValuesUpdated;
		}
    }



    public void DrawMapInEditor() {

		MapData mapData = GenerateMapDataEditor(Vector2.zero, NoiseType.Perlin);
		MapData mapData2 = GenerateMapDataEditor(Vector2.zero, NoiseType.Vornoi);
		MapData mapData3 = GenerateMapDataEditor(Vector2.zero, NoiseType.Blend);

		MapDisplay display = FindObjectOfType<MapDisplay> ();

		if (drawMode == DrawMode.Perlin_Mesh) {
			display.DrawMesh (MeshGenerator.GenerateTerrainMesh (mapData.heightMap, geoData.meshHeightMultiplier, geoData.meshHeightCurve, editorPreviewLOD));
		}
		else if (drawMode == DrawMode.Vornoi_Mesh)
		{
			display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData2.heightMap, geoData.meshHeightMultiplier, geoData.meshHeightCurve, editorPreviewLOD));
		}
		else if (drawMode == DrawMode.blend_Mesh)
		{
			display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData3.heightMap, geoData.meshHeightMultiplier, geoData.meshHeightCurve, editorPreviewLOD));
		}

	}

	public void RequestMapData(Vector2 centre, Action<MapData> callback) {
		ThreadStart threadStart = delegate {
			MapDataThread (centre, callback);
		};

		new Thread (threadStart).Start ();
	}

	void MapDataThread(Vector2 centre, Action<MapData> callback) {
		MapData mapData = GenerateMapData (centre, noiseTypeDraw);
		lock (mapDataThreadInfoQueue) {
			mapDataThreadInfoQueue.Enqueue (new MapThreadInfo<MapData> (callback, mapData));
		}
	}

	public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback) {
		ThreadStart threadStart = delegate {
			MeshDataThread (mapData, lod, callback);
		};

		new Thread (threadStart).Start ();
	}

	void MeshDataThread(MapData mapData, int lod, Action<MeshData> callback) {
		MeshData meshData = MeshGenerator.GenerateTerrainMesh (mapData.heightMap, geoData.meshHeightMultiplier, geoData.meshHeightCurve, lod);
		lock (meshDataThreadInfoQueue) {
			meshDataThreadInfoQueue.Enqueue (new MapThreadInfo<MeshData> (callback, meshData));
		}
	}

	void Update() {
		if (mapDataThreadInfoQueue.Count > 0) {
			for (int i = 0; i < mapDataThreadInfoQueue.Count; i++) {
				MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue ();
				threadInfo.callback (threadInfo.parameter);
			}
		}

		if (meshDataThreadInfoQueue.Count > 0) {
			for (int i = 0; i < meshDataThreadInfoQueue.Count; i++) {
				MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue ();
				threadInfo.callback (threadInfo.parameter);
			}
		}

	}
	MapData GenerateMapDataEditor(Vector2 centre, NoiseType noiseType)
	{
		geoData = geographies[GameController.Instance.geograpyIndex].geoData;
		float[,] noiseMap = perlinNoise.GenerateNoiseMap(mapChunkSize, mapChunkSize, geoData.seed, geoData.noiseScale, geoData.octaves, geoData.persistance, geoData.lacunarity, centre + geoData.offset, normalizeMode);
		if (noiseType == NoiseType.Vornoi)
		{
			noiseMap = VornoiNoiseEditor.GenerateNoiseMap(mapChunkSize, mapChunkSize, geoData.seed, geoData.regionAmount, geoData.centroidValue);

		}
		else if (noiseType == NoiseType.Blend)
		{
			float[,] noiseMap2 = VornoiNoiseEditor.GenerateNoiseMap(mapChunkSize, mapChunkSize, geoData.seed, geoData.regionAmount, geoData.centroidValue);
			noiseMap = blendMap(noiseMap, noiseMap2, geoData.alpha);
		}


		
		return new MapData(noiseMap);
	}
	MapData GenerateMapData(Vector2 centre, NoiseType noiseType) {

		geoData = geographies[GameController.Instance.geograpyIndex].geoData;
		textureData = geographies[GameController.Instance.geograpyIndex].textureData;

		float[,] noiseMap = perlinNoise.GenerateNoiseMap(mapChunkSize, mapChunkSize, geoData.seed, geoData.noiseScale, geoData.octaves, geoData.persistance, geoData.lacunarity, centre + geoData.offset, normalizeMode);
		if (noiseType == NoiseType.Vornoi)
		{
			noiseMap = VornoiNoise.GenerateNoiseMap(mapChunkSize, mapChunkSize, geoData.seed, geoData.regionAmount, geoData.centroidValue);

		}
		else if (noiseType == NoiseType.Blend)
        {
			float[,] noiseMap2 = VornoiNoise.GenerateNoiseMap(mapChunkSize, mapChunkSize, geoData.seed, geoData.regionAmount, geoData.centroidValue);
			noiseMap = blendMap(noiseMap, noiseMap2, geoData.alpha);
		}



		return new MapData (noiseMap);
	}


	struct MapThreadInfo<T> {
		public readonly Action<T> callback;
		public readonly T parameter;

		public MapThreadInfo (Action<T> callback, T parameter)
		{
			this.callback = callback;
			this.parameter = parameter;
		}

	}
	[System.Serializable]
	public class Geography
	{
		public GeoData geoData;
		public TextureData textureData;
		public Material matData;
	}
}



public struct MapData {
	public readonly float[,] heightMap;
	public MapData (float[,] heightMap)
	{
		this.heightMap = heightMap;
	}
}

