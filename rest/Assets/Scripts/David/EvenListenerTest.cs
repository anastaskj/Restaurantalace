using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvenListenerTest : MonoBehaviour
{
    public GameObject leftCube;
    public GameObject rightCube;

    void Start()
    {
        PlayerInput.OnSwipe += DirectionMessage;
        PlayerInput.OnMove += Move;
        PlayerInput.OnDrag += Drag;
    }

    private void Drag(Vector2 position)
    {
        leftCube.transform.position = new Vector3(position.x / 1000, position.y / 1000, 0);
    }

    private void Move(float distance)
    {
        leftCube.transform.position = new Vector3(leftCube.transform.position.x - distance, 0, 0);
        rightCube.transform.position = new Vector3(rightCube.transform.position.x - distance, 0, 0);
    }

    private void DirectionMessage(float direction)
    {
        if (direction == -1)
        {
            leftCube.transform.position = new Vector3(0, 0, 0);
            rightCube.transform.position = new Vector3(5, 0, 0);
        }
        else
        {
            leftCube.transform.position = new Vector3(-5, 0, 0);
            rightCube.transform.position = new Vector3(0, 0, 0);
        }
    }
}
