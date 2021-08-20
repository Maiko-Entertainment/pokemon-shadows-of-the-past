using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Camera myCamera;
    public float time = 1;
    public float zoomTime = 0.5f;
    public Vector2 cameraChangeMovement;
    public float cameraIdleSpeed = 0.2f;

    private Vector2 currentSpeed = Vector2.zero;
    private Vector2 target;
    private Vector3 initialPosition;
    private float currentSizeSpeed = 0;
    private float sizeTarget;
    private float sizeInitial;
    private bool isIdle = true;
    private int direction = 1;

    private void Start()
    {
        initialPosition = myCamera.transform.position;
        sizeInitial = myCamera.orthographicSize;
        ResetCamera();
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target;
        isIdle = false;
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

    IEnumerator SetIdleAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        isIdle = true;
    }

    public void SetIdle(float delay)
    {
        StartCoroutine(SetIdleAfter(delay));
    }

    private void Update()
    {
        if (isIdle)
        {
            Vector2 goal = (Vector2)initialPosition + cameraChangeMovement * direction;
            float distance = Vector2.Distance(transform.position, goal);
            if (distance == 0)
            {
                direction *= -1;
            }
            else
            {
                float speedBoost = Mathf.Abs(goal.x) < transform.position.x ? 4f : 1f;
                transform.position = Vector2.MoveTowards(transform.position, goal, cameraIdleSpeed * speedBoost * Time.deltaTime);
            }
        }
        else
        {
            transform.position = Vector2.SmoothDamp(transform.position, target, ref currentSpeed, time);
            isIdle = false;
        }
        myCamera.orthographicSize = Mathf.SmoothDamp(myCamera.orthographicSize, sizeTarget, ref currentSizeSpeed, zoomTime);
        transform.position += Vector3.forward * initialPosition.z;
    }
}
