using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class card : MonoBehaviour
{
    [SerializeField] private string[] titles;
    [SerializeField] private Sprite[] icons;
    [SerializeField] private string[] descriptions;

    public int id;
    public TMP_Text title;
    public Image icon;
    public TMP_Text description;
    public GameObject[] stars;
    public int level;

    public void SetCardInfo(int i, int level)
    {
        id = i;
        this.level = level;
        title.text = titles[i];
        icon.sprite = icons[i];
        description.text = descriptions[i];
        setStars();
    }

    public void setStars()
    {
        for(int i =0; i < level; i++)
        {
            stars[i].SetActive(true);
        }
    }

    public void Click()
    {
        AbilityManager.instance.CardClicked(this);
    }
}
