using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneSelectScript : MonoBehaviour {
    
    private CharacterChoices cc;

    private bool p1CharSet = false;
    private bool p2CharSet = false;
    private bool p1ElemSet = false;
    private bool p2ElemSet = false;

    Image img;

    private AudioSource audioSource;
	// Use this for initialization
	void Start () {
        img = GameObject.Find("Selected Stage").GetComponent<Image>();

            if (GameObject.Find("CharacterChoices"))
            {
                //Set Characters
                if (GameObject.Find("CharacterChoices").GetComponent<CharacterChoices>().getP1Character() == 'r')
                {
                    if (GameObject.Find("player1L"))
                    {
                        GameObject.Find("player1L").SetActive(false);
                        p1CharSet = true;
                    }
                }
                else if (GameObject.Find("CharacterChoices").GetComponent<CharacterChoices>().getP1Character() == 'l')
                {
                    if (GameObject.Find("player1R"))
                    {
                        GameObject.Find("player1R").SetActive(false);
                        p1CharSet = true;
                    }
                }
                else if(!p1CharSet)
                {
                    if (GameObject.Find("player1L"))
                    {
                        GameObject.Find("player1L").SetActive(false);
                        p1CharSet = true;
                    }
                }

                if (GameObject.Find("CharacterChoices").GetComponent<CharacterChoices>().getP2Character() == 'r')
                {
                    if (GameObject.Find("player2L"))
                    {
                        GameObject.Find("player2L").SetActive(false);
                        p2CharSet = true;
                    }
                }
                else if (GameObject.Find("CharacterChoices").GetComponent<CharacterChoices>().getP2Character() == 'l')
                {
                    if (GameObject.Find("player2R"))
                    {
                        GameObject.Find("player2R").SetActive(false);
                        p2CharSet = true;
                    }
                }
                else if (!p2CharSet)
                {
                    if (GameObject.Find("player2R"))
                    {
                        GameObject.Find("player2R").SetActive(false);
                        p1CharSet = true;
                    }
                }


                //Elements P1
                if (GameObject.Find("CharacterChoices").GetComponent<CharacterChoices>().getP1Element() == 'l')
                {
                    if (GameObject.Find("P1L Icon"))
                    {
                        GameObject.Find("P1D Icon").SetActive(false);
                        GameObject.Find("P1S Icon").SetActive(false);
                        GameObject.Find("P1T Icon").SetActive(false);
                        p1ElemSet = true;
                    }
                }
                else if (GameObject.Find("CharacterChoices").GetComponent<CharacterChoices>().getP1Element() == 'd')
                {
                    if (GameObject.Find("P1D Icon"))
                    {
                        GameObject.Find("P1L Icon").SetActive(false);
                        GameObject.Find("P1S Icon").SetActive(false);
                        GameObject.Find("P1T Icon").SetActive(false);
                        p1ElemSet = true;
                    }
                }
                else if (GameObject.Find("CharacterChoices").GetComponent<CharacterChoices>().getP1Element() == 's')
                {
                    if (GameObject.Find("P1S Icon"))
                    {
                        GameObject.Find("P1L Icon").SetActive(false);
                        GameObject.Find("P1D Icon").SetActive(false);
                        GameObject.Find("P1T Icon").SetActive(false);
                        p1ElemSet = true;
                    }
                }
                else if (GameObject.Find("CharacterChoices").GetComponent<CharacterChoices>().getP1Element() == 't')
                {
                    if (GameObject.Find("P1T Icon"))
                    {
                        GameObject.Find("P1L Icon").SetActive(false);
                        GameObject.Find("P1S Icon").SetActive(false);
                        GameObject.Find("P1D Icon").SetActive(false);
                        p1ElemSet = true;
                    }
                }
                else if(!p1ElemSet)
                {
                    Debug.Log("No element set p1");
                    if (GameObject.Find("P1L Icon"))
                    {
                        GameObject.Find("P1D Icon").SetActive(false);
                        GameObject.Find("P1S Icon").SetActive(false);
                        GameObject.Find("P1T Icon").SetActive(false);
                        p1ElemSet = true;
                    }
                }

                //Elements P2
                if (GameObject.Find("CharacterChoices").GetComponent<CharacterChoices>().getP2Element() == 'l')
                {
                    if (GameObject.Find("P2L Icon"))
                    {
                        GameObject.Find("P2D Icon").SetActive(false);
                        GameObject.Find("P2S Icon").SetActive(false);
                        GameObject.Find("P2T Icon").SetActive(false);
                        p2ElemSet = true;
                    }
                }
                else if (GameObject.Find("CharacterChoices").GetComponent<CharacterChoices>().getP2Element() == 'd')
                {
                    if (GameObject.Find("P2D Icon"))
                    {
                        GameObject.Find("P2L Icon").SetActive(false);
                        GameObject.Find("P2S Icon").SetActive(false);
                        GameObject.Find("P2T Icon").SetActive(false);
                        p2ElemSet = true;
                    }
                }
                else if (GameObject.Find("CharacterChoices").GetComponent<CharacterChoices>().getP2Element() == 's')
                {
                    if (GameObject.Find("P2S Icon"))
                    {
                        GameObject.Find("P2L Icon").SetActive(false);
                        GameObject.Find("P2D Icon").SetActive(false);
                        GameObject.Find("P2T Icon").SetActive(false);
                        p2ElemSet = true;
                    }
                }
                else if (GameObject.Find("CharacterChoices").GetComponent<CharacterChoices>().getP2Element() == 't')
                {
                    if (GameObject.Find("P2T Icon"))
                    {
                        GameObject.Find("P2L Icon").SetActive(false);
                        GameObject.Find("P2S Icon").SetActive(false);
                        GameObject.Find("P2D Icon").SetActive(false);
                        p2ElemSet = true;
                    }
                }
                else if(!p2ElemSet)
            {
                Debug.Log("No element set p2");
                    if (GameObject.Find("P2L Icon"))
                    {
                        GameObject.Find("P2D Icon").SetActive(false);
                        GameObject.Find("P2S Icon").SetActive(false);
                        GameObject.Find("P2T Icon").SetActive(false);
                        p1ElemSet = true;
                    }
                }
            }
            else  //No settings file found - for testing usually
            {
                GameObject.Find("player1L").SetActive(false);
                p1CharSet = true;

                GameObject.Find("player2R").SetActive(false);
                p1CharSet = true;

                GameObject.Find("P1D Icon").SetActive(false);
                GameObject.Find("P1S Icon").SetActive(false);
                GameObject.Find("P1T Icon").SetActive(false);
                p1ElemSet = true;

                GameObject.Find("P2L Icon").SetActive(false);
                GameObject.Find("P2S Icon").SetActive(false);
                GameObject.Find("P2T Icon").SetActive(false);
                p1ElemSet = true;
            }

        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("P1C") || Input.GetButtonDown("P2C"))
		{
			Button back = GameObject.Find ("Back Button").GetComponent<Button>();
			back.Select ();
			back.onClick.Invoke ();
		}
        if(GameObject.Find("EventSystem").GetComponent<EventSystem>().currentSelectedGameObject.name.Contains("Fountain"))
        {
            img.sprite = GameObject.Find("Fountain Button").GetComponent<Image>().sprite;
        }
        else if(GameObject.Find("EventSystem").GetComponent<EventSystem>().currentSelectedGameObject.name.Contains("Dojo"))
        {
            img.sprite = GameObject.Find("Dojo Button").GetComponent<Image>().sprite;
        }
	}
}
