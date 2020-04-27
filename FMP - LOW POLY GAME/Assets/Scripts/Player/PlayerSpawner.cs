using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    public void SpawnPlayer()
    {
        Instantiate(player, new Vector3(0,40,0), Quaternion.identity);        
    } 
}
