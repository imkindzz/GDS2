using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwapManager : MonoBehaviour
{
    public Transform player1;
    public Transform player2;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 temp = player1.position;
            player1.position = player2.position;
            player2.position = temp;
        }
    }
}
