using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessWorld : MonoBehaviour
{
    const float scale = 2.5f; //to game Control
    const float viewerMoveThresholdForChunkUpdate = 25f; // to game Control
    const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;
    public Transform viewer;
    public static Vector2 viewerPosition;
	Vector2 viewerPositionOld;
	public float chunksViewDst;

	

	public Material mapMaterial;
	static MapGenerator mapGenerator;
	int chunkSize;
	int chunksGenDst;
	int chunksLimits = 15;
	int currentChunkCoordX;
	int currentChunkCoordY;
	public GameObject tree;
	Vector2 coordHandle = new Vector2(0, 0);

	TerrainChunk chunkHandle;
	List<TerrainChunk> chunkPool = new List<TerrainChunk>();
	List<Vector2> chunkCoords = new List<Vector2>();
	static List<TerrainChunk> terrainChunksLastUpdate = new List<TerrainChunk>();
	System.Random prng = new System.Random();
	
	

	private void Start()
    {
		
		mapGenerator = FindObjectOfType<MapGenerator>();
		viewerPosition = new Vector2(viewer.position.x, viewer.position.z) / scale;
		chunkSize = MapGenerator.mapChunkSize - 1;
		viewerPositionOld = new Vector2(viewerPosition.x, viewerPosition.y);
		chunksGenDst = Mathf.RoundToInt(chunksViewDst / chunkSize);
		UpdateTerrainChunk();
		currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
		currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);


        for (int xOffset = 0; xOffset <= chunksGenDst; xOffset++)
        {
            Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY);

            if (!chunkCoords.Contains(viewedChunkCoord))
            {
                chunkCoords.Add(viewedChunkCoord);
                chunkPool.Add(new TerrainChunk(viewedChunkCoord, chunkSize, transform, mapMaterial));
            }

        }

    }


    private void Update()
    {
		viewerPosition.x = viewer.position.x / scale;
		viewerPosition.y = viewer.position.z / scale;
		if((viewerPositionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate)
        {
			viewerPositionOld.x = viewerPosition.x;
			viewerPositionOld.y = viewerPosition.y;
			UpdateTerrainChunk();

		}
	}

	public void UpdateTerrainChunk()
    {
		currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
		currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);


			for (int xOffset = -chunksGenDst; xOffset <= chunksGenDst; xOffset++)
			{
				Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY);

				if (!chunkCoords.Contains(viewedChunkCoord))
				{
					chunkCoords.Add(viewedChunkCoord);
					chunkPool.Add(new TerrainChunk(viewedChunkCoord, chunkSize, transform, mapGenerator.geographies[GameController.Instance.geograpyIndex].matData));
					if (chunkCoords.Count > 8)
                    {
						chunkCoords.RemoveAt(0);
						
						Destroy(chunkPool[0].meshObject);
						chunkPool.RemoveAt(0);
					}
				}

			}
		
	}

	public class TerrainChunk
	{
		Mesh mesh;
		public GameObject meshObject;
		Vector2 position;
		Bounds bounds;
		private delegate void MyDelegate();
		MyDelegate del;
		MeshRenderer meshRenderer;
		MeshFilter meshFilter;

		MapData mapData;
		bool mapDataReceived;
		System.Action updateCallback;

		public TerrainChunk(Vector2 coord, int size, Transform parent, Material material)
		{
			position = coord * size;
			bounds = new Bounds(position, Vector2.one * size);
			Vector3 positionV3 = new Vector3(position.x, 0, position.y);

			meshObject = new GameObject("Terrain Chunk");
			meshRenderer = meshObject.AddComponent<MeshRenderer>();
			meshFilter = meshObject.AddComponent<MeshFilter>();
			meshRenderer.material = material;
			mesh = new Mesh();

			meshObject.transform.position = positionV3 * scale;
			meshObject.transform.parent = parent;
			meshObject.transform.localScale = Vector3.one * scale;

			
			mapGenerator.RequestMapData(position, OnMapDataReceived);
			GameObject waterinstance = Instantiate(mapGenerator.water);
			waterinstance.transform.parent = meshObject.transform;
			int waterIdx = mapGenerator.textureData.waterLevelIdx;
			if (waterIdx > 0)
            {
				waterinstance.transform.localPosition = new Vector3(0, mapGenerator.geoData.meshHeightCurve.Evaluate(mapGenerator.textureData.layers[waterIdx].startHeight) * mapGenerator.geoData.meshHeightMultiplier, 0);
			}

			
		}
		
        void genPlant()
        {
            int idxlen = mapGenerator.textureData.layers.Length;
            float topLeftX = (MapGenerator.mapChunkSize-1) / -2f;
            float topLeftZ = (MapGenerator.mapChunkSize-1) / 2f;
            for (int y = 0; y < MapGenerator.mapChunkSize; y++)
            {
                for (int x = 0; x < MapGenerator.mapChunkSize; x++)
                {
                    for (int i = 0; i < idxlen; i++)
                    {
						if (mapGenerator.geoData.meshHeightCurve.Evaluate(mapData.heightMap[x, y]) >= mapGenerator.textureData.layers[i].startHeight)
						{
							if (i + 1 < idxlen)
							{
								if (mapGenerator.geoData.meshHeightCurve.Evaluate(mapData.heightMap[x, y]) < mapGenerator.textureData.layers[i + 1].startHeight)
								{
									if (Random.Range(0, 2000) < mapGenerator.textureData.layers[i].density)
									{
										GameObject plant = PlantsPool.Instance.GetPooledObject(mapGenerator.textureData.layers[i].plantIndex);
										if (plant != null)
										{
											plant.transform.parent = meshObject.transform;
											plant.transform.localPosition = new Vector3(topLeftX + x, mapGenerator.geoData.meshHeightCurve.Evaluate(mapData.heightMap[x, y]) * mapGenerator.geoData.meshHeightMultiplier, topLeftZ - y);
											plant.SetActive(true);
										}
									}
									if (Random.Range(0, 800) < mapGenerator.textureData.layers[i].density)
									{
										GameObject grass = GrassPool.Instance.GetPooledObject(0);
										if (grass != null)
										{
											grass.transform.parent = meshObject.transform;
											grass.transform.localPosition = new Vector3(topLeftX + x, mapGenerator.geoData.meshHeightCurve.Evaluate(mapData.heightMap[x, y]) * mapGenerator.geoData.meshHeightMultiplier, topLeftZ - y);
											grass.SetActive(true);
										}
									}
									break;
								}
							}
							else
							{
								if (Random.Range(0, 2000) < mapGenerator.textureData.layers[i].density)
								{
									GameObject plant = PlantsPool.Instance.GetPooledObject(mapGenerator.textureData.layers[i].plantIndex);
									if (plant != null)
									{
										plant.transform.parent = meshObject.transform;
										plant.transform.localPosition = new Vector3(topLeftX + x, mapGenerator.geoData.meshHeightCurve.Evaluate(mapData.heightMap[x, y]) * mapGenerator.geoData.meshHeightMultiplier, topLeftZ - y);
										plant.SetActive(true);
									}
								}
								if (Random.Range(0, 800) < mapGenerator.textureData.layers[i].density)
								{
									GameObject grass = GrassPool.Instance.GetPooledObject(0);
									if (grass != null)
									{
										grass.transform.parent = meshObject.transform;
										grass.transform.localPosition = new Vector3(topLeftX + x, mapGenerator.geoData.meshHeightCurve.Evaluate(mapData.heightMap[x, y]) * mapGenerator.geoData.meshHeightMultiplier, topLeftZ - y);
										grass.SetActive(true);
									}
								}
							}

						}
					}

                }
            }
            PlantsPool.Instance.nextPool();
			GrassPool.Instance.nextPool();

        }

        void OnMapDataReceived(MapData mapData)
		{
			this.mapData = mapData;
			UpdateTerrainChunk();
		}
		void OnMeshDataReceived(MeshData meshData)
		{
			mesh = meshData.CreateMesh();
			meshFilter.mesh = mesh;
		}


		public void UpdateTerrainChunk() {

			mapGenerator.RequestMeshData(mapData, 0, OnMeshDataReceived);
			genPlant();
			//del = genPlant;
			//del();
			
		}

	}

}
