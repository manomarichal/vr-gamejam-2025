using UnityEngine;

public class MosquitoMovement : MonoBehaviour
{
    public float SPHERE_RADIUS = 10.0f;  // Movement area size
    public float speed = 1.0f;           // Speed of movement

    private Vector3[] controlPoints = new Vector3[4]; // Bezier curve control points
    private float t = 0; // Bezier curve parameter (0 to 1)
    private Vector3 lastPosition; // Store last position for rotation calculation

    private AudioSource audioSource;
    public AudioClip mosquitoBuzz; // Assign this in the Inspector
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

        // Assign the mosquito sound
        if (mosquitoBuzz == null)
        {
            mosquitoBuzz = Resources.Load<AudioClip>("flying-mosquito-105770"); // Ensure file is in Resources folder
        }

        if (mosquitoBuzz != null)
        {
            audioSource.clip = mosquitoBuzz;
            audioSource.loop = true;
            audioSource.playOnAwake = true;
            audioSource.spatialBlend = 1.0f; // 3D effect
            audioSource.Play();
        }
        else
        {
            Debug.LogError("Mosquito sound file not found!");
        }
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

    bool checkValidPosition(Vector3 position, Vector3 otherPosition)
    {
        Vector3 centerPosition = Camera.main.transform.position;

        if (Vector3.Distance(position, otherPosition) < 2.0f)
            return false;

        if (Vector3.Distance(position, centerPosition) < 1.0f)
            return false;

        return (position.magnitude <= SPHERE_RADIUS);
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
        float x = Random.Range(-SPHERE_RADIUS, SPHERE_RADIUS);
        float y = Random.Range(-SPHERE_RADIUS, 0);
        float z = Random.Range(0, SPHERE_RADIUS);
        Vector3 position = new Vector3(x, y, z) + center;
        return position;
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