using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {

    private CharacterState cs;

    int time;
    int firstDigit;
    int frames;
    public Image TimerLeft;
    public Image TimerRight;
    public Sprite[] sprites = new Sprite[10];
    public bool gameover;
     
	// Use this for initialization
	void Start () {
        time = 99;
        gameover = false;
	}
	
    public void displayGameOver()
    {
        
    }

    // Update is called once per frame
    void Update () {

        if (Time.frameCount - frames >= 60 && time > 0 && Time.timeScale != 0 && !gameover)
        {
            frames = Time.frameCount;
            time--;
            TimerLeft.sprite = sprites[((time - time % 10) / 10)];
            TimerRight.sprite = sprites[(time % 10)];
            if (time <= 10)
            {
                GetComponent<Animator>().Play("TimeBlink");
            }
        }
        if (time <= 0 && !gameover)
        {
            GameObject p1, p2;
            if (GameObject.Find("player1L"))
            {
                p1 = GameObject.Find("player1L");
            }
            else
            {
                p1 = GameObject.Find("player1R");
            }

            if (GameObject.Find("player2L"))
            {
                p2 = GameObject.Find("player2L");
            }
            else
            {
                p2 = GameObject.Find("player2R");
            }
                
            if (p1.GetComponent<CharacterState>().healthBar.value > p2.GetComponent<CharacterState>().healthBar.value)
            {
                GameObject.Find("GameOverAnimationText").GetComponent<Text>().text = "Player 1 Wins!";
            }
            else if (p1.GetComponent<CharacterState>().healthBar.value < p2.GetComponent<CharacterState>().healthBar.value)
            {
                GameObject.Find("GameOverAnimationText").GetComponent<Text>().text = "Player 2 Wins!";
            }
            else
            {
                GameObject.Find("GameOverAnimationText").GetComponent<Text>().text = "DRAW!";
            }

            GameObject.Find("GameOverAnimationText").GetComponent<Animator>().Play("FadeInText");
            gameover = true;
        }
    }
}
