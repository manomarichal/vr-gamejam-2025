using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public int maxTime = 20; // Max game time in seconds
    public int maxMosquitos = 3;
    public AudioClip[] audioClips; // This is the exported variable

    public ParticleSystem particleSystem;
    public GameObject mosquitoPrefab;
    public TextMeshProUGUI scoreText; // Reference to the TextMeshPro component
    public TextMeshProUGUI clockText;
    public AudioClip introAudio; // The audio clip to play
    public AudioClip introAudio2; // The audio clip to play

    public AudioSource wekker;

    public PalmCollisionDetector palmCollision;
    public TextMeshProUGUI gameOverText;

    private GameObject _tutorialMosquito = null;
    private int _aliveMosquitos = 0;
    private int _tutorialPhase = 0;

    private int killCount = 0; // Static variable to keep track of the kill count
    private float gameTime = 0; // Current game time in seconds

    private static int startHour = 22;
    // private static int startMinute = 0;
    private static int endHour = 7;
    // private static int endMinute = 0;
    private static int totalGameMinutes = 9 * 60; // 9 hours

    // Start is called before the first frame update
    void Start()
    {
        gameTime = 0;
        // clockText.text = startHour.ToString("00") + ":" + startMinute.ToString("00");
        scoreText.text = "Clap your hands to start the game!";
        gameOverText.text = "";
        _tutorialPhase = -2;
        clockText.text = "22:00";
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += (float)Time.deltaTime;

        if (_tutorialPhase == -2 && palmCollision.clapped == true){
            scoreText.text = "";
            _tutorialPhase = -1;
            StartCoroutine(SetTutorialPhase0());
        }
        if (_tutorialPhase == 0)
        {
             scoreText.text = "Clap to kill as many mosquitoes as you can during the night!";
            _tutorialMosquito = SpawnMosquito();
            _tutorialMosquito.GetComponent<MosquitoMovement>().tutorialMode = true;  
            _tutorialPhase = 1;

            // Create a new GameObject dynamically
            GameObject audioObject = new GameObject("IntroAudioSource");
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();

            // Configure the AudioSource
            audioSource.clip = introAudio2;
            audioSource.playOnAwake = false; // Ensures it doesn't auto-play before setup
            audioSource.Play();

            // Destroy the GameObject after the audio clip finishes playing
            Destroy(audioObject, introAudio2.length);
        }

        if (_tutorialPhase == 1 && _tutorialMosquito == null)
        {
            scoreText.text = "Kills: " + killCount;
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

        if (_tutorialPhase == 3)
        {
            // No tutorial anymore
            UpdateClock();

            if (gameTime >= maxTime)
            {
                PlayClockSound();
                SetEndView();
                StartCoroutine(SetTutorialPhaseAfterDelay(1f)); // Start a coroutine to delay the phase change
            }
        }

        if (_tutorialPhase == 4 && palmCollision.clapped == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            _tutorialPhase = 5;
        }

    }

    private IEnumerator SetTutorialPhase0()
    {
        // Create a new GameObject dynamically
        GameObject audioObject = new GameObject("IntroAudioSource");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        // Configure the AudioSource
        audioSource.clip = introAudio;
        audioSource.playOnAwake = false; // Ensures it doesn't auto-play before setup
        audioSource.Play();

        // Destroy the GameObject after the audio clip finishes playing
        Destroy(audioObject, introAudio.length);

        yield return new WaitForSeconds(6);
        _tutorialPhase = 0; // Set the tutorial phase after the delay
    }

    // Coroutine to delay the tutorial phase change
    private IEnumerator SetTutorialPhaseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _tutorialPhase = 4; // Set the tutorial phase after the delay
    }

    void PlayClockSound() {
        // TODO: Play alarm beep sound (followed by our Mosquito song)
    }

    void SetEndView () {
        foreach (MosquitoMovement mosquito in FindObjectsOfType<MosquitoMovement>())
        {
            Destroy(mosquito.gameObject);
        }
        wekker.Play();
        gameOverText.text = "Time to wake up!";
        scoreText.text = "\nMosquitos killed: " + killCount + "\nClap to restart the game";

    }

    void UpdateClock() {
        float gameProgress = gameTime / maxTime;
        // update currentHour based on gameProgress, startHour and endHour. However, note that startHour is e.g. 22 and endHour is 7, so you need to handle the case where the game goes past midnight.
        int endHourAdjusted = endHour + (startHour > endHour ? 24 : 0);

        int currentHour = startHour + (int)(gameProgress * (endHourAdjusted - startHour));
        // int currentMinute = 0;

        if (currentHour >= endHour && currentHour < startHour) {
            clockText.text = endHour.ToString("00") + ":00";
            return;
        }

        // get the minutes
        int totalGameMinutes = (endHourAdjusted - startHour) * 60;
        int currentMinute = (int)(gameProgress * totalGameMinutes) % 60;
        // Round to 5 minutes
        currentMinute = (currentMinute / 15) * 15;


        if (currentHour > 23) {
            currentHour -= 24;
        }

        
        clockText.text = currentHour.ToString("00") + ":" + currentMinute.ToString("00");
    }
    
    void FixedUpdate()
    {
        _aliveMosquitos = Object.FindObjectsOfType<MosquitoMovement>().Length;

        if (_aliveMosquitos == 0 && _tutorialPhase == 3){
            SpawnMosquito();
        }

        if (_tutorialPhase == 5) {
            foreach (MosquitoMovement mosquito in FindObjectsOfType<MosquitoMovement>())
            {
                Destroy(mosquito.gameObject);
            }
        }
    }
    
    IEnumerator SpawnMosquitoWithDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3, 6)); // Random interval'

            if (gameTime >= maxTime){
                break;
            }

            if (_aliveMosquitos < maxMosquitos) {
                SpawnMosquito();
            }
        }
    }

    private GameObject SpawnMosquito()
    {
        GameObject newObject = Instantiate(mosquitoPrefab, transform.position, Quaternion.Euler(0, 180, 0));
        _tutorialMosquito = null;
        return newObject;
    }

    public void IncrementKillCount()
    {
        killCount++;
        scoreText.text = "Kills: " + killCount;
    }
}