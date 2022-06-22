using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassPool : MonoBehaviour
{
    public static GrassPool Instance;

    
    
    System.Random prng = new System.Random();
    int [] cursor;
    public int maxPool = 600;
    public Transform poolObj;
    public GameObject baseQuad;
    public GrassInfo[] grassInfo;
    List<GameObject[]> grassPools = new List<GameObject[]>();

    
    int maxIndex;
    int poolIndex = 0;
    int nextPoolIndex = 1;
    

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            
        }
    }



    private void Start()
    {
        cursor = new int[grassInfo.Length];
        maxIndex = maxPool;
        for(int j = 0; j < grassInfo.Length; j++)
        {
            GameObject[] pools = new GameObject[maxPool * 4];
            for (int i = 0; i < maxPool * 4; i++)
            {
                int randIdx = prng.Next(0, grassInfo[j].grassObj.Count);

                GameObject newObj = Instantiate(baseQuad);
                MeshRenderer[] mRender = newObj.GetComponentsInChildren<MeshRenderer>();
                for (int k = 0; k < mRender.Length; k++)
                {
                    mRender[k].material = grassInfo[j].grassObj[randIdx].grassPrefab;
                }
                
                newObj.transform.localScale *= grassInfo[j].scale * (1 + prng.Next(0,30)/100f);
                newObj.transform.Rotate(0f, prng.Next(0, 360), 0f);
                newObj.SetActive(false);
                newObj.transform.parent = poolObj;
                pools[i] = newObj;
            }
            grassPools.Add(pools);
        }

    }

    public GameObject GetPooledObject(int index)
    {
        
        if (cursor[index] < maxIndex)
        {
            return grassPools[index][cursor[index]++];
        }
        else
        {
            return null;
        }
    }

    public void nextPool()
    {
        poolIndex++;

        if (poolIndex > 3)
        {
            poolIndex = 0;
        }
        maxIndex = maxPool * (poolIndex + 1);
        for (int j = 0; j < grassInfo.Length; j++)
        {
            cursor[j] = maxPool * poolIndex;
        }
        DeactiveObj();
    }

    void DeactiveObj()
    {
        for (int j = 0; j < grassInfo.Length; j++)
        {
            for (int i = cursor[j]; i < maxIndex; i++)
            {
                grassPools[j][i].SetActive(false);
                grassPools[j][i].transform.parent = poolObj;
            }
        }
    }

    [System.Serializable]
    public class GrassInfo
    {
        public List<PlantsObj> grassObj = new List<PlantsObj>();
        [Range(0.01f, 30)]
        public float scale;
    }
    [System.Serializable]
    public struct PlantsObj
    {
        public Material grassPrefab;
    }
}
