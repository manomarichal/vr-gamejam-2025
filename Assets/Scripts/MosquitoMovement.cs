using UnityEngine;

public class MosquitoMovement : MonoBehaviour
{
    public float SPHERE_RADIUS = 10.0f;  // Movement area size
    public float speed = 1.0f;           // Speed of movement

    private Vector3[] controlPoints = new Vector3[4]; // Bezier curve control points
    private float t = 0; // Bezier curve parameter (0 to 1)
    private Vector3 lastPosition; // Store last position for rotation calculation

    void Start()
    {
        GenerateNewBezierCurve();
        lastPosition = transform.position;
    }

    void Update()
    {
        
    }

    void FixedUpdate() {
        if (t < 1)
        {
            t += speed * Time.deltaTime; // Increase t based on speed
            Vector3 newPosition = BezierCurve(t, controlPoints[0], controlPoints[1], controlPoints[2], controlPoints[3]);

            // // Calculate direction vector
            // Vector3 direction = (newPosition - lastPosition).normalized;
            // if (direction != Vector3.zero) // Ensure we don't rotate to NaN
            // {
            //     transform.rotation = Quaternion.LookRotation(direction);
            // }

            transform.position = newPosition;
            lastPosition = newPosition; // Update last position
        }
        else
        {
            GenerateNewBezierCurve();
            t = 0;
        }
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
        // use angles instead of ranges, it will be better in equal distribution of mosquito positions
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
