using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControlller : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust the movement speed as needed
    [SerializeField] private DynamicJoystick joystick;
    [SerializeField] PowerManager PowerManager;
    [SerializeField] private BoxCollider2D boxCollider;

    [SerializeField] private float boxColliderEnablingTime = 1f;
    private Rigidbody2D rb;

    private Vector2 movement;

    private Vector2 lastLookDir;

    //Public properties 
    public bool IsMoving => movement != Vector2.zero;
    
    /// <summary>
    /// The direction in which the player is looking (already normalized)
    /// </summary>
    public Vector2 LookDir
    {
        get;
        private set;
    }

    private Collector scoreCollector;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scoreCollector = GetComponent<Collector>();
      
    }

    private void Update()
    {
        // Input
        movement = joystick.Direction.normalized;
        if (IsMoving)
        {
            LookDir = joystick.Direction;
            lastLookDir = LookDir;
        }
        else
        {
            LookDir = lastLookDir;
        }
       
      
    }

    private void FixedUpdate()
    {
        // Movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

    }

    private void OnDrawGizmos()
    {
       Debug.DrawRay(transform.position,LookDir * 3f,Color.black);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "KillAll":
                Debug.Log("Kill all ability");
                PowerManager.KillAllEnemies();
                Destroy(other.gameObject);
                break;

            case "CollectAll":
                Debug.Log("Collect all ability");
                PowerManager.CollectAll();
                Destroy(other.gameObject);
                break;

            default:
                break;
        }
    }

    private void EnableCollider()
    {
        boxCollider.enabled = true;
    }
}
