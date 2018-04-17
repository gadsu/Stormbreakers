using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrainingPauseMenu : MonoBehaviour {

	public GameObject pauseMenu;
	public GameObject GameOverMenu;
	public Text tx;
	private EventSystem es;
	private int pausedplayer;
	public bool paused = false;
	private bool press = false;
	public Button[] list = new Button[5];
	private int index;
	//private bool p1dead = false;
	//private bool p2dead = false;
	private Text p2txt;
	private Text p1txt;
	private AudioClip confirmSound;
	private AudioClip denySound;
    private GameObject player1;
    private GameObject player2;

    private int p1HealDelay;
    private int p2HealDelay;
    private int healDelayTime;



	// Use this for initialization
	void Start () {
		confirmSound = Resources.Load("Sounds/Confirm") as AudioClip;
		denySound = Resources.Load("Sounds/Deny V2") as AudioClip;

		p1txt = GameObject.Find ("P1Text").GetComponent<Text>();
		p2txt = GameObject.Find ("P2Text").GetComponent<Text>();
		pauseMenu = GameObject.Find ("PauseMenu");
		tx = GameObject.Find ("PausedText").GetComponent<Text> ();
		pauseMenu.SetActive (false);
		p1txt.text = "  ";
		p2txt.text = "  ";

        if (GameObject.Find("player1R"))
        {
            player1 = GameObject.Find("player1R");
        }
        else if (GameObject.Find("player1L"))
        {
            player1 = GameObject.Find("player1L");
        }

        if (GameObject.Find("player2R"))
        {
            player2 = GameObject.Find("player2R");
        }
        else if (GameObject.Find("player2L"))
        {
            player2 = GameObject.Find("player2L");
        }

        p1HealDelay = int.MaxValue;
        p2HealDelay = int.MaxValue;
        healDelayTime = 120;

        player1.GetComponent<CharacterState>().fillHealth();
        player2.GetComponent<CharacterState>().fillHealth();
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
			Time.timeScale = 1f;
			Time.fixedDeltaTime = 0.02f;
			pauseMenu.SetActive(false);
			paused = false;
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
		}
	}

	void restart () 
	{

	}

	public void gameOver() 
	{
		if (GameObject.Find ("Player1Slider").GetComponent<Slider> ().value <= 0) 
		{
			//p1dead = true;

		}
	}

	// Update is called once per frame
	void Update () {

        if (player1.GetComponent<CharacterState>().healthBar.value < 100 && player1.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("hit"))
        {
            p1HealDelay = Time.frameCount + healDelayTime;
        }

        if (player2.GetComponent<CharacterState>().healthBar.value < 100 && player2.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("hit"))
        {
            p2HealDelay = Time.frameCount + healDelayTime;
        }

        if (p1HealDelay < Time.frameCount || p2HealDelay < Time.frameCount)
        {
            player1.GetComponent<CharacterState>().fillHealth();
            p1HealDelay = int.MaxValue;
            p1txt.text = "  ";
            player2.GetComponent<CharacterState>().fillHealth();
            p2HealDelay = int.MaxValue;
            p2txt.text = "  ";
        }


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
			if (Input.GetButtonDown("P1S") && pausedplayer == 1 && !press)
			{
				resume();
				AudioSource.PlayClipAtPoint(confirmSound, GameObject.Find("Camera").transform.position);
			}
			else if (Input.GetButtonDown("P2S") && pausedplayer == 2 && !press)
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

			//Player1 selecting
			if (pausedplayer == 1)
			{
				if (index >= 3 && !press)
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
				else if ((Input.GetAxis("P1Vertical") > 0 || Input.GetAxis("P1DPadVertical") < 0) && (index < 2) && !press) //Down
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
