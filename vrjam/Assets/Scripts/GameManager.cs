using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int maxTime = 180;
    public int maxMosquitos = 3;

    public ParticleSystem particleSystem;

    public GameObject mosquitoPrefab;
    private GameObject _tutorialMosquito = null;

    private int _aliveMosquitos = 0;
    private int _tutorialPhase = 0;

    // Start is called before the first frame update
    void Start()
    {
        // TODO, spawn tutorialmosquito   
        _tutorialPhase = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (_tutorialPhase == 1 && _tutorialMosquito == null)
        {
            _tutorialPhase = 2;
        }

        if (_tutorialPhase < 2){
            return;
        }
        Debug.Log(_aliveMosquitos);
        if (_aliveMosquitos == 0)
        {
            _aliveMosquitos++;
           SpawnMosquito(); 
        }
        else if (_aliveMosquitos < maxMosquitos) {
            _aliveMosquitos++;
            StartCoroutine(SpawnMosquitoWithDelay(_aliveMosquitos * 5));        }
    }
    
    public void ObjectDestroyed()
    {
        particleSystem.Play();
        _aliveMosquitos -= 1;
    }

    IEnumerator SpawnMosquitoWithDelay(float min)
    {
        Debug.Log("delay!! ");
        yield return new WaitForSeconds(Random.Range(min, min + 5)); // Random interval
        SpawnMosquito();
    }

    private void SpawnMosquito()
    {
        Debug.Log("spawn!!");
        GameObject newObject = Instantiate(mosquitoPrefab, transform.position, Quaternion.Euler(270, 90, 90));
        newObject.GetComponent<ClapTrigger>().spawner = this;
    }
}
