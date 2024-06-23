using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ExpBar : MonoBehaviour
{
    [SerializeField] private float fillTime = 1f;

    [SerializeField] private Image FillImage;
    [SerializeField] private TMP_Text LevelText;
    [SerializeField] private float maxXp;

    [Tooltip("max xp should increase by how much percentage each time")]
    [SerializeField] private float percentIncrease;


    [SerializeField] private int startLevel;

    int currentLvl = 0;
    float currentXp;

    private float CurrentXp
    {
        get { return currentXp; }
        set { currentXp = Mathf.Clamp(value,0,maxXp); }
    }


    private void Awake()
    {
        currentLvl = startLevel;
    }

    private void OnEnable()
    {
        Collector.OnCollect += OnGemsCollectedChanged;
        //Debug.Log("On enable event");
    }

    private void OnDisable()
    {
        Collector.OnCollect -= OnGemsCollectedChanged;
    }



   
   
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fillAmt">Should be b/w 0 to 1</param>
    private void UpdateExpFill(float fillAmt)
    {
        if (fillAmt == 0)
        {
            FillImage.fillAmount = 0;
        }
        else
        {
            DOTween.To(() => FillImage.fillAmount, x => FillImage.fillAmount = x, fillAmt, fillTime).SetUpdate(true);
            FillImage.fillAmount = fillAmt;
        }
        LevelText.text = currentLvl.ToString();
        
       
    }


    private void Update()
    {
        if(CurrentXp == 0)
        {
            CurrentXp = 0;
            UpdateExpFill(0);
        }
    }

    /// <summary>
    /// Will subscribe to the events which want to change the xp
    /// </summary>
    /// <param name="newXp">xp that will be added to the current xp</param>
    public void OnGemsCollectedChanged(float newXp)
    {
        CurrentXp += newXp;
        //Debug.Log(currentXp);
       // Debug.Log("Current xp : " + currentXp);

        float clampedXp = currentXp / maxXp;
        //Debug.Log("Clamped xp is: " + clampedXp);

        UpdateExpFill(clampedXp);
        if (CurrentXp >= maxXp)
        {
            //Debug.Log("xp is full");
            //After selecting ability from ability panel.
            UIController.Instance.OpenAbilityPanel();
            //Debug.Log("After Ability panel");

            maxXp = IncreaseByPercentage(maxXp, percentIncrease);
            //Debug.Log("After increasing max xp : " + maxXp);
            CurrentXp = 0;
            currentLvl++;
            UpdateExpFill(0);
        }
    }


    private float IncreaseByPercentage(float value, float percentaeIncrease)
    {
        float increasedValue = (value * percentaeIncrease) / 100;
        float newValue = value + increasedValue;
        return newValue;

    }

}
