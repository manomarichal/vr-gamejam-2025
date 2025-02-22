using UnityEngine;

public class DestroyHandler : MonoBehaviour
{
    public GameManager spawner;

    void Start()
    {
        Destroy(gameObject, 30f);
    }

    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.ObjectDestroyed();
        }
    }
}
