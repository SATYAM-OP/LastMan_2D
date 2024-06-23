using System;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public static Action<float> OnCollect;
    private float currentScore = 0;



    private void Start()
    {
        //Debug.Log("Invoking the on collect");
        currentScore = 0;
        OnCollect?.Invoke(currentScore);    
    }

    public void CollectItem(Collectable itemToCollect)
    {
        SoundManager.instance.Play("gem");

        if (itemToCollect.CanBeCollected)
        {
            Debug.Log("Item can be collected");
            currentScore += itemToCollect.CollectScore;
            //Debug.Log("Before calling event : " + itemToCollect.name);
            OnCollect?.Invoke(itemToCollect.CollectScore);
            itemToCollect.Collect();
        }
        else
        {
            Debug.Log("Cant be collected");
        }
    }

}
