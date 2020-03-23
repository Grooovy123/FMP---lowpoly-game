using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Tile
{
    public GameObject theTile;
    public float creationTime;

    public Tile(GameObject t, float ct)
    {
        theTile = t;
        creationTime = ct;
    }
}

public class InfiniteTerrain : MonoBehaviour
{
    public GameObject terrain;
    public GameObject player;

    [SerializeField]
    int radius = 6;
    int chunkSize;

    Vector3 startPos;

    Hashtable tiles = new Hashtable();

    float updateTime;
    bool move;

    void Awake()
    {
        chunkSize = terrain.GetComponent<MeshGen>().chunkSize;
    }

    void Start()
    {
        PlaneGenerator();
        //Debug.Log("Plane generation complete");
    }

    void Update()
    {
        int xMove = (int)(player.transform.position.x - startPos.x);
        int zMove = (int)(player.transform.position.z - startPos.z);

        if (Mathf.Abs(xMove) >= chunkSize * 0.25f || Mathf.Abs(zMove) >= chunkSize * 0.25f)
        {
            //Debug.Log("Running plane update");
            UpdatePlane();
            move = true;
            //Debug.Log("updated plane complete");
        }

        UpdateTiles();
        //Debug.Log("update tiles");
    }

    void UpdateTiles()
    {
        Hashtable newTerrain = new Hashtable();
        foreach (Tile tls in tiles.Values)
        {
            if (tls.creationTime != updateTime)
            {
                Destroy(tls.theTile);
            }
            else
            {
                newTerrain.Add(tls.theTile.name, tls);
            }
        }

        tiles = newTerrain;
        if (move)
        {
            startPos = player.transform.position;
            move = false;
        }

    }

    void UpdatePlane()
    {
        updateTime = Time.realtimeSinceStartup;

        int playerX = (int)(Mathf.Floor(player.transform.position.x / chunkSize) * chunkSize);
        int playerZ = (int)(Mathf.Floor(player.transform.position.z / chunkSize) * chunkSize);

        for (int z = -radius; z <= radius; z++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                Vector3 pos = new Vector3((x * chunkSize + playerX), 0,
                                          (z * chunkSize + playerZ));

                string tileName = "Tile_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString();

                if (!tiles.ContainsKey(tileName))
                {
                    GameObject t = Instantiate(terrain, pos, Quaternion.identity);
                    t.name = tileName;
                    Tile tile = new Tile(t, updateTime);
                    tiles.Add(tileName, tile);
                }
                else
                {
                    (tiles[tileName] as Tile).creationTime = updateTime;
                }
            }
        }
    }

    void PlaneGenerator()
    {
        updateTime = Time.realtimeSinceStartup;

        for (int z = -radius; z <= radius; z++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                Vector3 pos = new Vector3((x * chunkSize + startPos.x), 0,
                                          (z * chunkSize + startPos.z));
                GameObject t = Instantiate(terrain, pos, Quaternion.identity);

                t.name = "Tile_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString();

                Tile tile = new Tile(t, updateTime);
                tiles.Add(t.name, tile);
            }
        }
    }
}