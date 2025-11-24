using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTitle : MonoBehaviour
{
    public Transform transformCam;

    public float speed = 10f;
    void Update()
    {
        transformCam.position += Vector3.forward * speed * Time.deltaTime;
    }
}
