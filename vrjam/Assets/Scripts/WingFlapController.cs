using UnityEngine;

public class WingFlapController : MonoBehaviour
{
    public Transform leftWing;  // Assign in the Inspector
    public Transform rightWing; // Assign in the Inspector
    public float flapSpeed = 13;  // Speed of the flap (higher = faster)
    public float flapAmount = 30f; // Max rotation angle of the wings

    private float time;

void Start()
    {
        // Get all child transforms of the object this script is attached to
        Transform[] children = GetComponentsInChildren<Transform>();

        // Find the left and right wing by checking the names (or any other criteria you want)
        foreach (Transform child in children)
        {
            if (child.name.Contains("LeftWing"))  // Or use other identifiers
                leftWing = child;
            if (child.name.Contains("RightWing")) // Or use other identifiers
                rightWing = child;
        }

        // If wings are not found, log an error
        if (leftWing == null || rightWing == null)
        {
            Debug.LogError("LeftWing and/or RightWing not found in children!");
        }
    }

    void Update()
    {
        // Create a flap motion based on sine wave to simulate the flapping
        time += Time.deltaTime * flapSpeed;

        // Rotate wings left and right using sine wave on the y-axis
        float flapRotation = Mathf.Sin(time) * flapAmount;

        // Rotate around the y-axis, adjusting the left and right wings in opposite directions
        leftWing.localRotation = Quaternion.Euler(-90, -flapRotation,  0);  // Rotate around y-axis
        rightWing.localRotation = Quaternion.Euler(-90, flapRotation,  0); // Rotate around y-axis

        // Rotate the wings around the y-axis
        //leftWing.Rotate(-90, 0, 0);  // Rotate around y-axis
        //rightWing.Rotate(-90, 0, 0); // Rotate around y-axis

    }

}
