using System;
using System.Collections;
using UnityEngine;

public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    [Tooltip("Health at which this object should have in the start")]
    [SerializeField] private float startHealth = 100f;
    [SerializeField] private GameObject floatingText;
    private float currentHealth;


    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = value;
            currentHealth = Mathf.Clamp(value, 0, maxHealth);

        }
    }

    private bool shoulDie => CurrentHealth == 0;




    public Action<float> OnTakeDamage;
    public Action<float> OnHeal;
    public Action<float> OnMaxHealthIncrease;
    public Action OnDie;

    private void Awake()
    {
        startHealth = Mathf.Clamp(startHealth, 0, maxHealth);
        CurrentHealth = startHealth;
    }
    private void ShowFloatingText(float damageamount)
    {
        //can use  object pooling 
        var go = Instantiate(floatingText, transform.position, Quaternion.identity, transform.parent);
       
        go.GetComponent<TextMesh>().text =damageamount.ToString();


    }


 
    public void TakeDamage(float damageAmt)
    {
        CurrentHealth -= damageAmt;
        OnTakeDamage?.Invoke(currentHealth);
         ShowFloatingText(1);
       
        //Debug.Log("Current health of enemy is: " + CurrentHealth);
        if (shoulDie)
        {
            Die();
        }



    }
    
    public void Heal(float healAmt)
    {

    }

    public void IncraseMaxxHealth()
    {

    }

    private void Die()
    {
        //things that can be done before dying

        OnDie?.Invoke();
    }

    private void OnEnable()
    {
        CurrentHealth = startHealth;
    }
}
