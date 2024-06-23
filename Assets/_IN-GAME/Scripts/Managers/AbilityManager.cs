using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public enum AbilityType
{
    ForceField,
    SpeedBoost,
    RotatingBlade,
    Bomb,
}
[System.Serializable]
public class Ability
{
    public AbilityType AbilityType;
    public int Currentlevel;
    public GameObject[] LevelsGameObject;
}
public class AbilityManager : MonoBehaviour
{
    public static AbilityManager instance;

    private void Awake()
    {
        instance = this;
    }

    public PlayerControlller playerControlller;
    public GameObject BombPrefab;
    public List<Ability> abilities;
    public float SpeedUpgradeValue=0.5f;
    public card cards;
    public List<card> cardList;
    public int abilityCount = 0;
    private Ability CurrentAblity;
    private Transform[] AllChilds_RotatingBlade;
    private Transform[] AllChilds_BombAblity;


    public void Start()
    {
        CardSpwaner();
    }
    public void CardSpwaner()
    {
        for(int i=0; i < cardList.Count; i++)
        {
            Destroy(cardList[i].gameObject);
        }
        cardList.Clear();
        List<int> numbers = new List<int> { 0, 1, 2, 3};
        for(int i =0;i<abilities.Count;i++)
        {
            if (abilities[i].Currentlevel >= 3)
            {
                numbers.Remove(i);
            }
        }
        List<int> randomOrder = new List<int>();
        while (numbers.Count > 0)
        {
            int index = Random.Range(0, numbers.Count);
            randomOrder.Add(numbers[index]);
            numbers.RemoveAt(index);
        }

        for(int i=0;i< 3 && i<randomOrder.Count;i++)
        {
            if (abilities[randomOrder[i]].Currentlevel < 3)
            {
                GameObject newCard =  Instantiate(cards.gameObject, cards.transform.parent);
                newCard.GetComponent<card>().SetCardInfo(randomOrder[i], abilities[randomOrder[i]].Currentlevel);
                newCard.SetActive(true);
                cardList.Add(newCard.GetComponent<card>());
            }
        }
    }

    public void CardClicked(card card)
    {
        SoundManager.instance.Play("ability select");
        //Debug.Log((AbilityType)card.id);
        ActivateAbility((AbilityType)card.id);
        skills.instance.Setability(card.id);
        CardSpwaner();
        UIController.Instance.CloseAbilityPanel();
        abilityCount++;
    }
   
    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && abilityCount < 12)
        {
             UIController.Instance.OpenAbilityPanel();
        }
    
    }*/
    public void ActivateAbility(AbilityType _abilityType)
    {
        AssingCurrentAblity(_abilityType);
        if (_abilityType == AbilityType.SpeedBoost)
        {
            ActiveSpeedBoost(CurrentAblity.Currentlevel);
            CurrentAblity.Currentlevel++;
            return;
        }
        CheckCurrentAblityLevel();
        CurrentAblity.Currentlevel++;
    }

    private void CheckCurrentAblityLevel()
    {
        switch (CurrentAblity.Currentlevel)
        {
            case 0:
                SetCurrentLevelAblityActive(0);
                break;
            case 1:
                SetCurrentLevelAblityActive(1);
                break;
            case 2:
                SetCurrentLevelAblityActive(2);
                break;
            default:
                break;
        }
    }

    public void SetCurrentLevelAblityActive(int AblityLevel)
    {
        for (int i = 0; i < CurrentAblity.LevelsGameObject.Length; i++)
        {
            if (i == AblityLevel)
            {
                CurrentAblity.LevelsGameObject[i].SetActive(true);
                StartAblity(i);
                break;
            }
            else
            {
                CurrentAblity.LevelsGameObject[i].SetActive(false);
            }
        }
    }

    private void StartAblity(int i)
    {
        switch (CurrentAblity.AbilityType)
        {
            case AbilityType.RotatingBlade:
                StopCoroutine(nameof(RotatingBlade));
                RotatingBladeActivated(CurrentAblity.LevelsGameObject[i]);
                break;
            case AbilityType.SpeedBoost:
                break;
            case AbilityType.Bomb:
                StopCoroutine(nameof(SpawnBombs));
                BombAblityActivated(CurrentAblity.LevelsGameObject[i]);
                break;
            case AbilityType.ForceField:
                SoundManager.instance.Play("forcefield");
                break;
            default:
                break;
        }
    }

    private void AssingCurrentAblity(AbilityType abilityType)
    {
        foreach (Ability item in abilities)
        {
            if (item.AbilityType == abilityType)
            {
                CurrentAblity = item;
                break;
            }
        }
    }
    void ActiveSpeedBoost(int Currentlevel)
    {
        if (Currentlevel < 3)
        {
            playerControlller.moveSpeed += SpeedUpgradeValue;
        }
    }
   
    public void RotatingBladeActivated(GameObject LevelParent)
    {
        AllChilds_RotatingBlade = LevelParent.GetComponentsInChildren<Transform>();
        StartCoroutine(nameof(RotatingBlade));
    }
    IEnumerator RotatingBlade()
    {
        for (int i = 1; i < AllChilds_RotatingBlade.Length; i++)//Starting from 1 because 0 index is parent
        {
            AllChilds_RotatingBlade[i].DOScale(0, 0f);
            AllChilds_RotatingBlade[i].DOScale(1.75f, 1.5f);

        }
        SoundManager.instance.Play("spinner");
        yield return new WaitForSeconds(4f);
        for (int i = 1; i < AllChilds_RotatingBlade.Length; i++)
        {
            AllChilds_RotatingBlade[i].DOScale(0f, 1.5f).SetEase(Ease.OutBack);
        }
      
        yield return new WaitForSeconds(4f);
        StartCoroutine(nameof(RotatingBlade));
    }
    public void BombAblityActivated(GameObject LevelParent)
    {
        AllChilds_BombAblity = LevelParent.GetComponentsInChildren<Transform>();
        StartCoroutine(nameof(SpawnBombs));
    }
    IEnumerator SpawnBombs()
    {
        for (int i = 1; i < AllChilds_BombAblity.Length; i++)//Starting from 1 because 0 index is parent
        {
            GameObject Bomb=Instantiate(BombPrefab,playerControlller.transform.position, Quaternion.identity, null);
            
            Bomb.transform.DOMove(AllChilds_BombAblity[i].transform.position, 1f).OnComplete(() =>
            {
                Bomb.GetComponent<Rotation>().enabled = false;
                Bomb.GetComponent<Animator>().enabled = true;
                if (Bomb != null)
                {
                    Destroy(Bomb, 1f);
                }
            });
        }
        SoundManager.instance.Play("bomb");
        yield return new WaitForSeconds(3f);
        StartCoroutine(nameof(SpawnBombs));
    }

   

}
