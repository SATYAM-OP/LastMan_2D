using System.Collections;
using UnityEngine;
using DG.Tweening;
public enum Power
{
    KillAll,
    CollectAll

}
[System.Serializable]
public class PowerClass
{
    public Power Power;
    public GameObject _PowerPrefab;
    public Sprite _afterDrop;
}
public class PowerManager : MonoBehaviour
{
    [Header("Kill-All")]
    public PowerClass _KillAll;
    [Header("Collect-All")]
    public PowerClass _CollectAll;
    [Header("SpawnValues")]
    public int SpawnRangeMinX;
    public int SpawnRangeMaxX;
    public int SpawnRangeMinY;
    public int SpawnRangeMaxY;
    [Header("Values")]
    public float waitTimeToVansish;


    [SerializeField] private float killPowerDuration;
    [SerializeField] private float collectPowerDuration;

    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask collectableLayer;


    [SerializeField] private float abilitySpawnTime;
    private ParticleSystem skullParticle;

    private GameObject playerObject;
    private Collector itemCollector;


    [Header("Area detection")]
    [SerializeField] private Transform bottomLeftPoint;
    [SerializeField] private Transform topRightPoint;


    private void Awake()
    {
        skullParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {

        playerObject = GameObject.FindGameObjectWithTag("Player");
        itemCollector = playerObject.GetComponent<Collector>();

        InvokeRepeating(nameof(SpawnPowerRandomly), abilitySpawnTime, abilitySpawnTime);
    }
    public void SpawnPower(PowerClass powerClass)
    {
        GameObject ActivePower = Instantiate(powerClass._PowerPrefab, new Vector2(GetRandomInt(SpawnRangeMinX, SpawnRangeMaxX), 9), Quaternion.identity, null);
        ActivePower.transform.DOMoveY(GetRandomInt(SpawnRangeMinY, SpawnRangeMaxY), 3f).OnComplete(() =>
        {
            ActivePower.GetComponent<SpriteRenderer>().sprite = powerClass._afterDrop;
            StartCoroutine(VanishPower(ActivePower.GetComponent<SpriteRenderer>()));
        });
    }

    private void SpawnPowerRandomly()
    {
        int powerIndex = Random.Range(0, 2);
        switch (powerIndex)
        {
            case 0:
                SpawnPower(_CollectAll);
                break;
            case 1:
                SpawnPower(_KillAll);
                break;
            default:
                break;
        }
    }
    public int GetRandomInt(int Min, int Max)
    {
        return Random.Range(Min, Max);
    }
    IEnumerator VanishPower(SpriteRenderer spriteRenderer)
    {

        yield return new WaitForSeconds(waitTimeToVansish);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(spriteRenderer.DOFade(0.5f, 0.75f));
        sequence.Append(spriteRenderer.DOFade(1f, 0.75f));
        sequence.Append(spriteRenderer.DOFade(0.5f, 0.75f));
        sequence.Append(spriteRenderer.DOFade(1f, 0.75f));
        sequence.Append(spriteRenderer.DOFade(0.5f, 1.5f).OnComplete(() =>
        {
            Destroy(spriteRenderer.gameObject);

        }));
    }
    public void KillAllEnemies()
    {

        //Debug.Log("Killing all");

        Collider2D[] colliders = DetectObject(enemyLayer);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out EnemyEntity enemyEntity))
            {
                enemyEntity.TakeDamage(enemyEntity.CurrentHealth);
            }
        }
        Debug.Log("Killed all enemies");

    }
    public void CollectAll()
    {
        Collider2D[] colliders = DetectObject(collectableLayer);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Collectable collectable))
            {
                collectable.transform.DOMove(playerObject.transform.position, .5f).SetEase(Ease.InElastic).OnComplete(() =>
                {
                    itemCollector.CollectItem(collectable);

                });


            }
        }
    }

     private Collider2D[] DetectObject(LayerMask layer)
     {
            Collider2D[] colliders = Physics2D.OverlapAreaAll(bottomLeftPoint.position, topRightPoint.position, layer);
            return colliders;
      }

    
}
