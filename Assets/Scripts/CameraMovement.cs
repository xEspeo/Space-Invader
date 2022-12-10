using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera arCam;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = arCam.transform.rotation;
    }
}
