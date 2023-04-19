using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNav : MonoBehaviour
{
    public GameObject bulletPrefab;
        
    private NavMeshAgent enemyNavAgent;
    private GameObject player;
    // Start is called before the first frame update
    private void Awake()
    {
        enemyNavAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
    }
    void Start()
    {
        enemyNavAgent.stoppingDistance = 10f;
        InvokeRepeating("Shoot", 2, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        NavMeshHit hit;
        if(enemyNavAgent.Raycast(player.transform.position, out hit))
        {
            enemyNavAgent.stoppingDistance = 3f;
        }
        else
        {
            enemyNavAgent.stoppingDistance = 10f;
        }
        enemyNavAgent.destination = player.transform.position;
    }

    private void Shoot()
    {
        Vector3 lookAt = (player.transform.position - transform.position).normalized*2;
        Quaternion lookRot = Quaternion.LookRotation(lookAt);

        Instantiate(bulletPrefab, transform.position+lookAt, lookRot* bulletPrefab.transform.rotation);
    }
}

