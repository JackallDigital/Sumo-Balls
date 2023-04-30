using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float enemySpeed = 3f;

    private Rigidbody enemyRb;
    private GameObject playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        playerPosition = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -10)
        {
            Destroy(gameObject);
        }

        Vector3 enemyMoveDirection = (playerPosition.transform.position - transform.position).normalized;
        enemyRb.AddForce(enemyMoveDirection * enemySpeed);
    }
}