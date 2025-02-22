using UnityEngine;

public class MosquitoMovement : MonoBehaviour
{
    public float SPHERE_RADIUS = 10.0f;  // Movement area size
    public float speed = 1.0f;           // Speed of movement

    private Vector3[] controlPoints = new Vector3[4]; // Bezier curve control points
    private float t = 0; // Bezier curve parameter (0 to 1)

    void Start()
    {
        GenerateNewBezierCurve();
    }

    void Update()
    {
        if (t < 1)
        {
            t += speed * Time.deltaTime; // Increase t based on speed
            transform.position = BezierCurve(t, controlPoints[0], controlPoints[1], controlPoints[2], controlPoints[3]);
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

        // ensure that the distance between the two positions is large enough
        if (Vector3.Distance(position, otherPosition) < 2.0f)
        {
            return false;
        }
        // ensure that the position is outside of the radius of the root position
        if (Vector3.Distance(position, centerPosition) < 1.0f)
        {
            return false;
        }
        // ensure that the position is within the quarter sphere in front of the user
        return (position.magnitude <= SPHERE_RADIUS);
    }

    void GenerateNewBezierCurve()
    {
        Vector3 startPos = transform.position;
        // ensure that this point is within the quarter sphere in front of the player
        Vector3 endPos = GenerateRandomPosition();
        while (!checkValidPosition(startPos, endPos))
        {
            endPos = GenerateRandomPosition();
        }
        
        
        // Control points create a smooth curve
        controlPoints[0] = startPos;
        controlPoints[1] = startPos + Random.insideUnitSphere * SPHERE_RADIUS * 0.5f; // First control point
        controlPoints[2] = endPos + Random.insideUnitSphere * SPHERE_RADIUS * 0.5f; // Second control point
        controlPoints[3] = endPos;
    }

    Vector3 GenerateRandomPosition()
    {
        Vector3 center = Camera.main.transform.position;
        // ensure that the position is within the quarter sphere in front of the user
        while (true)
        {
            float x = Random.Range(-SPHERE_RADIUS, SPHERE_RADIUS);
            float y = Random.Range(-SPHERE_RADIUS, 0);
            float z = Random.Range(0, SPHERE_RADIUS);
            Vector3 position = new Vector3(x, y, z) + center;

            if (position.magnitude <= SPHERE_RADIUS)
            {
                return position;
            }
        }
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