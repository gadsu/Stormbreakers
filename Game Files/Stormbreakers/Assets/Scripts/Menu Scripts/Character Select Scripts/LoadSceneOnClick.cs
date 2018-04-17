using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LoadSceneOnClick : MonoBehaviour {

    private GameObject characterChoices;
    private CharacterChoices cc;
    private string buttonName;
    private GameObject aitext;
    void Start()
    {
        if (GameObject.Find("CharacterChoices"))
        {
            characterChoices = GameObject.Find("CharacterChoices");
            cc = characterChoices.GetComponent<CharacterChoices>();
        }
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            EventSystem es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
            es.SetSelectedGameObject(null);
            es.SetSelectedGameObject(es.firstSelectedGameObject);
        }
        if (GameObject.Find("AIText"))
        {
            aitext = GameObject.Find("AIText");
            aitext.SetActive(false);
        }
    }

    //Assigns character elements to the CharacterChoices script on-click
    public void CharacterSelection()
    {
        buttonName = EventSystem.current.currentSelectedGameObject.name;
        if (GameObject.Find("First Player Character").GetComponent<Image>().sprite.name.ToLower().Contains("randolf"))
        {
            cc.setP1Character('r');
        }
        else if (GameObject.Find("First Player Character").GetComponent<Image>().sprite.name.ToLower().Contains("lynne"))
        {
            cc.setP1Character('l');
        }

        if (GameObject.Find("Second Player Character").GetComponent<Image>().sprite.name.ToLower().Contains("randolf"))
        {
            cc.setP2Character('r');
        }
        else if (GameObject.Find("Second Player Character").GetComponent<Image>().sprite.name.ToLower().Contains("lynne"))
        {
            cc.setP2Character('l');
        }

        if (buttonName.Contains("Left"))
        {
            if (buttonName.Contains("Life"))
            {
                cc.setP1Element('l');
            }
            else if (buttonName.Contains("Death"))
            {
                cc.setP1Element('d');
            }
            else if (buttonName.Contains("Space"))
            {
                cc.setP1Element('s');
            }
            else if (buttonName.Contains("Time"))
            {
                cc.setP1Element('t');
            }
            if (SceneManager.GetActiveScene().name == "TrainingCharacterSelect")
            {
                GameObject.Find("Chartext").SetActive(false);
                aitext.SetActive(true);
            }
        }
        else if (buttonName.Contains("Right"))
        {
            if (buttonName.Contains("Life"))
            {
                cc.setP2Element('l');
            }
            else if (buttonName.Contains("Death"))
            {
                cc.setP2Element('d');
            }
            else if (buttonName.Contains("Space"))
            {
                cc.setP2Element('s');
            }
            else if (buttonName.Contains("Time"))
            {
                cc.setP2Element('t');
            }
        }

        if (cc.getP1Element() != ' ' && cc.getP2Element() != ' ' && cc.getP1Character() != ' ' && cc.getP2Character() != ' ')
        {
            if (SceneManager.GetActiveScene().name == "TrainingCharacterSelect")
            {
                SceneManager.LoadScene("TrainingMode");
            }
            else
            {
                SceneManager.LoadScene("StageSelect");
            }
        }
    }
    //Clears choices (Back button)
    public void clearElementChoices()
    {
        cc.setP1Element(' ');
        cc.setP2Element(' ');
        cc.setP2Character(' ');
        cc.setP1Character(' ');
    }

	public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

	public void LoadByName(string name)
	{
		SceneManager.LoadScene(name);
	}

    public void ExitGame()
    {
        Application.Quit();
    }
}