using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClapTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
    }
}
