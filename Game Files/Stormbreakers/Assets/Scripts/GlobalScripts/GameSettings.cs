/*
 * Script that checks the settings players have set for
 * the game, primarily the button configuration, characters, and elements.  Sets the stage and UI elements accordingly.
 * 
 * This might eventually end up just reading that info from
 * a settings file, but for now, it'll be stored in here.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour {

    // Bools to check if an option was selected
    private bool p1CharSet = false;
    private bool p2CharSet = false;
    private bool p1ElemSet = false;
    private bool p2ElemSet = false;
    private CharacterChoices cc;

    private AudioSource audioSource;

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

	// Use this for initialization
	void Start () 
    {
        //Read all settings from the CharacterChoices file - and apply them.
        if (GameObject.Find("CharacterChoices"))
        {
            cc = GameObject.Find("CharacterChoices").GetComponent<CharacterChoices>();
        

            if(SceneManager.GetActiveScene().name == "TutorialMode")
            {
                GameObject.Find("P1R Portrait").SetActive(false);
                p1CharSet = true;

                GameObject.Find("P2L Portrait").SetActive(false);
                p2CharSet = true;

                GameObject.Find("P1L Icon").SetActive(false);
                GameObject.Find("P1D Icon").SetActive(false);
                GameObject.Find("P1S Icon").SetActive(false);
                GameObject.Find("P1T Icon").SetActive(false);
                p1ElemSet = true;
                cc.setP1Element(' ');

                GameObject.Find("P2L Icon").SetActive(false);
                GameObject.Find("P2D Icon").SetActive(false);
                GameObject.Find("P2S Icon").SetActive(false);
                GameObject.Find("P2T Icon").SetActive(false);
                p2ElemSet = true;
                cc.setP2Element(' ');
            }
            else
            {
                //Set Characters
                if (cc.getP1Character() == 'r')
                {
                    if (GameObject.Find("player1L"))
                    {
                        GameObject.Find("player1L").SetActive(false);
                        GameObject.Find("P1L Portrait").SetActive(false);
                        p1CharSet = true;
                    }
                }
                else if (cc.getP1Character() == 'l')
                {
                    if (GameObject.Find("player1R"))
                    {
                        GameObject.Find("player1R").SetActive(false);
                        GameObject.Find("P1R Portrait").SetActive(false);
                        p1CharSet = true;
                    }
                }
                else if(!p1CharSet)
                {
                    if (GameObject.Find("player1L"))
                    {
                        GameObject.Find("player1L").SetActive(false);
                        GameObject.Find("P1L Portrait").SetActive(false);
                        p1CharSet = true;
                    }
                }

                if (cc.getP2Character() == 'r')
                {
                    if (GameObject.Find("player2L"))
                    {
                        GameObject.Find("player2L").SetActive(false);
                        GameObject.Find("P2L Portrait").SetActive(false);
                        p2CharSet = true;
                    }
                }
                else if (cc.getP2Character() == 'l')
                {
                    if (GameObject.Find("player2R"))
                    {
                        GameObject.Find("player2R").SetActive(false);
                        GameObject.Find("P2R Portrait").SetActive(false);
                        p2CharSet = true;
                    }
                }
                else if (!p2CharSet)
                {
                    if (GameObject.Find("player2R"))
                    {
                        GameObject.Find("player2R").SetActive(false);
                        GameObject.Find("P2R Portrait").SetActive(false);
                        p1CharSet = true;
                    }
                }
                 

                //Elements P1
                if (cc.getP1Element() == 'l')
                {
                    if (GameObject.Find("P1L Icon"))
                    {
                        GameObject.Find("P1D Icon").SetActive(false);
                        GameObject.Find("P1S Icon").SetActive(false);
                        GameObject.Find("P1T Icon").SetActive(false);
                        p1ElemSet = true;
                        Debug.Log("P1Life");
                    }
                }
                else if (cc.getP1Element() == 'd')
                {
                    if (GameObject.Find("P1D Icon"))
                    {
                        GameObject.Find("P1L Icon").SetActive(false);
                        GameObject.Find("P1S Icon").SetActive(false);
                        GameObject.Find("P1T Icon").SetActive(false);
                        p1ElemSet = true;
                        Debug.Log("P1Death");
                    }
                }
                else if (cc.getP1Element() == 's')
                {
                    if (GameObject.Find("P1S Icon"))
                    {
                        GameObject.Find("P1L Icon").SetActive(false);
                        GameObject.Find("P1D Icon").SetActive(false);
                        GameObject.Find("P1T Icon").SetActive(false);
                        p1ElemSet = true;
                        Debug.Log("P1Space");
                    }
                }
                else if (cc.getP1Element() == 't')
                {
                    if (GameObject.Find("P1T Icon"))
                    {
                        GameObject.Find("P1L Icon").SetActive(false);
                        GameObject.Find("P1S Icon").SetActive(false);
                        GameObject.Find("P1D Icon").SetActive(false);
                        p1ElemSet = true;
                        Debug.Log("P1Time");
                    }
                }
                else if(!p1ElemSet)
                {
                    if (GameObject.Find("P1L Icon"))
                    {
                        GameObject.Find("P1D Icon").SetActive(false);
                        GameObject.Find("P1S Icon").SetActive(false);
                        GameObject.Find("P1T Icon").SetActive(false);
                        p1ElemSet = true;
                        Debug.Log("P1Life");
                    }
                }

                //Elements P2
                if (cc.getP2Element() == 'l')
                {
                    if (GameObject.Find("P2L Icon"))
                    {
                        GameObject.Find("P2D Icon").SetActive(false);
                        GameObject.Find("P2S Icon").SetActive(false);
                        GameObject.Find("P2T Icon").SetActive(false);
                        p2ElemSet = true;
                        Debug.Log("P2Life");
                    }
                }
                else if (cc.getP2Element() == 'd')
                {
                    if (GameObject.Find("P2D Icon"))
                    {
                        GameObject.Find("P2L Icon").SetActive(false);
                        GameObject.Find("P2S Icon").SetActive(false);
                        GameObject.Find("P2T Icon").SetActive(false);
                        p2ElemSet = true;
                        Debug.Log("P2Death");
                    }
                }
                else if (cc.getP2Element() == 's')
                {
                    if (GameObject.Find("P2S Icon"))
                    {
                        GameObject.Find("P2L Icon").SetActive(false);
                        GameObject.Find("P2D Icon").SetActive(false);
                        GameObject.Find("P2T Icon").SetActive(false);
                        p2ElemSet = true;
                        Debug.Log("P2Space");
                    }
                }
                else if (cc.getP2Element() == 't')
                {
                    if (GameObject.Find("P2T Icon"))
                    {
                        GameObject.Find("P2L Icon").SetActive(false);
                        GameObject.Find("P2S Icon").SetActive(false);
                        GameObject.Find("P2D Icon").SetActive(false);
                        p2ElemSet = true;
                        Debug.Log("P2Time");
                    }
                }
                else if(!p2ElemSet)
                {
                    if (GameObject.Find("P2L Icon"))
                    {
                        GameObject.Find("P2D Icon").SetActive(false);
                        GameObject.Find("P2S Icon").SetActive(false);
                        GameObject.Find("P2T Icon").SetActive(false);
                        p1ElemSet = true;
                        Debug.Log("P2Life");
                    }
                }
            }

            //set stage
            if (SceneManager.GetActiveScene().name == "Stage1")
            {
                if (cc.getStage() == "Fountain")
                {
                    GameObject.Find("DojoGraphics").SetActive(false);
                }
                else if (cc.getStage() == "Dojo")
                {
                    GameObject.Find("FountainGraphics").SetActive(false);
                }
            }

        }
        else  //No settings file found - for testing usually
        {
            GameObject.Find("player1L").SetActive(false);
            GameObject.Find("P1L Portrait").SetActive(false);
            p1CharSet = true;

            GameObject.Find("player2R").SetActive(false);
            GameObject.Find("P2R Portrait").SetActive(false);
            p2CharSet = true;

            GameObject.Find("P1D Icon").SetActive(false);
            GameObject.Find("P1S Icon").SetActive(false);
            GameObject.Find("P1T Icon").SetActive(false);
            p1ElemSet = true;

            GameObject.Find("P2L Icon").SetActive(false);
            GameObject.Find("P2S Icon").SetActive(false);
            GameObject.Find("P2T Icon").SetActive(false);
            p2ElemSet = true;
        }



        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.Play();

	}



    //Function to convert a keycode into the right character //DO WE NEED THIS?
    public static char convertStuffToInput(KeyCode k)
    {
        switch (k)
        {
            case KeyCode.UpArrow:
                return '8';
            case KeyCode.DownArrow:
                return '2';
            case KeyCode.LeftArrow:
                return '4';
            case KeyCode.RightArrow:
                return '6';
			case KeyCode.A:
				return 'A';
			case KeyCode.B:
				return 'B';
			case KeyCode.C:
				return 'C';
			case KeyCode.E:
				return 'E';
            default:
                return '5';
        }
    }

    //Function to convert a list of keycodes into the right character  //DO WE NEED THIS?
    public static char convertStuffToInput(List<KeyCode> l)
    {
        List<char> convertedKeyCodes = new List<char>();

        foreach (KeyCode k in l)
        {
            convertedKeyCodes.Add(convertStuffToInput(k));
        }

		if (l.Count < 2) 
		{
			return convertStuffToInput (l[0]);
		}
		else if ((convertedKeyCodes.Contains('4') && convertedKeyCodes.Contains('6')) 
			|| (convertedKeyCodes.Contains('8') && convertedKeyCodes.Contains('2'))) 
		{
			return '5';
		} 
		else if (convertedKeyCodes.Contains('2') && convertedKeyCodes.Contains('4')) 
		{
			return '1';
		} 
		else if (convertedKeyCodes.Contains('4') && convertedKeyCodes.Contains('8')) 
		{
			return '7';
		} 
		else if (convertedKeyCodes.Contains('8') && convertedKeyCodes.Contains('6')) 
		{
			return '9';
		} 
		else if (convertedKeyCodes.Contains('2') && convertedKeyCodes.Contains('6')) 
		{
			return '3';
		}
		else
		{
			return '5';
		}
    }

    //Convert controller input into directional characters
    public static char convertStuffToInput(Vector2 direction)
    {
        if (direction.x > 0)
        {
            if (direction.y > 0)
            {
                return '9';
            }
            else if (direction.y < 0)
            {
                return '3';
            }
            else
            {
                return '6';
            }
        }
        else if (direction.x < 0)
        {
            if (direction.y > 0)
            {
                return '7';
            }
            else if (direction.y < 0)
            {
                return '1';
            }
            else
            {
                return '4';
            }
        }
        else if (direction.y < 0)
        {
            return '2';
        }
        else if (direction.y > 0)
        {
            return '8';
        }

        return '5';
    }

    //Convert controller input into attack characters
    public static char convertStuffToInput(List<char> l)
    {
        if (l.Count == 1)
        {
            return l[0];
        }
        else
        {
            if (l.Contains('E'))
            {
                if (l.Contains('A'))
                {
                    return 'T';
                }
                else if (l.Contains('B') || l.Contains('C'))
                {
                    return 'S';
                }
                else
                {
                    return 'E';
                }
            }
            else if (l.Contains('C'))
            {
                return 'C';
            }
            else if (l.Contains('B'))
            {
                return 'B';
            }
            else if (l.Contains('A'))
            {
                return 'A';
            }
        }

        return ' ';
    }

    public char convertStuffToInput(string button)
    {
        return '5';
    }
        
	
	// Update is called once per frame
	void Update () {
		
	}
}
