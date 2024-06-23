using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class skills : MonoBehaviour
{
    public static skills instance;

    private void Awake()
    {
        instance = this;
    }
    [SerializeField] Sprite[] icons;
    [SerializeField] Image[] images;
    public List<int> abilities;
    private int count;

    public void Setability(int i)
    {
        if(abilities.Contains(i))
        {
            return;
        }
        if (images.Length > count)
        {
            images[count].gameObject.SetActive(true);
            images[count].sprite = icons[i];
            abilities.Add(i);
            count++;
        }
    }
}
