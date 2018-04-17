using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings : MonoBehaviour {


    private char playerNumber;
	// Use this for initialization

	void Start () 
    {
        Debug.Log("player settings " + gameObject.name);
        if (gameObject.name.Contains("player1"))
        {
            playerNumber = '1';
        }
        else if (gameObject.name.Contains("player2"))
        {
            playerNumber = '2';
        }
	}

    public char getPlayerNumber()
    {
        return playerNumber;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
