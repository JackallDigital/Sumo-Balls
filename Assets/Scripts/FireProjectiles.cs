using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectiles : MonoBehaviour
{
    private Transform enemyTarget;
    private float projectileSpeed = 15.0f;
    private bool isHoming;
    private float projectileStrength = 15.0f;
    private float aliveTimer = 5.0f;

    // Update is called once per frame
    void Update() {
        if (isHoming && enemyTarget != null) {
            Vector3 moveDirection = (enemyTarget.transform.position - transform.position).normalized;
            transform.position += moveDirection * projectileSpeed * Time.deltaTime;
            transform.LookAt(enemyTarget);
        }
        else if(transform.position.y < -0.5){
            Destroy(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    public void Fire(Transform homingTarget) {
        enemyTarget = homingTarget;
        isHoming = true;
        Destroy(gameObject, aliveTimer);
    }

    private void OnTriggerEnter(Collider other) {
        if(enemyTarget != null) {
            Rigidbody targetRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 knockAwayFromProjectile = (targetRigidbody.transform.position - transform.position).normalized;

            targetRigidbody.AddForce(knockAwayFromProjectile * projectileStrength, ForceMode.Impulse);
        }
        Destroy(gameObject);
    }
}