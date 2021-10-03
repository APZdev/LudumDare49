using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;

    public float playerSpeed = 5f;
    public float jumpForce = 5f;
    public float gravityForce = 5f;

    public GameObject playerGraphics;
    public float flipSpeed;

    private bool isFliped;
    private float horizontal;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }   

    private void Update()
    {
        PlayerGraphics();
        PlayerMovements();
        PlayerAnimations();
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

        isGrounded = GroundCheck();

        if (!isGrounded)
            rb.AddForce(transform.up * -gravityForce);

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void PlayerAnimations()
    {
        anim.SetBool("IsMoving", horizontal != 0 && rb.velocity.sqrMagnitude > 1f);
    }

    private bool GroundCheck()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.1f);
    }
}
