using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public Vector3 offsetPosition;
    public float cameraDepth = -70f;
    public float smoothSpeed;
    public float clampMinX = 4;
    public float clampMaxX = 4;
    public float clampMinY = 4;
    public float clampMaxY = 4;

    private Vector3 finalPos;

    void Update()
    {
        finalPos = target.position + offsetPosition;
        finalPos = new Vector3(Mathf.Clamp(finalPos.x, clampMinX, clampMaxX), Mathf.Clamp(finalPos.y, clampMinY, clampMaxY), finalPos.z);

        transform.position = Vector3.Lerp(transform.position, new Vector3(finalPos.x, finalPos.y, cameraDepth), smoothSpeed * Time.deltaTime);
    }
}
