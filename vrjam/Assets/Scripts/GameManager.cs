using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int maxTime = 20; // Max game time in seconds
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
    private static float gameTime = 0; // Current game time in seconds

    private static int startHour = 22;
    // private static int startMinute = 0;
    private static int endHour = 7;
    // private static int endMinute = 0;
    private static int totalGameMinutes = 9 * 60; // 9 hours

    // Start is called before the first frame update
    void Start()
    {
        _tutorialMosquito = SpawnMosquito();
        _tutorialMosquito.GetComponent<MosquitoMovement>().tutorialMode = true;
        _tutorialPhase = 1;
        gameTime = 0;
        // clockText.text = startHour.ToString("00") + ":" + startMinute.ToString("00");
        clockText.text = "21:00";
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += (float)Time.deltaTime;

        if (_tutorialPhase == 1 && _tutorialMosquito == null)
        {
            _tutorialPhase = 2;
            gameTime = 0;
        }

        if (_tutorialPhase == 2){
            // Tutorial just finished, start normal gameplay
            SpawnMosquito();
            StartCoroutine(SpawnMosquitoWithDelay());
            _tutorialPhase = 3;
            gameTime = 0;
        }

        if (_tutorialPhase == 3) {
            // No tutorial anymore
            UpdateClock();

            if (gameTime >= maxTime) {
                PlayClockSound();
                SetEndView(); 
                _tutorialPhase = 4;
            }
        }
    }

    void PlayClockSound() {
        // TODO: Play alarm beep sound (followed by our Mosquito song)
    }

    void SetEndView () {
        // TODO: Show "Clap to restart game"
        // Keep alarm view flickering on/off
        // Show credit scene
    }

    void UpdateClock() {
        float gameProgress = gameTime / maxTime;
        // update currentHour based on gameProgress, startHour and endHour. However, note that startHour is e.g. 22 and endHour is 7, so you need to handle the case where the game goes past midnight.
        int endHourAdjusted = endHour + (startHour > endHour ? 24 : 0);

        int currentHour = startHour + (int)(gameProgress * (endHourAdjusted - startHour));
        int currentMinute = 0;

        if (currentHour > 23) {
            currentHour -= 24;
        }

        clockText.text = currentHour.ToString("00") + ":" + currentMinute.ToString("00");
    }

    void FixedUpdate()
    {
        _aliveMosquitos = Object.FindObjectsOfType<MosquitoMovement>().Length;
    }

    IEnumerator SpawnMosquitoWithDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(4, 10)); // Random interval'
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