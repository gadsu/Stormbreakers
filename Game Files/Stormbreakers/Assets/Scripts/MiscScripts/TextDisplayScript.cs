using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using System;

public class TextDisplayScript : MonoBehaviour {

    public float letterPause = 0.05f;
    public Text txt1;
    public Text txt2;
    public Text txtN;
    public Text txtC;
    GameObject txtbox1;
    GameObject txtbox2;
    GameObject txtboxN;
    GameObject player1;
    GameObject player2;

    Vector2 Lstartpos;
    Vector2 Rstartpos;
    Dictionary<string,string> dic;

    int option;
    int section;
    int timeToContinue;
    int timeBetweenTasks;
    string msg;
    string title;
    bool crComplete;
    bool lineFinish;
    bool readyToContinue;
    public bool taskComplete;
    TextAsset TutorialText;

	// Use this for initialization
    void Start () 
    {
        txtbox1 = GameObject.Find("P1Txtbox");
        txtbox2 = GameObject.Find("P2Txtbox");
        txtboxN = GameObject.Find("NarrTxtbox");

        txt1 = txtbox1.GetComponentInChildren<Text>();
        txt2 = txtbox2.GetComponentInChildren<Text>();
        txtN = txtboxN.GetComponentInChildren<Text>();

        txt1.text = "";
        txt2.text = "";
        txtN.text = "";

        txtbox1.SetActive(false);
        txtbox2.SetActive(false);
        txtboxN.SetActive(false);
        option = 0;
        section = 0;
        timeBetweenTasks = 60; //2 seconds
        crComplete = false;
        lineFinish = true;
        readyToContinue = false;
        timeToContinue = 0;
        player1 = GameObject.Find("player1L");
        player2 = GameObject.Find("player2R");
        Lstartpos = player1.transform.position;
        Rstartpos = player2.transform.position;
        TutorialText = Resources.Load("TutorialText", typeof(TextAsset)) as TextAsset;
        dic = TutorialText.text.Split(new char[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).Select(l => l.Split(new[] { '=' })).ToDictionary( s => s[0].Trim(), s => s[1].Trim());

        taskComplete = true;
        startDialogue();

	}

    IEnumerator TypeText()
    {
        crComplete = true;
        foreach (char letter in msg.ToCharArray())
        {
            txtC.text += letter;
            yield return new WaitForSeconds(letterPause);
        }
        lineFinish = true;
    }
	
	// Update is called once per frame
   void startDialogue()
    {
        player1.GetComponent<CommandInterpreter>().enabled = false;
        player1.GetComponent<InputGatherer>().enabled = false;

        player1.transform.position = Lstartpos;
        player1.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player2.transform.position = Rstartpos;
        player2.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        readyToContinue = false;
    }

    void endDialogue()
    {
        player1.GetComponent<CommandInterpreter>().enabled = true;
        player1.GetComponent<InputGatherer>().enabled = true;
        section++;
        taskComplete = false;
    }

    void setMsg()
    {

        title = section.ToString() + "-" + option.ToString();
        msg = dic[title];

        if (msg[0] == 'R')
        {
            msg = msg.Substring(2);
            txtC = txt2;
            txtbox2.SetActive(true);
            txtbox1.SetActive(false);
            txtboxN.SetActive(false);
        }
        else if (msg[0] == 'L')
        {
            msg = msg.Substring(2);
            txtC = txt1;
            txtbox1.SetActive(true);
            txtbox2.SetActive(false);
            txtboxN.SetActive(false);
        }
        else if (msg[0] == 'N')
        {
            msg = msg.Substring(2);
            txtC = txtN;
            txtboxN.SetActive(true);
            txtbox1.SetActive(false);
            txtbox2.SetActive(false);
        }
        txtC.text = " ";
    
    }

	void Update () 
    {

     //   Debug.Log("Section: " + section + "\nOption: " + option);

        if (!readyToContinue && taskComplete && section != 0 && option == -1)
        {
            readyToContinue = true;
            timeToContinue = Time.frameCount + timeBetweenTasks;
        }
        else if (taskComplete && timeToContinue < Time.frameCount && section != 0 && option == -1)
        {
            startDialogue();
            option++;
            crComplete = false;
        }
        else if ((Input.GetButtonDown("P1E") || Input.GetButtonDown("P2E")) && lineFinish && taskComplete && timeToContinue < Time.frameCount)
        {
            /*if (option == -1)
            {
                startDialogue();
            }*/
            option++;
            crComplete = false;
        }

        if (msg == "END")
        {
            endDialogue();
            txtC.text = " ";
            txtbox1.SetActive(false);
            txtbox2.SetActive(false);
            txtboxN.SetActive(false);
            msg = " ";
            option = -1;
        }
        else if (msg == "FINISH")
        {
            SceneManager.LoadScene("MainMenu");
        }
        else if (crComplete == false && lineFinish)
        {
            lineFinish = false;
            setMsg();
            StartCoroutine(TypeText());
        }
	}
}
