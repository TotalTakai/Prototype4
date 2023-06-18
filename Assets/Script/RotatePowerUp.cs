using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePowerUp : MonoBehaviour
{
    private readonly float rotationSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up * rotationSpeed);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
