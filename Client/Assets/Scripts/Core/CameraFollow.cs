using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float smoothSpeed = 0.125f;

    private void LateUpdate()
    {
        if (target == null) return;

        // We only care about following Z (forward) and maybe X (lane change)
        // Let's keep Y (height) somewhat steady or relative
        Vector3 desiredPosition = target.position + offset;
        
        // Optional: Lock X if you want a straight "hallway" view, 
        // but following X feels more dynamic for lane switching.
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // transform.LookAt(target); // Optional, usually better to keep fixed rotation for runners
    }
}
