
using UnityEditor;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float attackRate = 2f;

    [Header("References")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletParent;
    [SerializeField] private Animator MuzzleFlash;
    private ObjectPooler bulletPooler;


    [Header("Enemy detection")]

    [Tooltip("Maximum distance to detect enemies")]
    [SerializeField,Range(5f,30f)] private float detectRange = 20f;

    [Tooltip("FOV of player for enemy detection")]
    [SerializeField,Range(10f,360f)] private float detectAngle = 30f;
    [SerializeField] private LayerMask detectLayer;

    private bool isEnemyInFOV = false;
    private Transform target = null;


    [SerializeField] GameObject ShootSound;
    private float lastTimeFire = 0f;
    private PlayerControlller controller;
    bool canShoot =false;
  
    float lookAngle;

    private void Awake()
    {
        controller = GetComponent<PlayerControlller>();
        if(bulletParent == null)
        {
            bulletParent = transform;
        }
        if(attackPoint == null)
        {
            attackPoint = transform;
        }
    }


    private void Start()
    {
        if (bulletPrefab != null)
        {
            bulletPooler = ObjectPooler.Instance;
            bulletPooler.InitializePool(bulletPrefab, 20, bulletParent);
        }
        else
        {
            Debug.LogWarning("Player bullet prefab is not assigned ,continue it that is intented");
        }
    }
    private void Update()
    {
        lookAngle = Mathf.Atan2(controller.LookDir.y, controller.LookDir.x) * Mathf.Rad2Deg;

        DetectEnemies();
     
        if (UIController.Instance.isGameStarted && Input.GetMouseButtonDown(0))
        {
            canShoot = true;
        }
        if (ShouldShoot() && canShoot )
        {
            Shoot();
        }
      


    }


    private bool ShouldShoot()
    {
        //Debug.Log("isEnemyInFOV : " + isEnemyInFOV);
        return Time.time >= attackRate + lastTimeFire && isEnemyInFOV && !controller.IsMoving;
    }

    private void DetectEnemies()
    {

        //Debug.Log("Detecting enemies");


        //Checking if we can shoot the new target
        if(target != null)
        {
            //Debug.Log("Target is not null");
            if (!IsObjectInsideRange(target))
            {
                //Debug.Log("Enemy is outside of the range");
                target = null;
            }
        }


        //Debug.Log("After setting target to null");

        //Checking for new target
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, detectRange, detectLayer);

        if (rangeCheck.Length > 0)
        {
            //Debug.Log("Checking inside collider array");
            float closestDistance = float.MaxValue;
            foreach (Collider2D item in rangeCheck)
            {
                Transform currentIten = item.transform;

                //Calculating distance and direction to the current enemy
                float distanceToCurrentItem = Vector2.Distance(transform.position, currentIten.position);

                if (IsObjectInsideRange(currentIten))//Checking if the enemy is in the distance and angle range.
                {
                    //if enemies are inside the range then we search for the closest one.
                    isEnemyInFOV = true;
                    if (distanceToCurrentItem < closestDistance)
                    {
                        closestDistance = distanceToCurrentItem;
                        target = currentIten;
                        //Debug.Log("Found a nearby enemy" + target.name + " and the enemy is in FOV: " + isEnemyInFOV);
                    }
                }


            }
            if(target == null)
            {
                isEnemyInFOV = false;
            }

        }
        else
        {

            isEnemyInFOV = false;
        }
    }


    /// <summary>
    /// Find if the object is in the detection range and vision.
    /// </summary>
    /// <param name="objToDetect"></param>
    /// <returns></returns>
    private bool IsObjectInsideRange(Transform objToDetect)
    {
        Vector2 targetPos = (objToDetect.position - transform.position);
        float distanceToTarget = targetPos.magnitude; 
        Vector2 directionToTarget = targetPos.normalized;
        //Debug.Log("Target is not null");
        return (distanceToTarget <= detectRange && Vector2.Angle(controller.LookDir, directionToTarget) <= detectAngle / 2) ;
    }

    private void Shoot()
    {
        
        //Shooting logic
       // Debug.Log("Shooting");
        if (bulletPrefab != null)
        {
            GameObject go = bulletPooler.GetPooledObject(bulletPrefab);
            if (go != null)
            {
               // Debug.Log("Bullet is taken from the pool");
                go.transform.position = attackPoint.position;
                go.transform.rotation = Quaternion.Euler(0f, 0f, Vector2.Angle(attackPoint.position, target.position));
                //Debug.Log("Before setting bullet component reference");
                if (go.TryGetComponent(out PlayerBullet bullet))
                {
                    MuzzleFlash.SetTrigger("Shoot");
                    bullet.SetTarget(target);
                }
                //Debug.Log("Setting the bullet to true");
               GameObject yes  = Instantiate(ShootSound, transform.position,Quaternion.identity);
                Destroy(yes, 1f);
                go.SetActive(true);
            }
        }

        lastTimeFire = Time.time;
    }


    private void OnValidate()
    {
        if(bulletParent == null)
        {
            bulletParent = transform;
        }    
    }

  /*  private void OnDrawGizmos()
    {
        if (target == null)
            Handles.color = Color.white;
        else
        {
            Handles.color = Color.red;
            Handles.DrawLine(transform.position,target.position);
        }
        
        Handles.DrawWireDisc(transform.position, Vector3.forward, detectRange);

        #region Gadbad wala code
        


        Vector3 viewAngle01 = DirectionFromAngle(*//*transform.eulerAngles.z*//* lookAngle, -detectAngle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(*//*transform.eulerAngles.z*//* lookAngle, detectAngle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(transform.position,transform.position + viewAngle01 * detectRange);
        Handles.DrawLine(transform.position, transform.position + viewAngle02 * detectRange);

        #endregion
    }
    private Vector3 DirectionFromAngle(float eulerZ, float angleInDegrees)
    {
        angleInDegrees += eulerZ;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }*/
}
