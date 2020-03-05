using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    // Declearing varables
    public float freq;
    public float amp;    

    public float PerlinMap(int x, int z, float freq, float amp)
    {        
        return Mathf.PerlinNoise(x / freq, z / freq)*amp;
    }
}
