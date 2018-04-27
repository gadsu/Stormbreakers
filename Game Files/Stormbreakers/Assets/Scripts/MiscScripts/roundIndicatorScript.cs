using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class roundIndicatorScript : MonoBehaviour {

    public Sprite openSprite;
    public Sprite closedSprite;

    private CharacterState cs;
    private Image img;
    private CharacterChoices cc;
    private int playerIndex;
    private int roundIndex;

	// Use this for initialization
	void Start () 
    {
        img = GetComponent<Image>();
        cc = GameObject.Find("CharacterChoices").GetComponent<CharacterChoices>();

        if (gameObject.name.Contains("P1")) //Naming Convention: "RoundCounter" followed by "P" and the player number, followed by "R" and then the round that the object is supposed to indicate (1 or 2)
        {
            Debug.Log("player1" + cc.getP1Character());
            cs = GameObject.Find("player1" + cc.getP1Character().ToString().ToUpper()).GetComponent<CharacterState>();
            playerIndex = 1;
        }
        else if (gameObject.name.Contains("P2"))
        {
            cs = GameObject.Find("player2" + cc.getP2Character().ToString().ToUpper()).GetComponent<CharacterState>();
            playerIndex = 2;
        }

        if (gameObject.name[15] == '1')
        {
            roundIndex = 1;
        }
        else if (gameObject.name[15] == '2')
        {
            roundIndex = 2;
        }

        if (playerIndex == 1)
        {
            //Debug.Log(Time.frameCount.ToString() + " : " + cc.p1Score + " : " + roundIndex);
            if (cc.p1Score >= roundIndex && img.sprite != openSprite)
            {
                Debug.Log("Sprite Swap!");
                img.sprite = openSprite;
            }
            else if (cc.p1Score < roundIndex && img.sprite != closedSprite)
            {
                Debug.Log("Sprite Swap!");
                img.sprite = closedSprite;
            }
        }
        else if (playerIndex == 2)
        {
            if (cc.p2Score >= roundIndex && img.sprite != openSprite)
            {
                Debug.Log("Sprite Swap!");
                img.sprite = openSprite;
            }
            else if (cc.p2Score < roundIndex && img.sprite != closedSprite)
            {
                Debug.Log("Sprite Swap!");
                img.sprite = closedSprite;
            }
        }
	}

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded!");
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (playerIndex == 1)
        {
            //Debug.Log(Time.frameCount.ToString() + " : " + cc.p1Score + " : " + roundIndex);
            if (cc.p1Score >= roundIndex && img.sprite != openSprite)
            {
                Debug.Log("Sprite Swap!");
                img.sprite = openSprite;
            }
        }
        else if (playerIndex == 2)
        {
            if (cc.p2Score >= roundIndex && img.sprite != openSprite)
            {
                Debug.Log("Sprite Swap!");
                img.sprite = openSprite;
            }
        }

	}
}