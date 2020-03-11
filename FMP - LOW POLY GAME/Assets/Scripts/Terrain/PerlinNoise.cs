using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    public float PerlinMap(int x, int z, float freq, float amp)
    {        
        return Mathf.PerlinNoise(x / freq, z / freq)*amp;
    }
}
