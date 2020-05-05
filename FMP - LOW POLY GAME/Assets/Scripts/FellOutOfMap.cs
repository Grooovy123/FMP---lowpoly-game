using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FellOutOfMap : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y <= 10 || Input.GetKeyDown(KeyCode.R))
        {
            this.transform.position = new Vector3(0, 70, 0);
            this.gameObject.SetActive(false);
            this.gameObject.SetActive(true);
        }
            
    }
}
