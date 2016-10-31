using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> AI, Player1, Player2;
    //[SerializeField]
    //Transform Location1,Location2;
    // Use this for initialization
    int num1, num2;
    void Start()
    {
        var script = FindObjectOfType<GameInfo>();
        var player1 = Instantiate(Player1[script.P1Character]);
        GameObject player2;
        if (script.AIMode)
        {
            player2 = Instantiate(AI[script.P2Character]);
        }
        else
        {
            player2 = Instantiate(Player2[script.P2Character]);
        }
        player1.transform.position = new Vector3(3, 5, 0);
        player2.transform.position = new Vector3(-3, 5, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
