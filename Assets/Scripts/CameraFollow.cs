using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://generalistprogrammer.com/unity/unity-2d-how-to-make-camera-follow-player/
public class CameraFollow : MonoBehaviour
{

    public Transform followTransform;
    

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = new Vector3(followTransform.position.x, followTransform.position.y, this.transform.position.z);
        
        
    }
}