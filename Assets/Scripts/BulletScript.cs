using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float bulletSpeed = 5f;
    public Rigidbody bulletBody;

    GameObject player;
    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        bulletBody = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");

        direction = (player.transform.position - transform.position).normalized * bulletSpeed;
        bulletBody.velocity = new Vector3(direction.x, direction.y,direction.z);

        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
          //  Debug.Log("Hit!");
            Destroy(gameObject);
        }
    }
}
