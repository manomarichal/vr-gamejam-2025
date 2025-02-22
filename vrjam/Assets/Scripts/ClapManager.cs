using UnityEngine;
using Wave.Native;
using Wave.Essence.Hand;

public class ClapDetector : MonoBehaviour
{
    public Transform leftHandT;
    public Transform rightHandT;

    public PalmCollisionDetector pcd;

    public float clapThreshold = 0.1f; // Distance in meters for a clap
    public float speedThreshold = 1.5f; // Minimum speed of hands coming together
    private Vector3 prevLeftPos, prevRightPos;
    private bool handsTracked = false;

    void Update()
    {
        
    }

    void OnClap()
    {
        // Implement your clap event logic here (e.g., play sound, trigger animation)
    }
}       