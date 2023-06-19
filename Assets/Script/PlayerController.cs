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

    public GameObject powerupIndicator;
    
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
            hasPowerup = true;
            powerupIndicator.gameObject.SetActive(true);
            StartCoroutine("PowerupCountdownRoutine"); //Works on a different thread
        }
        else if (other.CompareTag("PowerupRockets"))
        {

        }
    }

    // Uses a different thread to wait 7 seconds to deactivate the powerup
    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    // Sends enemy away if the player have the powerup
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRB = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            enemyRB.AddForce(awayFromPlayer * powerupStrenght, ForceMode.Impulse);

        }
    }
}
