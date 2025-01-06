using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed;
    public float speed;

    public float jumpForce;
    public int jumps;
    int jumpsLeft;

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
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity / 1.1f;
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //if you are on the ground and you have jumps left, then jump
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
        
    }

    void Move(Vector3 dir)
    {
        rb.AddForce(dir * speed * Time.deltaTime);
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
