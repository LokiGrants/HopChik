using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector2 minPos;
    public Vector2 maxPos;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = player.transform.position;
        newPos.z = transform.position.z;

        if (newPos.x < minPos.x)
        {
            newPos.x = minPos.x;
        } else if (newPos.x > maxPos.x)
        {
            newPos.x = maxPos.x;
        }

        if (newPos.y < minPos.y)
        {
            newPos.y = minPos.y;
        }
        else if (newPos.y > maxPos.y)
        {
            newPos.y = maxPos.y;
        }

        transform.position = newPos;
    }
}
