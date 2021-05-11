using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoots : MonoBehaviour
{
    [SerializeField] private float fireDelay = 3f;
    [SerializeField] GameObject bullet;
    public Transform player;
    
    public Transform spawnPoint;

    Vector3 spawn;

    private float nextFire;
    private bool pauseFire;
       
    // Start is called before the first frame update
    void Start()
    {
        nextFire = Time.time + fireDelay;
        pauseFire = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);

        spawn = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);

        if (Input.GetKeyDown(KeyCode.P) && pauseFire == false)
        {
            pauseFire = true;
        } 
        else if (Input.GetKeyDown(KeyCode.P))
        {
            pauseFire = false;
        }
        Fire();
    }

    private void Fire()
    {
        if (Time.time > nextFire && !pauseFire)
        {
            Instantiate(bullet, spawn, Quaternion.identity);
            nextFire = Time.time + fireDelay;
        }
           
    }
}
