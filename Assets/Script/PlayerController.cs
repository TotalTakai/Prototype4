using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private readonly float speed = 4.0f;
    private readonly float powerupStrenght = 15.0f;
    private bool hasPowerup = false;
    private Rigidbody playerRB;
    private GameObject focalPoint;
    private GameObject tmpRocket;
    private Coroutine powerupCountdown;

    public GameObject powerupIndicator;
    public GameObject rocketPrefab;
    public PowerupType currentPowerUp = PowerupType.None;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        playerRB.AddForce(focalPoint.transform.forward * verticalInput * speed);
        powerupIndicator.gameObject.transform.position = transform.position + new Vector3(0, -0.6f, 0);

        if (currentPowerUp == PowerupType.Rockets && Input.GetKeyDown(KeyCode.F)) //If player press F during rockets powerup, he will shoot a rcoket
        {
            LaunchRockets();
        }
    }

    // Checks when the player collects a powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            if(hasPowerup == true)
            {
                hasPowerup = false;
                StopCoroutine("PowerupCountdownRoutine");
            }
            Destroy(other.gameObject);
            currentPowerUp = other.gameObject.GetComponent<Powerups>().powerupType;
            hasPowerup = true;
            powerupIndicator.gameObject.SetActive(true);
            StartCoroutine("PowerupCountdownRoutine"); //Works on a different thread
        }
    }

    // Uses a different thread to wait 7 seconds to deactivate the powerup
    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        currentPowerUp = PowerupType.None;
        powerupIndicator.gameObject.SetActive(false);
    }

    // Sends enemy away if the player have the powerup
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && hasPowerup && currentPowerUp == PowerupType.Pushback)
        {
            Rigidbody enemyRB = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
            enemyRB.AddForce(awayFromPlayer * powerupStrenght, ForceMode.Impulse);

        }
    }

    // Launches the rockets
    private void LaunchRockets()
    {
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            tmpRocket = Instantiate(rocketPrefab, transform.position + Vector3.up,
            Quaternion.identity);
            tmpRocket.GetComponent<RocketBehaviour>().Fire(enemy.transform);
        }
    }
}
