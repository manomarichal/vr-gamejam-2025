using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClapTrigger : MonoBehaviour
{
    public GameManager spawner;
    public ParticleSystem bloodExplosion;

    void Start()
    {
        if (spawner == null)
        {
            spawner = FindObjectOfType<GameManager>();
            if (spawner == null)
            {
                Debug.LogError("GameManager not found in the scene.");
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        ParticleSystem newEffect = Instantiate(bloodExplosion, other.transform.position, Quaternion.identity);
        Destroy(newEffect.gameObject, newEffect.main.duration);
        Destroy(this.gameObject);
        
        // Increment the kill count in the GameManager
        spawner.IncrementKillCount();
    }
}