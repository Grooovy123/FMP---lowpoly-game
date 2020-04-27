using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPos : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {        

        Ray ray = new Ray(this.transform.position, Vector3.down);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100))
        {
            Debug.Log(hit.distance);
            Vector3 startPos = new Vector3(0, hit.distance - 0.1f, 0);
            transform.position = startPos;
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.green);
        }
    }   
    
    void Update()
    {
        
    }
}
