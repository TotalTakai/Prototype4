using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3;
    private Rigidbody enemyRB;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        enemyRB = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (player.transform.position - enemyRB.transform.position).normalized;
        enemyRB.AddForce(lookDirection * speed);

        if (gameObject.transform.position.y < -10) Destroy(gameObject); // destorys enemies thrown off the field
    }
}
