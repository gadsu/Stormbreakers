using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    public GameObject pauseMenu;
    public GameObject GameOverMenu;
    public Text tx;
    private EventSystem es;
    private int pausedplayer;
    public bool paused = false;
    private bool press = false;
    public Button[] list = new Button[5];
    private int index;
    private bool gameover = false;
    private AudioClip confirmSound;
    private AudioClip denySound;

	// Use this for initialization
	void Start () {
        confirmSound = Resources.Load("Sounds/Confirm") as AudioClip;
        denySound = Resources.Load("Sounds/Deny V2") as AudioClip;
	}
	
    void pause () {
        
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0;
        pauseMenu.SetActive(true);
        tx.text = "PLAYER " + pausedplayer;
        paused = true;

        if (GameObject.Find("player1R"))
        {
            GameObject.Find("player1R").GetComponent<Animator>().enabled = false;
            GameObject.Find("player1R").GetComponent<InputGatherer>().enabled = false;
        }
        else if (GameObject.Find("player1L"))
        {
            GameObject.Find("player1L").GetComponent<Animator>().enabled = false;
            GameObject.Find("player1L").GetComponent<InputGatherer>().enabled = false;
        }

        if (GameObject.Find("player2L"))
        {
            GameObject.Find("player2L").GetComponent<Animator>().enabled = false;
            GameObject.Find("player2L").GetComponent<InputGatherer>().enabled = false;
        }
        else if (GameObject.Find("player2R"))
        {
            GameObject.Find("player2R").GetComponent<Animator>().enabled = false;
            GameObject.Find("player2R").GetComponent<InputGatherer>().enabled = false;
        }
    }

    public void resume () {
        if (pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
        }
        else if (GameOverMenu.activeSelf)
        {
            GameOverMenu.SetActive(false);
            gameover = false;
        }
        paused = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        pausedplayer = 0;
        list[index].image.color = Color.white;

        if (GameObject.Find("player1R"))
        {
            GameObject.Find("player1R").GetComponent<Animator>().enabled = true;
            GameObject.Find("player1R").GetComponent<InputGatherer>().enabled = true;
        }
        else if (GameObject.Find("player1L"))
        {
            GameObject.Find("player1L").GetComponent<Animator>().enabled = true;
            GameObject.Find("player1L").GetComponent<InputGatherer>().enabled = true;
        }

        if (GameObject.Find("player2L"))
        {
            GameObject.Find("player2L").GetComponent<Animator>().enabled = true;
            GameObject.Find("player2L").GetComponent<InputGatherer>().enabled = true;
        }
        else if (GameObject.Find("player2R"))
        {
            GameObject.Find("player2R").GetComponent<Animator>().enabled = true;
            GameObject.Find("player2R").GetComponent<InputGatherer>().enabled = true;
        }
    }

    public void gameOver() 
    {
        paused = true;

        if (GameObject.Find("player1R"))
        {
          //  GameObject.Find("player1R").GetComponent<Animator>().enabled = false;
            GameObject.Find("player1R").GetComponent<CommandInterpreter>().clearInputQueue();
            GameObject.Find("player1R").GetComponent<InputGatherer>().enabled = false;
            GameObject.Find("player1R").GetComponent<CommandInterpreter>().enabled = false;
        }
        else if (GameObject.Find("player1L"))
        {
       //     GameObject.Find("player1L").GetComponent<Animator>().enabled = false;
            GameObject.Find("player1L").GetComponent<CommandInterpreter>().clearInputQueue();
            GameObject.Find("player1L").GetComponent<InputGatherer>().enabled = false;
            GameObject.Find("player1L").GetComponent<CommandInterpreter>().enabled = false;
        }

        if (GameObject.Find("player2L"))
        {
 //           GameObject.Find("player2L").GetComponent<Animator>().enabled = false;
            GameObject.Find("player2L").GetComponent<CommandInterpreter>().clearInputQueue();
            GameObject.Find("player2L").GetComponent<InputGatherer>().enabled = false;
            GameObject.Find("player2L").GetComponent<CommandInterpreter>().enabled = false;
        }
        else if (GameObject.Find("player2R"))
        {
 //           GameObject.Find("player2R").GetComponent<Animator>().enabled = false;
            GameObject.Find("player2R").GetComponent<CommandInterpreter>().clearInputQueue();
            GameObject.Find("player2R").GetComponent<InputGatherer>().enabled = false;
            GameObject.Find("player2R").GetComponent<CommandInterpreter>().enabled = false;
        }

        Time.timeScale = 0.25f;
    }

    public void showGameOverMenu(){
        GameOverMenu.SetActive(true);
        GameObject.Find("GameOverAnimationText").SetActive(false);

        if (GameObject.Find("Player1Slider").GetComponent<Slider>().value > GameObject.Find("Player2Slider").GetComponent<Slider>().value)
        {
            GameObject.Find("WinText").GetComponent<Text>().text = "PLAYER 1 WINS";
        }
        else if (GameObject.Find("Player1Slider").GetComponent<Slider>().value < GameObject.Find("Player2Slider").GetComponent<Slider>().value)
        {
            GameObject.Find("WinText").GetComponent<Text>().text = "PLAYER 2 WINS";
        }
        else
        {
            GameObject.Find("WinText").GetComponent<Text>().text = "--- TIE ---";
        }
        index = 3;
        list[index].image.color = Color.blue;
        pausedplayer = 1;
        press = true;
        gameover = true;
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0;
    }

	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("P1S") && !paused)
        {
            pausedplayer = 1;
            pause();
            list[0].image.color = Color.blue;
            index = 0;
            press = true;
        }
        else if (Input.GetButtonDown("P2S") && !paused)
        {
            pausedplayer = 2;
            pause();
            list[0].image.color = Color.blue;
            index = 0;
            press = true;
        }


        //navigation based on who paused
        if (paused) 
        {
            if (Input.GetButtonDown("P1S") && pausedplayer == 1 && !press && !gameover)
            {
                resume();
                AudioSource.PlayClipAtPoint(confirmSound, GameObject.Find("Camera").transform.position);
            }
            else if (Input.GetButtonDown("P2S") && pausedplayer == 2 && !press && !gameover)
            {
                resume();
                AudioSource.PlayClipAtPoint(confirmSound, GameObject.Find("Camera").transform.position);
            }
            else if ((Input.GetButtonDown("P1E") && pausedplayer == 1) || (Input.GetButtonDown("P2E") && pausedplayer == 2))
            {
                list[index].Select();
                list[index].onClick.Invoke();
                press = true;
            }

            //gameover
            if(gameover)
            {
                if ((Input.GetAxis("P1Vertical") > 0 || Input.GetAxis("P1DPadVertical") < 0)) //Down
                {
                    list[index].image.color = Color.white;
                    index = 4;
                    list[index].image.color = Color.blue;
                    press = true;
                }
                else if ((Input.GetAxis("P1Vertical") < 0 || Input.GetAxis("P1DPadVertical") > 0)) //Up
                {
                    list[index].image.color = Color.white;
                    index = 3;
                    list[index].image.color = Color.blue;
                    press = true;
                }
            }
            //Player1 selecting
            else if (pausedplayer == 1)
            {
//                if (index >= 3 && !press)
//                {
//                    if ((Input.GetAxis("P1Vertical") > 0 || Input.GetAxis("P1DPadVertical") < 0)) //Down
//                    {
//                        list[index].image.color = Color.white;
//                        index = 4;
//                        list[index].image.color = Color.blue;
//                        press = true;
//                    }
//                    else if ((Input.GetAxis("P1Vertical") < 0 || Input.GetAxis("P1DPadVertical") > 0)) //Up
//                    {
//                        list[index].image.color = Color.white;
//                        index = 3;
//                        list[index].image.color = Color.blue;
//                        press = true;
//                    }
//                }
                if ((Input.GetAxis("P1Vertical") > 0 || Input.GetAxis("P1DPadVertical") < 0) && (index < 2) && !press) //Down
                {
                    list[index].image.color = Color.white;
                    index++;
                    list[index].image.color = Color.blue;
                    press = true;
                }
                else if ((Input.GetAxis("P1Vertical") < 0 || Input.GetAxis("P1DPadVertical") > 0) && (index > 0) && !press) //Up
                {
                    list[index].image.color = Color.white;
                    index--;
                    list[index].image.color = Color.blue;
                    press = true;
                }
                else if ((Input.GetAxis("P1Vertical") == 0 && Input.GetAxis("P1DPadVertical") == 0) && press && Input.GetAxis("P1Horizontal") == 0 && Input.GetAxis("P1DPadHorizontal") == 0)
                {
                    press = false;
                }
            }


            //Player2 selecting
            if (pausedplayer == 2)
            {
                if ((Input.GetAxis("P2Vertical") > 0 || Input.GetAxis("P2DPadVertical") < 0) && (index != 2) && !press) //Down
                {
                    list[index].image.color = Color.white;
                    index++;
                    list[index].image.color = Color.blue;
                    press = true;
                }
                else if ((Input.GetAxis("P2Vertical") < 0 || Input.GetAxis("P2DPadVertical") > 0) && (index != 0) && !press) //Up
                {
                    list[index].image.color = Color.white;
                    index--;
                    list[index].image.color = Color.blue;
                    press = true;
                }
                else if ((Input.GetAxis("P2Vertical") == 0 && Input.GetAxis("P2DPadVertical") == 0) && press && Input.GetAxis("P2Horizontal") == 0 && Input.GetAxis("P2DPadHorizontal") == 0)
                {
                    press = false;
                }
            }
        }


	}
}
