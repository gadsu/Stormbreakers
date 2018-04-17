using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSet : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setStageCC(string s)
    {
        if (GameObject.Find("CharacterChoices"))
        {
            GameObject.Find("CharacterChoices").GetComponent<CharacterChoices>().setStage(s);
        }
    }
}
