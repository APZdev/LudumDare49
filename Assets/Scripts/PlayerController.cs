using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    public float playerSpeed = 5f;
    public float gravityForce = 5f;

    public GameObject playerGraphics;
    public float flipSpeed;

    private bool isFliped;
    private float horizontal;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        PlayerInputs();
        PlayerGraphics();
        PlayerMovements();
    }

    private void PlayerInputs()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
    }

    private void PlayerGraphics()
    {
        //I don't use ternary to keep the current flip state if player presses nothing
        if(horizontal > 0)
            isFliped = true;
        else if(horizontal < 0)
            isFliped = false;


        Vector3 finalRot = isFliped ? new Vector3(0, 0, 0) : new Vector3(0, 180, 0);

        playerGraphics.transform.localEulerAngles = Vector3.Lerp(playerGraphics.transform.localEulerAngles, finalRot, flipSpeed * Time.deltaTime);
    }

    private void PlayerMovements()
    {
        horizontal = Input.GetAxisRaw("Horizontal");


        rb.AddForce(transform.right * Input.GetAxisRaw("Horizontal") * playerSpeed * Time.deltaTime);

        if (!GroundCheck())
            rb.AddForce(transform.up * -gravityForce);
    }

    private bool GroundCheck()
    {
        Debug.DrawRay(transform.position, Vector3.down);
        return Physics.Raycast(transform.position, Vector3.down, 0.1f);
    }
}
