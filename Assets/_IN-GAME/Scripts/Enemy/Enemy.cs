using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField,Tooltip(" damage to player amount ")] float PlayerdamageAmount = 10;
    [SerializeField, Tooltip(" SawBlade Damage ")] private float SawbladeDamageAmount;
    [SerializeField, Tooltip(" ForceField Damage")] private float ForcefieldDamageAmount;
    [SerializeField, Tooltip(" Bomb Damage")] private int BombDamgeAmount;

    public float circleRadius = 2f; // Adjust the radius of the overlapping circle
    public LayerMask playerLayer;
    public float damageDuration=1 ; // The duration over which the damage is applied



    private float damageTimer = 0.0f; // Timer to track the damage duration
    private bool isDamaging = false;
    private float currentSpeed;
    private Transform playerTransform;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isPlayerInRange = false;
    private Vector3 direction;
    private PlayerEntity PlayerEnity;
    private EnemyEntity enemyEntity;


    private ObjectPooler enemyPooler;


    private CollectableSpawner collectableSpawner;



    private void Start()
    {
        currentSpeed = moveSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        enemyPooler = ObjectPooler.Instance;
    }
    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        PlayerEnity = playerTransform.GetComponent<PlayerEntity>();

        //inetrnal references
        enemyEntity = transform.GetComponent<EnemyEntity>();
        if (enemyEntity == null)
        {
            Debug.Log("Enemy enetity is null");
        }


        collectableSpawner = FindObjectOfType<CollectableSpawner>();
    }


    private void Update()
    {
        MoveTowardsPlayer(); // move
        Flip();
        DetectPlayerAndAttack();//detecting and attacking player 
        DoDamage(); // to player
        TakeDamage();//taking damage based on layer
     //   Debug.Log(enemyEntity.CurrentHealth);

       

    }

    // this takdeDamge is depeand upon the layer discribe the layer in if and Do Damage
    private void TakeDamage()
    {

        Collider2D[] detectCol = Physics2D.OverlapCircleAll(transform.position, circleRadius);

        if (detectCol.Length > 0)
        {
           
            foreach (Collider2D collider in detectCol)
            {
                int collidedLayer = collider.gameObject.layer;
                string layerName = LayerMask.LayerToName(collidedLayer);

                if (layerName == "SawBloody")
                {
                    // Take damage or perform actions specific to the obstacle
                   
                    enemyEntity.TakeDamage(SawbladeDamageAmount);
                   
                }
                else if (layerName == "ForceField")
                {
                    // Take damage or perform actions specific to the sawblade


                    enemyEntity.TakeDamage(ForcefieldDamageAmount);
                }
                else
                {
                    //  Debug.Log("Collided with layer: " + layerName);

                }

               

            }
        }
/*
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1, targetLayer);

        if (colliders.Length > 0)
        {

          
        }*/
    }
  

    private void Flip()
    {
        if (direction.x < 0f)
        {
            spriteRenderer.flipX = true;
        }
        else if (direction.x > 0f)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void DetectPlayerAndAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, circleRadius,playerLayer);
        bool isPlayerDetected = colliders.Length > 0;
       
        if (isPlayerDetected && !isPlayerInRange)
        {
            // Player entered the attack range
            isPlayerInRange = true;
            currentSpeed = 0;
           
            // Play attack animation
            animator.SetBool("CanAttack", true);


      
          
        }
        else if (!isPlayerDetected && isPlayerInRange)
        {
            // Player exited the attack range
            isPlayerInRange = false;
            currentSpeed = moveSpeed;
         
           
            animator.SetBool("CanAttack", false);
            isDamaging = false;
            damageTimer = 0.0f;

        }


    }


    private void MoveTowardsPlayer()
    {
        direction = playerTransform.position - transform.position;
        float distanceX = direction.x;
        direction.y += 0.5f; // Adjust the Y-component of the direction

        direction.Normalize();
      
       // Debug.Log(playerTransform.position.x);
        transform.Translate(direction * currentSpeed * Time.deltaTime);
      

       

    }
    private void DoDamage()
    {
        if (isPlayerInRange)
        {
            if (!isDamaging)
            {
                // Start applying damage
                damageTimer = 0.0f;
                isDamaging = true;
            }
            else
            {
                // Continue applying damage gradually
                damageTimer += Time.deltaTime;
                if (damageTimer >= damageDuration)
                {
                    // Damage duration has passed, stop applying damagez
                    isDamaging = false;
                    damageTimer = 0.0f;
                }
                else
                {
                    // Apply a fraction of the total damage over time
                    float damageFraction = Time.deltaTime / damageDuration;
                    float damageAmountPerFrame = PlayerdamageAmount * damageFraction;
                    PlayerEnity.TakeDamage(damageAmountPerFrame);
                }
            }
        }
        else
        {
            // Player is out of range, stop applying damage
            isDamaging = false;
            damageTimer = 0.0f;
        }

       // Debug.Log(playerTransform.GetComponent<PlayerEntity>().CurrentHealth);
    }
    private void OnDrawGizmosSelected()
    {
        // Visualize the overlapping circle in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, circleRadius);
    }

    private void OnEnable()
    {
        EnemySpawner.EnemySpawned++;
        enemyEntity.OnDie += OnEnemyDie;
    }

    private void OnDisable()
    {
        enemyEntity.OnDie -= OnEnemyDie;
        EnemySpawner.EnemySpawned--;
    }



    private void OnEnemyDie()
    {
        
        UIController.Instance.Addkill();
        //Choosing which collectable to spawn
        int collectableIndex = Random.Range(0,collectableSpawner.MaxNumberOfPrefabs);

        //Debug.Log("Spawned by: " + transform.name);
        collectableSpawner.SpawnCollectable(collectableIndex,transform.position,Quaternion.identity);
        //Debug.Log(EnemyDeathCount);
        enemyPooler.ReturnToPool(gameObject);
    
 
     }
}
