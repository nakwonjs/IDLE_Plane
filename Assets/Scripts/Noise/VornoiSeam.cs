using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VornoiSeam
{
    static float[,] seam = new float[0, 0];
    static public void setSeam(float [,] _seam)
    {
        seam = _seam;
    }

    static public float[,] getSeam()
    {
        return seam;
    }
    
}
