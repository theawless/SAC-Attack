using UnityEngine;
using System.Collections;

public class PlaneCamera : MonoBehaviour
{
    [SerializeField]
    private Transform enemyTarget;
    [SerializeField]
    private Transform playerTarget;
    [SerializeField]
    private float zoomFactor;
    [SerializeField]
    private float zoomOffset;
    [SerializeField]
    private float heightOffset;

    void LateUpdate()
    {
        var midPoint = (enemyTarget.position + playerTarget.position) / 2;
        var lineVector = enemyTarget.position - playerTarget.position;
        float distance = lineVector.magnitude;
        var perpendicularToLine = Vector3.Cross(lineVector, Vector3.up);
        perpendicularToLine = perpendicularToLine * zoomFactor;
        if (perpendicularToLine.magnitude < zoomOffset)
        {
            perpendicularToLine.Normalize();
            perpendicularToLine = perpendicularToLine * zoomOffset;
        }
        var newPosition = midPoint - perpendicularToLine;
        newPosition.y = newPosition.y + heightOffset;
        transform.position = newPosition;
        transform.rotation = Quaternion.LookRotation(perpendicularToLine);
    }
}
