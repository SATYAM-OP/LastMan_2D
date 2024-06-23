using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public LayerMask targetLayer;
    [SerializeField] float DamgeAmount=100;


    private void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1, targetLayer);

        if (colliders.Length > 0)
        {

            StartCoroutine(DetectLayerAfterDelay());
        }
    }

    private System.Collections.IEnumerator DetectLayerAfterDelay()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second

    
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, 1, targetLayer))
        {
            GameObject collidedObject = collider.gameObject;
          
            collidedObject.gameObject.GetComponent<EnemyEntity>().TakeDamage((int)DamgeAmount);
            Debug.Log("Collision detected!");
        }
    }
    private void OnDrawGizmosSelected()
    {
        // Visualize the overlapping circle in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1);
    }



}
