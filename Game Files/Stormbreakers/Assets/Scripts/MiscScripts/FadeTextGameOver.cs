using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeTextGameOver : MonoBehaviour {

    public void gameOverWrapper()
    {
        GameObject.Find("Timer").GetComponent<PauseMenu>().gameOver();
        GameObject.Find("Timer").GetComponent<TimerScript>().gameover = true;
    }

    public void showMenu(){
        GameObject.Find("Timer").GetComponent<PauseMenu>().showGameOverMenu();
    }

    public void restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Stage1");
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
