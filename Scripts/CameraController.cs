using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Makes camera move up or down, depending on the users mouse scroll.

    private Vector3 pos;
    private float speed;

    public void SetPos(Vector3 camPos)
    {
       pos = camPos;
       speed = 5f;
    }

    void Update()
    {   

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            transform.position = new Vector3(0, pos.y -= speed, 0);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            transform.position = new Vector3(0, pos.y += speed, 0);
        }
    }
}
