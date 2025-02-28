using UnityEngine;
using System.Collections;

public class MosquitoMovement : MonoBehaviour
{
    public float SPHERE_RADIUS = 10.0f;  // Movement area size
    public float speed = 1.0f;           // Speed of movement
    public bool tutorialMode = false;   // Ensures new points are in view
    public float handReachRadius = 0.5f; // Max distance from player for tutorial mode

    public AudioClip[] audioClips; // This is the exported variable

    private Vector3[] controlPoints = new Vector3[4]; // Bezier curve control points
    private float t = 0; // Bezier curve parameter (0 to 1)
    private Vector3 lastPosition; // Store last position for rotation calculation

    private AudioSource audioSource;
    public float maxVolume = 1.0f;  // Maximum volume when closest to the camera
    public float minVolume = 0.05f;  // Minimum volume when farthest from the camera

    void Start()
    {
        GenerateNewBezierCurve();
        lastPosition = transform.position;

        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (audioClips.Length > 0) // Ensure the array is not empty
        {
            AudioClip randomClip = audioClips[Random.Range(0, audioClips.Length)]; // Select a random clip
            audioSource.clip = randomClip;
            audioSource.loop = true;
            audioSource.playOnAwake = false; // Start with volume 0 before playing
            audioSource.spatialBlend = 1.0f; // 3D effect
            audioSource.volume = 0f; // Start silent
            audioSource.Play();
            
            StartCoroutine(FadeInAudio(audioSource, 1f)); // Fade in over 0.5 seconds
        }

        else
        {
            Debug.LogError("Mosquito sound file not found!");
        }
    }
    IEnumerator FadeInAudio(AudioSource source, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            source.volume = Mathf.Lerp(0f, 1f, timer / duration); // Gradually increase volume
            yield return null;
        }
        source.volume = 1f; // Ensure volume is fully set at the end
    }

    void FixedUpdate()
    {
        if (t < 1)
        {
            t += speed * Time.deltaTime; // Increase t based on speed
            Vector3 newPosition = BezierCurve(t, controlPoints[0], controlPoints[1], controlPoints[2], controlPoints[3]);

            transform.position = newPosition;
            lastPosition = newPosition; // Update last position

            AdjustSoundVolume(); // Adjust volume based on distance
        }
        else
        {
            GenerateNewBezierCurve();
            t = 0;
        }
    }

    void AdjustSoundVolume()
    {
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        float normalizedDistance = Mathf.Clamp01(distance / SPHERE_RADIUS); // Normalize between 0 and 1
        audioSource.volume = Mathf.Lerp(maxVolume, minVolume, normalizedDistance);
    }

    void GenerateNewBezierCurve()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = GenerateRandomPosition();

        controlPoints[0] = startPos;
        controlPoints[1] = startPos + Random.insideUnitSphere * SPHERE_RADIUS * 0.5f;
        controlPoints[2] = endPos + Random.insideUnitSphere * SPHERE_RADIUS * 0.5f;
        controlPoints[3] = endPos;
    }

    Vector3 GenerateRandomPosition()
    {
        Vector3 center = Camera.main.transform.position;
        Vector3 position;
        
        if (tutorialMode)
        {
            // Ensure position is within camera view and within hand reach
            position = GetPositionInViewAndReach();
        }
        else
        {
            // random position in sphere of radius sphere radius
            position = Random.insideUnitSphere * SPHERE_RADIUS + center;
        }
        return position;
    }

    Vector3 GetPositionInViewAndReach()
    {
        Camera cam = Camera.main;
        Vector3 position;
        int attempts = 10;
        do
        {
            Vector3 viewportPoint = new Vector3(Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f), Random.Range(0.5f, handReachRadius));
            position = cam.ViewportToWorldPoint(viewportPoint);
            attempts--;
        } while (!checkValidPosition(position, transform.position) && attempts > 0);

        return position;
    }

    bool checkValidPosition(Vector3 position, Vector3 otherPosition)
    {
        Vector3 centerPosition = Camera.main.transform.position;
        if (Vector3.Distance(position, otherPosition) < 2.0f || Vector3.Distance(position, centerPosition) < 1.0f)
        {
            return false;
        }
        return position.magnitude <= SPHERE_RADIUS;
    }

    Vector3 BezierCurve(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float uu = u * u;
        float uuu = uu * u;
        float tt = t * t;
        float ttt = tt * t;

        return (uuu * p0) + (3 * uu * t * p1) + (3 * u * tt * p2) + (ttt * p3);
    }
}