using System;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    [Header("Damage parameters")]
    [Tooltip("Amount of damage to give to the enemy")]
    [SerializeField] private float damageAmout = 10f;

    [Header("Bullet")]
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletLifetime = 10f;


    private ObjectPooler bulletPooler;
    private Transform target;


    public Action OnBulletReturnToPool;

    private void Update()
    {
        if(target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;

            transform.position = Vector2.MoveTowards(transform.position,target.position,bulletSpeed * Time.deltaTime);


            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        }
    }

    private void OnEnable()
    {
        if (bulletPooler == null)
        {
            bulletPooler = ObjectPooler.Instance;
        }
        Invoke(nameof(Die), bulletLifetime);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Enemy")
        {
            //Taking the reference of enemy health component
            var healthComponent = other.transform.GetComponent<EnemyEntity>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage((int)damageAmout);
            }


            Die();
        }
    }

    private void Die()
    {
        //Debug.Log("Deactivating the bullet");
        target = null;
        if (gameObject.activeInHierarchy)
        {
            //Debug.Log("Disabing them if they are enabled");
            bulletPooler.ReturnToPool(this.gameObject, OnBulletReturnToPool);
        }
    }

}
