using System;
using System.Collections;
using System.Collections.Generic;
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
    //slam
    public float slamForce;
    bool isSlamming;
    float slamJumpBoost; //how much jumping after slamming will boost the jump, based on velocity when hitting the ground
    float slamSpeed; //your velocity when the slam ended
    float slamBoostTimer; //time until slamboost ends

    bool canDash = true;
    bool isDashing;
    public float dashForce = 10f;
    public float dashTime = 0.5f;
    public float dashCooldown = 1f;

    bool onGround;

    Rigidbody rb;
    TrailRenderer tr;

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
        //if sens is <= 0 on launch, set it to 50
        if (PlayerPrefs.GetFloat("sens") <= 0)
        {
            PlayerPrefs.SetFloat("sens", 50);
        }

        //find the rigidbody component
        rb = GetComponent<Rigidbody>();
        //find the line renderer
        tr = GetComponentInChildren<TrailRenderer>();
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
        Vector3 horVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (horVel.magnitude > maxSpeed && (isDashing == false || useNewDash == true))
        {
            horVel = horVel / (1f + Time.deltaTime);

            if(horVel.magnitude > maxSpeed * 3f)
            {
                horVel = horVel / (1.05f + Time.deltaTime);
            }
            // repeated on purpose
            if (horVel.magnitude > maxSpeed * 3f)
            {
                horVel = horVel / (1.05f + Time.deltaTime);
            }

            rb.velocity = new Vector3(horVel.x, rb.velocity.y, horVel.z);
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
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //CHANGED if you are on the ground OR you have jumps left, then jump
            if (onGround || jumpsLeft > 0)
            {
                isSlamming = false;
                Jump();
                slamBoostTimer = 0;
                jumpsLeft--;
                jumpSparkParticles.transform.position = transform.position + (Vector3.up * 0.1f);
                jumpSparkParticles.GetComponent<ParticleSystem>().Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!onGround || !isSlamming)
            {
                Slam();
                slamBoostTimer = 0.1f;
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

        //slam logic
        if (isSlamming) { tr.emitting = false; }
        else { tr.emitting = true; }
        if(isSlamming && !onGround) { slamJumpBoost = rb.velocity.y * -1f; }
        if(isSlamming && onGround)
        {
            isSlamming = false;
        }

        //Update Timers
        if (useNewDash)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer < 0) { canDash = true; }
            if (dashTimer < dashTime) { isDashing = false; }
        }
        if (!isSlamming)
        {
            slamBoostTimer -= Time.deltaTime;
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


    void Slam()
    {
        isSlamming = true;
        rb.velocity = Vector3.down * slamForce;
    }

    void Jump()
    {
        if(slamBoostTimer > 0 && !isSlamming)
        {
            rb.AddForce(Vector3.up * jumpForce * (slamJumpBoost / 30f));
        }
        else
        {
            rb.AddForce(Vector3.up * jumpForce);
        }
        
    }

    void CamControl()
    {
        sensitivity = PlayerPrefs.GetFloat("sens") / 10f;

        if(Cursor.lockState == CursorLockMode.Locked)
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
