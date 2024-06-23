using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float rotationSpeed = 250f;

    private void Update()
    {
        // Get the current rotation of the object
        Quaternion currentRotation = transform.rotation;

        // Calculate the desired rotation amount
        float rotationAmount = rotationSpeed * Time.deltaTime;

        // Create a new rotation with the desired Z-axis rotation
        Quaternion newRotation = Quaternion.Euler(0f, 0f, currentRotation.eulerAngles.z + rotationAmount);

        // Apply the new rotation to the object
        transform.rotation = newRotation;
    }
}
