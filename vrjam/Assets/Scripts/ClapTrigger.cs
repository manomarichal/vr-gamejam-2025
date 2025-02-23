using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClapTrigger : MonoBehaviour
{
    public GameManager spawner;
    public ParticleSystem bloodExplosion;
    public AudioClip[] audioClips; // This is the exported variable
    private AudioSource audioSource;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
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
        // Instantiate the blood effect
        ParticleSystem newEffect = Instantiate(bloodExplosion, other.transform.position, Quaternion.identity);
        Destroy(newEffect.gameObject, newEffect.main.duration);

        // Play sound from a temporary audio object
        PlaySoundAtPosition(audioClips[Random.Range(0, audioClips.Length)], other.transform.position);

        // Destroy this object
        Destroy(this.gameObject);

        // Increment the kill count in the GameManager
        spawner.IncrementKillCount();
    }

    void PlaySoundAtPosition(AudioClip clip, Vector3 position)
    {
        GameObject audioObj = new GameObject("TempAudio");
        audioObj.transform.position = position; // Set position for 3D sound

        AudioSource tempSource = audioObj.AddComponent<AudioSource>();
        tempSource.clip = clip;
        tempSource.spatialBlend = 1.0f; // 3D effect
        tempSource.loop = false;
        tempSource.playOnAwake = false;
        tempSource.Play();

        Destroy(audioObj, clip.length); // Destroy after sound finishes
    }

}