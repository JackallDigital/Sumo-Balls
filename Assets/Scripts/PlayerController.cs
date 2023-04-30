using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Rigidbody playerRb;
    private GameObject cameraFocalPoint;
    public float movementSpeed = 5f;

    public GameObject projectilePrefab;
    private GameObject tmpProjectile;

    private float powerUpStrength = 15f;
    private float powerJumpStrength = 20f;

    [SerializeField] GameObject[] powerupIndicator;

    public bool hasPowerup = false;
    public bool hasPowerJump = false;
    private bool isGrounded = true;

    // Start is called before the first frame update
    void Awake() {
        playerRb = GetComponent<Rigidbody>();
        cameraFocalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update() {
        if (transform.position.y < -10) {
            //Destroy(gameObject);
            gameObject.transform.position = new Vector3(0.0f, 0.12f, 0.0f);
            playerRb.velocity = Vector3.zero;
        }

        float maxRotationX = 30f;
        float maxRotationY = cameraFocalPoint.transform.localRotation.eulerAngles.y;

        if (isGrounded) {
            float verticalMovement = Input.GetAxis("Vertical");
            playerRb.AddForce(cameraFocalPoint.transform.forward * verticalMovement * movementSpeed);
            powerupIndicator[0].gameObject.transform.position = transform.position;
            powerupIndicator[0].gameObject.transform.rotation = Quaternion.Euler(verticalMovement * maxRotationX, maxRotationY, 0);

            if (hasPowerJump) {
                powerupIndicator[1].transform.position = transform.position;
                //powerupIndicator[1].transform.Rotate(0, 0.5f, 0);
                //powerupIndicator[1].transform.Rotate(verticalMovement * maxRotationX, maxRotationY, 0);

                powerupIndicator[1].gameObject.transform.rotation = Quaternion.Euler(verticalMovement * maxRotationX, maxRotationY, 0);

                if (Input.GetKeyDown(KeyCode.Space)) {
                    isGrounded = false;
                    //Vector3 jumpPosition = transform.position;
                    playerRb.AddForce(Vector3.up * powerJumpStrength, ForceMode.Impulse);

                    hasPowerJump = false;
                    powerupIndicator[1].SetActive(false);
                }
            }
        }
        if (transform.position.y > 1) {
            //playerRb.constraints = RigidbodyConstraints.FreezeAll;
            playerRb.constraints = RigidbodyConstraints.FreezePositionX;
            playerRb.constraints = RigidbodyConstraints.FreezePositionZ;
            playerRb.constraints = RigidbodyConstraints.FreezeRotation;

            if (transform.position.y > 6) {
                transform.position = new Vector3(transform.position.x, 6, transform.position.z);
                StartCoroutine(DropDownWithImpact());
            }
        }
        if (transform.position.y < 1) {
            playerRb.constraints = RigidbodyConstraints.None;
            isGrounded = true;
        }
    }
    
    IEnumerator DropDownWithImpact() {
        yield return new WaitForSeconds(0.2f);

        Vector3 initialPosition = transform.position;
        initialPosition.y = 0.12f;

        playerRb.AddForce(Vector3.down * powerJumpStrength * 2, ForceMode.Impulse);

        Collider[] colliders = Physics.OverlapSphere(initialPosition, 10f);

        foreach (Collider hit in colliders) {

            Rigidbody enemyRb = hit.GetComponent<Rigidbody>();

            if (enemyRb != null) {
                float distance = Vector3.Distance(enemyRb.transform.position, initialPosition);
                //enemyRb.AddExplosionForce(300f, initialPosition, 10f, 0.09f);
                enemyRb.AddExplosionForce(600f / distance, initialPosition, 10f, 0.09f);
            }
        }
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Powerup")) {
            hasPowerup = true;
            Destroy(other.gameObject);
            powerupIndicator[0].SetActive(true);
            StartCoroutine(PowerupCountdownTimer());
        }

        if (other.name.Equals("Powerup Jump(Clone)")) {
            hasPowerJump = true;
            Destroy(other.gameObject);

            powerupIndicator[1].SetActive(true);
        }


        if (other.CompareTag("PowerupProjectiles")) {
            //InvokeRepeating(nameof(LaunchRockets), 0f, 0.5f);  //continous fire
            StartCoroutine(LaunchRockets());
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup) {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 knockAwayFromPlayer = collision.gameObject.transform.position - transform.position;

            enemyRb.AddForce(knockAwayFromPlayer * powerUpStrength, ForceMode.Impulse);
        }
    }

    IEnumerator LaunchRockets() { //can be void if we don't want a timer and have a 1 time shot
        int multiFire = 0;
        
        while (multiFire < 3) {
            foreach (var enemy in FindObjectsOfType<EnemyController>()) {
                tmpProjectile = Instantiate(projectilePrefab, transform.position + Vector3.up, Quaternion.identity);
                tmpProjectile.GetComponent<FireProjectiles>().Fire(enemy.transform);
            }
            multiFire++;
            
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator PowerupCountdownTimer() {
        yield return new WaitForSeconds(8);
        hasPowerup = false;
        powerupIndicator[0].SetActive(false);
    }
}
