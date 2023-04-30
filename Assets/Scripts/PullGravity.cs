using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullGravity : MonoBehaviour {
    private GameObject playerPosition;
    private Rigidbody bossRb;
    private Renderer bossRenderColor;

    private WaitForSeconds pullInterval = new WaitForSeconds(2);
    private WaitForSeconds pauseInterval = new WaitForSeconds(4);

    private bool isPulling = true;
    private bool hasToWait = true;

    // Start is called before the first frame update
    void Start() {
        playerPosition = GameObject.Find("Player");
        bossRb = gameObject.GetComponent<Rigidbody>();
        bossRenderColor= gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update() {
        if (isPulling) {
            PullPlayerToBoss();

            if (hasToWait) { 
                StartCoroutine(PullForceTimer());
            }
        }
    }

    IEnumerator PullForceTimer() {
        hasToWait = false;
        
        bossRenderColor.material.color = new Color32(222, 177, 26, 255);

        yield return pullInterval;
        isPulling = false;
        // bossRb.constraints = RigidbodyConstraints.None;
        bossRenderColor.material.color = Color.white;
        yield return pauseInterval;
        
        isPulling = true;
        hasToWait = true;
    }

    private void PullPlayerToBoss() {
        //bossRb.constraints = RigidbodyConstraints.FreezeAll;
        Vector3 initialPosition = transform.position;
        Rigidbody playerRb = playerPosition.GetComponent<Rigidbody>();
        float distance = Vector3.Distance(playerRb.transform.position, initialPosition);
        playerRb.AddExplosionForce(-4f / distance, initialPosition, 20f, 0.09f);
    }
}