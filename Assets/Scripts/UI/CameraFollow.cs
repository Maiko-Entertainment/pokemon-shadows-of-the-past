using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Camera myCamera;
    public float time = 1;
    public float zoomTime = 0.5f;

    private Vector2 currentSpeed = Vector2.zero;
    private Vector2 target;
    private Vector3 initialPosition;
    private float currentSizeSpeed = 0;
    private float sizeTarget;
    private float sizeInitial;

    private void Start()
    {
        initialPosition = myCamera.transform.position;
        sizeInitial = myCamera.orthographicSize;
        SetTarget(initialPosition);
        SetSizeTarget(sizeInitial);
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target;
    }

    public void SetSizeTarget(float size)
    {
        sizeTarget = size;
    }

    public void ResetCamera()
    {
        SetTarget(initialPosition);
        SetSizeTarget(sizeInitial);
    }

    private void Update()
    {
        transform.position = Vector2.SmoothDamp(transform.position, target, ref currentSpeed, time);
        transform.position += Vector3.forward * initialPosition.z;
        myCamera.orthographicSize = Mathf.SmoothDamp(myCamera.orthographicSize, sizeTarget, ref currentSizeSpeed, zoomTime);
    }
}
