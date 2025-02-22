using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalmCollisionDetector : MonoBehaviour
{
    public bool clap = false;

    public bool can_clap = true;
    public ParticleSystem effectPrefab; // Assign in Inspector
    void OnTriggerEnter(Collider other)
    {
        clap = true;
        can_clap = false;

        if (effectPrefab != null)
        {
            ParticleSystem newEffect = Instantiate(effectPrefab, other.transform.position, Quaternion.identity);
            Debug.Log(other.transform.position);
            Destroy(newEffect.gameObject, newEffect.main.duration);
        }
    }  
}

