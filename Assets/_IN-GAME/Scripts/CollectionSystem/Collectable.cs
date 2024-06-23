using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private float collectScore = 10;

    private ObjectPooler pooler;
    public bool CanBeCollected
    {
        get;
        private set;
    }


    private void OnEnable()
    {
        CanBeCollected = true;
    }

    private void Start()
    {
        pooler = ObjectPooler.Instance;
    }
    public float CollectScore
    {
        get { return collectScore; }
        private set { collectScore = value; }
    }

    public void Collect()
    {
        //Debug.Log("Item Collected");
        pooler.ReturnToPool(gameObject);
    }

    private void OnDisable()
    {
        CanBeCollected = false;
        //Debug.Log(name + " disabled");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Collectable collided with player : " + gameObject.name);
            var collector = other.GetComponent<Collector>();
            collector.CollectItem(this);
        }
    }
}
