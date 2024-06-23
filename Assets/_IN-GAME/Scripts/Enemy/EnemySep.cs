using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySep : MonoBehaviour
{
    public float separationRadius = 2f; // Radius for separation check
    public LayerMask objectLayer; // The layer containing the objects to separate

    public float separationForce = 10f; // Force to apply for separation
    public float separationDistance = 1f; // Minimum separation distance

    private void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, separationRadius, objectLayer);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject) // Exclude self from separation
            {
                Vector2 separationDirection = transform.position - collider.transform.position;
                float separationMagnitude = separationDirection.magnitude;

                if (separationMagnitude < separationDistance)
                {
                    // Calculate separation force
                    float separationStrength = separationForce * (1f - separationMagnitude / separationDistance);
                    Vector2 separationForceVector = separationDirection.normalized * separationStrength;

                    // Apply separation force
                    transform.Translate(separationForceVector * Time.deltaTime);
                }
            }
        }
    }
}
