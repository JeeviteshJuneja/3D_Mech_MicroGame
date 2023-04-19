using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody bulletRB;
    private float bulletSpeed = 50f;

    private void Awake()
    {
        bulletRB = GetComponent<Rigidbody>();   
    }
    // Start is called before the first frame update
    void Start()
    {
        bulletRB.AddForce(transform.up*bulletSpeed, ForceMode.Impulse);
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().health -= 5;
        }
        if (!collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
