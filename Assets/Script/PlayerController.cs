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
    

    // Smash Powerup Info, All public for IEnumerator
    public readonly float hangTime = 0.5f;
    public readonly float smashSpeed = 30.0f;
    public readonly float explosionForce = 30.0f;
    public readonly float explosionRadius = 25.0f;
    public float floorY;
    public bool smashing = false;

    public Coroutine powerupCountdown;
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
        else if (currentPowerUp == PowerupType.Smash && Input.GetKeyDown(KeyCode.Space))
        {
            smashing = true;
            StartCoroutine(Smash());
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
            tmpRocket = Instantiate(rocketPrefab, transform.position + Vector3.up, Quaternion.identity);
            tmpRocket.GetComponent<RocketBehaviour>().Fire(enemy.transform);
        }
    }

    IEnumerator Smash()
    {
        var enemies = FindObjectsOfType<Enemy>();
        //Store the y position before taking off
        floorY = transform.position.y;
        //Calculate the amount of time we will go up
        float jumpTime = Time.time + hangTime;
        while (Time.time < jumpTime)
        {
            //move the player up while still keeping their x velocity.
            playerRB.velocity = new Vector2(playerRB.velocity.x, smashSpeed);
            yield return null;
        }
        //Now move the player down
        while (transform.position.y > floorY)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, -smashSpeed * 2);
            yield return null;
        }
        //Cycle through all enemies.
        for (int i = 0; i < enemies.Length; i++)
        {
            //Apply an explosion force that originates from our position.
            if (enemies[i] != null)
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce,
                transform.position, explosionRadius, 0.0f, ForceMode.Impulse);
        }
        //We are no longer smashing, so set the boolean to false
        smashing = false;
    }
}
