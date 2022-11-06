using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotPosition : MonoBehaviour
{
    [SerializeField] private float radius;
    Transform _camera;


    Vector3 cameraPlanePos
    {
        get
        {
            if (_camera == null)
                _camera = Camera.main.transform;
            return new Vector3(_camera.position.x, 0, _camera.position.z);
        }
    }
    Vector3 cameraPlaneFront
    {
        get
        {
            if (_camera == null)
                _camera = Camera.main.transform;
            return new Vector3(_camera.forward.x, 0, _camera.forward.z).normalized;
        }
    }

    void Start()
    {
        _camera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraPlanePos + cameraPlaneFront * radius;
        transform.LookAt(cameraPlanePos, Vector3.up);
    }
}
