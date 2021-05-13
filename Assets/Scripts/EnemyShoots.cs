using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoots : MonoBehaviour
{
    [SerializeField] private float fireDelay = 3f;
    [SerializeField] GameObject bullet;
    [SerializeField] float optimumDistance = 30f;

    GameObject playerBody;

    public Transform player;    
    public Transform spawnPoint;

    public bool canShoot;
    Vector3 spawn;

    private float nextFire;
    private float distance;
    private bool pauseFire;
       
    // Start is called before the first frame update
    void Start()
    {
        playerBody = GameObject.FindGameObjectWithTag ("Player");
        nextFire = Time.time + fireDelay;
        pauseFire = false;
        canShoot = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);

        spawn = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);
        distance = Vector3.Distance(player.position, transform.position);

        if (Input.GetKeyDown(KeyCode.P) && pauseFire == false)
        {
            pauseFire = true;
        } 
        else if (Input.GetKeyDown(KeyCode.P))
        {
            pauseFire = false;
        }

       canShoot = !playerBody.GetComponent<PlayerMove>().safe;


        if (distance <= optimumDistance)
        {
            Fire();
        }  
    }

    private void Fire()
    {
        if (Time.time > nextFire && !pauseFire && canShoot)
        {
            Instantiate(bullet, spawn, Quaternion.identity);
            nextFire = Time.time + fireDelay;
        }
           
    }
}
