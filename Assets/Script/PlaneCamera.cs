using UnityEngine;
using System.Collections;

public class PlaneCamera : MonoBehaviour
{
    private Transform player2Target = null;
    private Transform player1Target = null;

    [SerializeField]
    private float zoomFactor;
    [SerializeField]
    private float zoomOffset;
    [SerializeField]
    private float heightOffset;

    void Start()
    {
    }

    void LateUpdate()
    {
        if (player1Target == null)
            player1Target = GameObject.FindGameObjectWithTag(TagsTypeString.Player1.ToString()).transform;
        if (player2Target == null)
            player2Target = GameObject.FindGameObjectWithTag(TagsTypeString.Player2.ToString()).transform;
        if (player1Target == null || player2Target == null) { return; }
        var midPoint = (player2Target.position + player1Target.position) / 2;
        var lineVector = player2Target.position - player1Target.position;
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
