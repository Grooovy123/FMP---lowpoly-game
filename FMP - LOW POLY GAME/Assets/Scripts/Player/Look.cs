using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    float mouseX;
    float mouseY;

    public float mouseSens = 30f;
    float xRotation;

    Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * (mouseSens * 10) * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * (mouseSens * 10) * Time.deltaTime;

        PlayerLook();
    }

    void PlayerLook()
    {
        xRotation -= mouseX;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        transform.localPosition = Quaternion.Euler(xRotation, 0f, 0f);
        Player.Rotate(Vector3.up * mouseX);
    }

}
