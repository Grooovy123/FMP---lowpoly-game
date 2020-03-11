using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public GameObject terrain;

    void Awake()
    {
        MeshGen meshGen = terrain.GetComponent<MeshGen>();
    }

    // Start is called before the first frame update
    void Start()
    {        
        GameObject tile = Instantiate(terrain, new Vector3(0, 0, 0), Quaternion.identity);
        tile.name = ("Terrain tile " + terrain.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
