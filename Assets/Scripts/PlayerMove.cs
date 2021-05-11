using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashTime;
    [SerializeField] private Camera cam;
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform spawnPoint;
    

    private bool jumpClick;
    private bool dashClick;
    private bool dashPossible;
    private bool jumpPossible;

    public int deathCounter;
    public int muddledCounter;
    public int currentMuddle;

    private int dashCounter;
    
    float horizontalIn;
    float verticalIn;

    public Transform groundCheck;
    public float speed = 10f;
    public float gravityMult = 2.5f;

    private Rigidbody playerBody;
    
    
    Vector3 move;



    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
        deathCounter = 0;
        muddledCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpClick = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            dashClick = true;
            dashCounter++;
        }

        if ( dashClick && dashCounter < 2)
        {
            StartCoroutine(Dashing());

        }

       
        horizontalIn = Input.GetAxisRaw("Horizontal");
        verticalIn = Input.GetAxisRaw("Vertical");

        move = (horizontalIn * transform.right + verticalIn * transform.forward).normalized;
    }

    private void FixedUpdate()
    {
        if(Physics.OverlapSphere(groundCheck.position, .2f, playerMask).Length != 0)
        {
            jumpPossible = true;
            dashCounter = 0;
           
        }
        else if (dashCounter == 1)
        {
            jumpPossible = true;

           
        }
        else
        {
            jumpPossible = false;
        }

      


        if (jumpClick && jumpPossible)
        {
            playerBody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            jumpPossible = false;
            jumpClick = false;
        }

        if(playerBody.velocity.y < 0)
        {
            playerBody.velocity += Vector3.up * Physics.gravity.y * (gravityMult - 1) * Time.deltaTime;
           
        }

        
        Movement();
    }

     private void Movement()
    {
        Vector3 playerGravity = new Vector3(0, playerBody.velocity.y, 0);
        playerBody.velocity = move * speed * Time.deltaTime;
        playerBody.velocity += playerGravity;
    }

    public IEnumerator Dashing()
    {
       
        playerBody.AddForce(transform.forward * dashForce, ForceMode.VelocityChange);
        //Countering gravity
        if(playerBody.velocity.y < 0)
        {
            playerBody.velocity += Vector3.up * Physics.gravity.y * -1;
        }
       
        yield return new WaitForSeconds(dashTime);
        playerBody.velocity = Vector3.zero;
        dashClick = false;
        dashCounter = 1;
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Bullet") && currentMuddle > 0)
        {
            currentMuddle--;
        }
        else if(collision.gameObject.tag.Equals("Bullet") || collision.gameObject.tag.Equals("Hazard"))
        {           
            deathCounter++;
            if(deathCounter % 4 == 0)
            {
                muddledCounter++;
            }
            currentMuddle = muddledCounter;

            transform.position = spawnPoint.transform.position;
        }

        if (collision.gameObject.tag.Equals("Finish"))
        {
            muddledCounter = 0;            
        }
    }
}
