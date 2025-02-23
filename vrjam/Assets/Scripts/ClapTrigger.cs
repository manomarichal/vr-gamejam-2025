using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClapTrigger : MonoBehaviour
{
    public GameManager spawner;

    public ParticleSystem bloodExplosion;

    void OnTriggerEnter(Collider other)
    {
        ParticleSystem newEffect = Instantiate(bloodExplosion, other.transform.position, Quaternion.identity);
        Destroy(newEffect.gameObject, newEffect.main.duration);
        Destroy(this.gameObject);
    }
}
