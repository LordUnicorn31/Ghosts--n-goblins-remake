using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform playerT;

    // Update is called once per frame
    void Update()
    {
        CameraMove();
    }

    void CameraMove()
    {
        float xPos = playerT.position.x;
        Vector3 position = transform.position;
        position.x = xPos;
        transform.position = position;
    }
}
