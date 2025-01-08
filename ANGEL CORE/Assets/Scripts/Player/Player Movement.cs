using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //movement
    public float maxSpeed;
    public float speed;

    public float jumpForce;
    public int jumps;
    int jumpsLeft;
    //dash
    public bool useNewDash;
    float dashTimer;

    bool canDash = true;
    bool isDashing;
    public float dashForce = 10f;
    public float dashTime = 0.5f;
    public float dashCooldown = 1f;

    bool onGround;

    Rigidbody rb;

    // cam control variables;
    public float sensitivity;
    public GameObject cam;
    float yaw = 0.0f;
    float pitch = 0.0f;

    //effects
    public GameObject sparkParticles;
    public GameObject jumpSparkParticles;



    void Start()
    {
        //lock cursor in game and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //find the rigidbody component
        rb = GetComponent<Rigidbody>();
        
        //reset jumps
        jumpsLeft = jumps;
        jumps = 0;
    }

    void Update()
    {
        CamControl();
        GroundCheck();
        GetInput();

        //limit velocity
        if (rb.velocity.magnitude > maxSpeed && (isDashing == false || useNewDash == true))
        {
            rb.velocity = rb.velocity / (1f + Time.deltaTime);

            if(rb.velocity.magnitude > maxSpeed * 3f)
            {
                rb.velocity = rb.velocity / (1.05f + Time.deltaTime);
            }
        }
    }

    void GetInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Move(transform.forward);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Move(-transform.forward);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Move(-transform.right);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Move(transform.right);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            if (useNewDash)
            {
                NewDash();
            }
            else
            {
                StartCoroutine(Dash());
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //CHANGED if you are on the ground OR you have jumps left, then jump
            if (onGround || jumpsLeft > 0)
            {
                Jump();
                jumpsLeft--;
                jumpSparkParticles.transform.position = transform.position + (Vector3.up * 0.1f);
                jumpSparkParticles.GetComponent<ParticleSystem>().Play();
            }
        }

        //Update Sparks
        sparkParticles.transform.position = transform.position + (Vector3.up * 0.1f);
        if (onGround)
        {
            sparkParticles.GetComponent<ParticleSystem>().emissionRate = Mathf.FloorToInt(rb.velocity.magnitude * 2f);
        }
        else
        {
            sparkParticles.GetComponent<ParticleSystem>().emissionRate = 0;
        }

        //Update Timers
        if (useNewDash)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer < 0) { canDash = true; }
            if (dashTimer < dashTime) { isDashing = false; }
        }
    }

    void Move(Vector3 dir)
    {
        rb.AddForce(dir * speed * Time.deltaTime);
    }
    
    void NewDash()
    {
        canDash = false;
        isDashing = true;
        dashTimer = dashCooldown;

        // gather what buttons the player is pressing. Horizontal is (-1, A) (1, D), Vertical is (-1, S) (1, W).
        Vector3 inputs = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        // this makes sure holding "A" and "W" wont give you double the boost
        inputs.Normalize();
        // the new velocity is the multitude of the old one * dashforce (reduced for now) and then * to our inputs relative to our rotation.
        rb.velocity = ((transform.right * inputs.x) + (transform.forward * inputs.y)) * (dashForce / 4f) * rb.velocity.magnitude;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originGravity = -9.81f;
        Physics.gravity = new Vector3(0,0,0);
        rb.velocity = new Vector3 (transform.forward.x * dashForce, 0, transform.forward.z * dashForce);
        yield return new WaitForSeconds(dashTime);
        rb.velocity = Vector3.zero;
        Physics.gravity = new Vector3(0,originGravity,0);
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        isDashing = false;
        Debug.Log("dashing");
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce);
    }

    void CamControl()
    {
        //get mouse input
        yaw += sensitivity * Input.GetAxis("Mouse X");
        pitch -= sensitivity * Input.GetAxis("Mouse Y");

        //limit cam angle
        pitch = Mathf.Clamp(pitch, -85.0f, 85.0f);

        //set cam angle
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, yaw, transform.eulerAngles.z);
        cam.transform.eulerAngles = new Vector3(pitch, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    void GroundCheck()
    {
        //shoot a raycast downward to check if the ground is there
        Ray groundRay = new Ray(transform.position + (Vector3.up * 0.1f), Vector3.down);
        if(Physics.Raycast(groundRay, 0.2f))
        {
            if (!onGround)
            {
                jumpSparkParticles.transform.position = transform.position + (Vector3.up * 0.1f);
                jumpSparkParticles.GetComponent<ParticleSystem>().Play();
            }

            onGround = true;
            jumpsLeft = jumps;
        }
        else
        {
            onGround = false;
        }
    }
    private void OnCollisionEnter(Collision collision) 
    {
       if(collision.gameObject.name == "Item") 
        {
            Destroy(collision.gameObject);
            jumps += 1;
            Debug.Log("+ 1 jump");
        }
    }
    
}
