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
    [SerializeField] private AudioSource splash;
    [SerializeField] private AudioSource hit;
    [SerializeField] private AudioSource dash;


    private GameObject platformSection1;
    private GameObject platformSection2;
    private GameObject platformSection3;

    private GameObject enemiesSection1;
    private GameObject enemiesSection2;
    private GameObject enemiesSection3;

    private bool jumpClick;
    private bool dashClick;
    private bool dashPossible;
    private bool jumpPossible;

    public bool safe;
    public bool endGame;

    public int deathCounter;
    public int muddledCounter;
    public int currentMuddle;

 
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

        endGame = false;

        platformSection1 = GameObject.FindGameObjectWithTag("Platform1");
        platformSection2 = GameObject.FindGameObjectWithTag("Platform2");
        platformSection3 = GameObject.FindGameObjectWithTag("Platform3");
        platformSection2.SetActive(false);
        platformSection3.SetActive(false);

        enemiesSection1 = GameObject.FindGameObjectWithTag("Enemies1");
        enemiesSection2 = GameObject.FindGameObjectWithTag("Enemies2");
        enemiesSection3 = GameObject.FindGameObjectWithTag("Enemies3");
        enemiesSection2.SetActive(false);
        enemiesSection3.SetActive(false);
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
            // dashCounter++;
        }

        if (dashClick && dashPossible)
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
            dashPossible = true;
           
        }
     
        if (jumpClick && jumpPossible)
        {
            playerBody.AddForce((Vector3.up * jumpForce), ForceMode.VelocityChange);
            jumpPossible = false;
            jumpClick = false;
        }

        //For a more natural fall increases gravity when falling down
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
        dash.Play();
       
        //Countering gravity
        if(playerBody.velocity.y < 0)
        {
            playerBody.velocity += Vector3.up * Physics.gravity.y * -1;
        }
       
        yield return new WaitForSeconds(dashTime);
        playerBody.velocity = Vector3.zero;
        dashClick = false;
        dashPossible = false;
       
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag.Equals("Bullet") && currentMuddle > 0)
        {
            currentMuddle--;
            hit.Play();
            
        }
        else if(collision.gameObject.tag.Equals("Bullet") || collision.gameObject.tag.Equals("WaterHazard") || collision.gameObject.tag.Equals("Hazard"))
        {
            if (collision.gameObject.tag.Equals("WaterHazard"))
            {
                splash.Play();
            }
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

        if (collision.gameObject.tag.Equals("Wall"))
        {
            jumpPossible = true;
            dashPossible = true;
        }

     }

    


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("SafeZone"))
        {
            safe = true;
            //Debug.Log("In Safe Zone!");
        }

        //changing spawn point
        if (other.gameObject.tag.Equals("SPoint"))
        {
            spawnPoint.transform.position = other.transform.position;
        }

        //Deleting previous section
        if (other.gameObject.tag.Equals("Trigger1"))
        {
            Destroy(platformSection1);
            Destroy(enemiesSection1);
            platformSection2.SetActive(true);
            enemiesSection2.SetActive(true);
        }

        if (other.gameObject.tag.Equals("Trigger2")) 
        {
            Destroy(platformSection2);
            Destroy(enemiesSection2);
            platformSection3.SetActive(true);
            enemiesSection3.SetActive(true);
        }

        if (other.gameObject.tag.Equals("Trigger3"))
        {
            Time.timeScale = 0;
            endGame = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("SafeZone"))
        {
            safe = false;
          // Debug.Log("Out of Safe Zone!");
        }
    }
}
