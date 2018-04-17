using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeTextGameOver : MonoBehaviour {

    public void gameOverWrapper()
    {
        GameObject.Find("Timer").GetComponent<PauseMenu>().gameOver();
        GameObject.Find("Timer").GetComponent<TimerScript>().gameover = true;
    }

    public void showMenu(){
        GameObject.Find("Timer").GetComponent<PauseMenu>().showGameOverMenu();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
