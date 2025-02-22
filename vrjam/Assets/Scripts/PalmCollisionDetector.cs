using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalmCollisionDetector : MonoBehaviour
{
    public bool can_clap = true;

    public float clap_cd = 1f;
    public ParticleSystem effectPrefab; // Assign in Inspector
    void OnTriggerEnter(Collider other)
    {
        if (!can_clap){return;}

        if (effectPrefab != null)
        {
            ParticleSystem newEffect = Instantiate(effectPrefab, other.transform.position, Quaternion.identity);
            Debug.Log(other.transform.position);
            Destroy(newEffect.gameObject, newEffect.main.duration);
            can_clap = false;
            StartCoroutine(ResetClap());
        }
    }  


    IEnumerator ResetClap()
    {
        yield return new WaitForSeconds(clap_cd); // Random interval
        can_clap = true;
    }
}

