using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClapTrigger : MonoBehaviour
{
    public GameManager spawner;

    void OnTriggerEnter(Collider other)
    {
        if (spawner != null)
        {
            spawner.ObjectDestroyed();
        }
        Destroy(this.gameObject);
    }
}
