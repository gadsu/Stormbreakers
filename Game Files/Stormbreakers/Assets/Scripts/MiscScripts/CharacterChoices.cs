using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterChoices : MonoBehaviour {

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

        Application.targetFrameRate = 60;

        SceneManager.LoadScene("TitleScreen");

    }

    private char p1Character;
    private char p2Character;

    private char p1Element;
    private char p2Element;

    private string stage;
    public int p1Score;
    public int p2Score;
	// Use this for initialization
	void Start () 
    {
        p1Character = ' ';
        p2Character = ' ';

        p1Element = ' ';
        p2Element = ' ';

        resetScore();
	}

    public void winScore(int player)
    {
        if (player == 1)
        {
            p1Score++;
        }
        else if (player == 2)
        {
            p2Score++;
        }
    }

    public void resetScore()
    {
        p1Score = 0;
        p2Score = 0;


    }

    public int getScore(int player)
    {
        if (player == 1)
        {
            return p1Score;
        }
        else if (player == 2)
        {
            return p2Score;
        }

        return 0;
    }

    public void setP1Character(char p1)
    {
        p1Character = p1;
    }

    public void setP2Character(char p2)
    {
        p2Character = p2;
    }
	
    public void setP1Element(char e1)
    {
        p1Element = e1;
    }

    public void setP2Element(char e2)
    {
        p2Element = e2;
    }

    //------------------

    public char getP1Character()
    {
        return p1Character;
    }

    public char getP2Character()
    {
        return p2Character;
    }

    public char getP1Element()
    {
        return p1Element;
    }

    public char getP2Element()
    {
        return p2Element;
    }

    public void setStage(string s)
    {
        stage = s;
    }

    public string getStage()
    {
        return stage;
    }


	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("P1 Character: " + p1Character + " P1 Element: " + p1Element);
            Debug.Log("P2 Character: " + p2Character + " P2 Element: " + p2Element);
        }
	}
}
