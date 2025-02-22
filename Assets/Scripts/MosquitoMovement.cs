using UnityEngine;

public class MosquitoMovement : MonoBehaviour
{
    public float SPHERE_RADIUS = 10.0f;  // Movement area size
    public float speed = 3.0f;           // Speed of movement

    private Vector3 cur_position;
    private Vector3 new_position;

    void Start()
    {
        cur_position = transform.position;
        new_position = generate_random_position();
    }

    void Update()
    {
        // Move towards new position
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, new_position, step);

        // Check if we reached the destination
        if (Vector3.Distance(transform.position, new_position) < 0.2f)
        {
            new_position = generate_random_position();
        }
    }

    bool is_inside_sphere(Vector3 position)
    {
        return (position.magnitude <= SPHERE_RADIUS);
    }

    Vector3 generate_random_position()
    {
        Vector3 center = Camera.main.transform.position;  // Make it relative to camera
        while (true)
        {
            float x = Random.Range(-SPHERE_RADIUS, SPHERE_RADIUS);
            float y = Random.Range(-SPHERE_RADIUS, SPHERE_RADIUS);
            float z = Random.Range(-SPHERE_RADIUS, SPHERE_RADIUS);
            Vector3 position = new Vector3(x, y, z) + center;  // Shift relative to center

            if (is_inside_sphere(position))
            {
                return position;
            }
        }
    }
}