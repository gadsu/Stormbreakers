using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerScript : MonoBehaviour {

    CharacterChoices cc;

    int time;
    int firstDigit;
    int frames;
    public Image TimerLeft;
    public Image TimerRight;
    public Sprite[] sprites = new Sprite[10];
    public bool gameover;
     
	// Use this for initialization
	void Start () 
    {
        cc = GameObject.Find("CharacterChoices").GetComponent<CharacterChoices>();
        time = 99;
        gameover = false;
    }
	
    public void displayGameOver()
    {
        
    }

    public void endGame(int player)
    {
        if (SceneManager.GetActiveScene().name == "TrainingMode")
        {
            if (player == 1)
            {
                GameObject.Find("P2Text").GetComponent<Text>().text = "DEAD";

            }
            else if (player == 2)
            {
                GameObject.Find("P1Text").GetComponent<Text>().text = "DEAD";
            }

            Debug.Log("If");

        }
        else if((cc.getScore(1) == 1 && player == 2) || (cc.getScore(2) == 1 && player == 1) || (cc.getScore(1) == 1 || cc.getScore(2) == 1 && player == 0))
        {
            if (player == 1)
            {
                GameObject.Find("GameOverAnimationText").GetComponent<Text>().text = "Player 2 Wins!";
                cc.p2Score--;
            }
            else if (player == 2)
            {
                GameObject.Find("GameOverAnimationText").GetComponent<Text>().text = "Player 1 Wins!";
                cc.p1Score--;
            }
            else if (player == 0)
            {
                if (cc.getScore(1) == 1 && cc.getScore(2) == 1)
                {
                    GameObject.Find("GameOverAnimationText").GetComponent<Text>().text = "Draw"; 
                    cc.p1Score--;
                    cc.p2Score--;
                }
                else if (cc.getScore(1) == 1)
                {
                    GameObject.Find("GameOverAnimationText").GetComponent<Text>().text = "Player 1 Wins!"; 
                    cc.p1Score--;
                    cc.p2Score--;

                }
                else if (cc.getScore(2) == 1)
                {
                    GameObject.Find("GameOverAnimationText").GetComponent<Text>().text = "Player 2 Wins!"; 
                    cc.p1Score--;
                    cc.p2Score--;
                }

            }

            cc.resetScore();

            Debug.Log("Else If");

            GameObject.Find("GameOverAnimationText").GetComponent<Animator>().Play("FadeInText");
        }
        else
        {
            int total = cc.getScore(1) + cc.getScore(2);
            if (player == 2)
            {
                GameObject.Find("GameOverAnimationText").GetComponent<Text>().text = "Round " + (total + 1) + ": Player 1"; 
                cc.winScore(1);
            }
            else if (player == 1)
            {
                GameObject.Find("GameOverAnimationText").GetComponent<Text>().text = "Round " + (total + 1) + ": Player 2"; 
                cc.winScore(2);
            }
            else if (player == 0)
            {
                GameObject.Find("GameOverAnimationText").GetComponent<Text>().text = "Round " + (total + 1) + ": Draw";
                cc.winScore(1);
                cc.winScore(2);
            }

            Debug.Log("Else");

            GameObject.Find("GameOverAnimationText").GetComponent<Animator>().Play("EndRoundFade");
        }

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
            gameover = true;

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
                endGame(2);
            }
            else if (p1.GetComponent<CharacterState>().healthBar.value < p2.GetComponent<CharacterState>().healthBar.value)
            {
                endGame(1);
            }
            else
            {
                GameObject.Find("GameOverAnimationText").GetComponent<Text>().text = "DRAW!";
                endGame(0);
            }

            //GameObject.Find("GameOverAnimationText").GetComponent<Animator>().Play("FadeInText");


        }
    }
}
