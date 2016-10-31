using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pickable : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> prefabList = new List<GameObject>();

    GameObject player1;
    GameObject player2;

    Vector3 directionVector;

    void Update()
    {
        if (UnityEngine.Random.Range(0, 2000) != 1) { return; }

        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");

        directionVector = player1.transform.position - player2.transform.position;

        Vector3 ObjectPosition = player1.transform.position + new Vector3(0, 2, 0) + UnityEngine.Random.Range(1, 5) * directionVector.normalized;

        int prefabIndex = UnityEngine.Random.Range(0, 3);
        Instantiate(prefabList[prefabIndex], ObjectPosition, Quaternion.identity);
    }
}
