using System;
using UnityEngine;
using UnityEngine.UI;
public class PlayerEntity : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    [Tooltip("Health at which this object should have in the start (Don't assign value bigger than max health)")]
    [SerializeField] private float startHealth = 100f;

    private float currentHealth;
    public Image healthFill;

    public float CurrentHealth
    {
        get { return currentHealth; }
        set { 
            currentHealth = value;
            currentHealth = Mathf.Clamp(value,0,maxHealth); 
        
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



    public void TakeDamage(float damageAmt)
    {
        CurrentHealth -= damageAmt;
        OnTakeDamage?.Invoke(currentHealth);
        float fillAmount = currentHealth / maxHealth;
        healthFill.fillAmount = fillAmount;
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
        UIController.Instance.gameOver();
        gameObject.SetActive(false);
        OnDie?.Invoke();
    }


}
