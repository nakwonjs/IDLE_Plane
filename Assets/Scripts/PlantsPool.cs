using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantsPool : MonoBehaviour
{
    public static PlantsPool Instance;

    
    
    System.Random prng = new System.Random();
    int [] cursor;
    public int maxPool = 600;
    public Transform poolObj;
    public PlantsInfo[] plantsInfo;
    List<GameObject[]> plantsPools = new List<GameObject[]>();

    
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
        cursor = new int[plantsInfo.Length];
        maxIndex = maxPool;
        for(int j = 0; j < plantsInfo.Length; j++)
        {
            GameObject[] pools = new GameObject[maxPool * 4];
            for (int i = 0; i < maxPool * 4; i++)
            {
                int randIdx = prng.Next(0, plantsInfo[j].plantsObj.Count);
                GameObject newObj = Instantiate(plantsInfo[j].plantsObj[randIdx].plantsPrefab);
                newObj.transform.localScale *= plantsInfo[j].plantsObj[randIdx].scale;
                newObj.transform.Rotate(0f, prng.Next(0, 360), 0f);
                newObj.SetActive(false);
                newObj.transform.parent = poolObj;
                pools[i] = newObj;
            }
            plantsPools.Add(pools);
        }

    }

    public GameObject GetPooledObject(int index)
    {
        
        if (cursor[index] < maxIndex)
        {
            return plantsPools[index][cursor[index]++];
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
        for (int j = 0; j < plantsInfo.Length; j++)
        {
            cursor[j] = maxPool * poolIndex;
        }
        DeactiveObj();
    }

    void DeactiveObj()
    {
        for (int j = 0; j < plantsInfo.Length; j++)
        {
            for (int i = cursor[j]; i < maxIndex; i++)
            {
                plantsPools[j][i].SetActive(false);
                plantsPools[j][i].transform.parent = poolObj;
            }
        }
    }

    [System.Serializable]
    public class PlantsInfo
    {
        public List<PlantsObj> plantsObj = new List<PlantsObj>();

    }
    [System.Serializable]
    public struct PlantsObj
    {
        public GameObject plantsPrefab;
        [Range(0,30)]
        public float scale;
    }
}
