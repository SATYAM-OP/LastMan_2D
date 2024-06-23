using UnityEngine;

public abstract class SingletonPersistant<T> : Singleton<T> where T : MonoBehaviour
{

    /// <summary>
    /// This will instalize singletan instance with dont destroy on load.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}