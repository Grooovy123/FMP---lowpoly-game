﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils {

	static int maxHeight = 120;
	static float smooth = 0.01f;
	static int octaves = 4;
	static float persistence = 0.5f;
    
    class seedGen
    {
        public static int seed = Random.Range(-100000, 100000);
    }

	public static int GenerateStoneHeight(float x, float z)
	{
		float height = Map(0,maxHeight, 0, 1, fBM(x*smooth * 0.8f, z*smooth * 0.8f, octaves+2, persistence* 0.5f));
		return (int) height;
	}

	public static int GenerateHeight(float x, float z)
	{
		float height = Map(0,maxHeight, 0, 1, fBM(x*smooth * 0.3f, z*smooth * 0.3f, octaves, persistence));
		return (int) height;
	}

    public static float fBM3D(float x, float y, float z, float sm, int oct)
    {
        float XY = fBM(x*sm,y*sm,oct,0.5f);
        float YZ = fBM(y*sm,z*sm,oct,0.5f);
        float XZ = fBM(x*sm,z*sm,oct,0.5f);

        float YX = fBM(y*sm,x*sm,oct,0.5f);
        float ZY = fBM(z*sm,y*sm,oct,0.5f);
        float ZX = fBM(z*sm,x*sm,oct,0.5f);

        return (XY+YZ+XZ+YX+ZY+ZX)/6.0f;
    }

	static float Map(float newmin, float newmax, float origmin, float origmax, float value)
    {
        return Mathf.Lerp (newmin, newmax, Mathf.InverseLerp (origmin, origmax, value));
    }

    public static float fBM(float x, float z, int oct, float pers)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;
        float offset = seedGen.seed;
        for (int i = 0; i < oct; i++)
        {
            total += Mathf.PerlinNoise((x + offset) * frequency, (z + offset) * frequency) * amplitude;

            maxValue += amplitude;

            amplitude *= pers;
            frequency *= 2;
        }

        return total / maxValue;
    }

    public float fBMForGrass(float x, float z)
    {
        float total = 0;
        float frequency = 0.3f;
        float amplitude = 1f;        
        float offset = seedGen.seed;
        
        total = Mathf.PerlinNoise((x + offset) * frequency, (z + offset) * frequency) * amplitude;
        
        return total ;
    }

}
