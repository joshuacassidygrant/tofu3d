using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : AbstractMonoService
{

    private float _cameraSpeed = 0.4f;

    public void MoveBy(Vector3 vector)
    {
        transform.position = transform.position + (vector * _cameraSpeed);
    }

}