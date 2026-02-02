using UnityEngine;

public class TrackSegment : MonoBehaviour
{
    // Start of the segment relative to its transform position
    public Transform beginPoint;
    // End of the segment, used for connecting the next one
    public Transform endPoint;

    private void OnDrawGizmos()
    {
        if (beginPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(beginPoint.position, 0.5f);
        }
        if (endPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(endPoint.position, 0.5f);
        }
    }
}
