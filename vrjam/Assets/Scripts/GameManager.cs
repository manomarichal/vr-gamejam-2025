using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int maxTime = 180;
    public int maxMosquitos = 3;
    public AudioClip[] audioClips; // This is the exported variable

    public ParticleSystem particleSystem;
    public GameObject mosquitoPrefab;
    public TextMeshProUGUI scoreText; // Reference to the TextMeshPro component
    public TextMeshProUGUI clockText;

    private GameObject _tutorialMosquito = null;
    private int _aliveMosquitos = 0;
    private int _tutorialPhase = 0;

    private static int killCount = 0; // Static variable to keep track of the kill count

    // Start is called before the first frame update
    void Start()
    {
        _tutorialMosquito = SpawnMosquito();
        _tutorialMosquito.GetComponent<MosquitoMovement>().tutorialMode = true;
        _tutorialPhase = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (_tutorialPhase == 1 && _tutorialMosquito == null)
        {
            _tutorialPhase = 2;
        }

        if (_tutorialPhase == 2){
            SpawnMosquito();
            StartCoroutine(SpawnMosquitoWithDelay());
            _tutorialPhase = 3;
        }
    }

    void FixedUpdate()
    {
        _aliveMosquitos = Object.FindObjectsOfType<MosquitoMovement>().Length;
    }

    IEnumerator SpawnMosquitoWithDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(7, 15)); // Random interval'
            if (_aliveMosquitos < maxMosquitos) {
                SpawnMosquito();
            }
        }
    }

    private GameObject SpawnMosquito()
    {
        GameObject newObject = Instantiate(mosquitoPrefab, transform.position, Quaternion.Euler(270, 90, 90));
        _tutorialMosquito = null;
        return newObject;
    }

    public void IncrementKillCount()
    {
        killCount++;
        scoreText.text = "Kills: " + killCount;
    }
}