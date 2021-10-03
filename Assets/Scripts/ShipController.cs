using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float waveSpeed;
    public float waveAmount;
    public float posSmooth;

    private Vector3 initialPosition;


    public float rotAngle;
    public float rotSpeed;
    public float rotSmooth;

    private Vector3 initialRotation;


    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.eulerAngles;
    }

    void Update()
    {
        //Vertical movements
        float offsetY = Mathf.Abs(waveAmount * Mathf.Sin(waveSpeed * Time.time));
        transform.position = Vector3.Lerp(transform.position, initialPosition + Vector3.up * offsetY, posSmooth * Time.deltaTime);

        //Rotation
        float rotX = (Mathf.Abs(Mathf.Sin(rotSpeed * Time.time + 0.5f)) - 0.5f)  * 2;
        transform.localRotation = Quaternion.Lerp(transform.localRotation,
                                                  Quaternion.Euler(new Vector3(rotAngle * rotX, transform.localEulerAngles.y, transform.localEulerAngles.z)),
                                                  rotSmooth * Time.deltaTime);
    }
}
