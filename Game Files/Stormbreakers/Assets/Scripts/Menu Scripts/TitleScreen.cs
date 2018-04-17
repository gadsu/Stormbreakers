using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject.Find("BLACK").GetComponent<Animator>().Play("BlackFade");
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetButtonDown("P1E") || Input.GetButtonDown("P2E") || Input.GetButtonDown("P1A") || Input.GetButtonDown("P2A")
            || Input.GetButtonDown("P1B") || Input.GetButtonDown("P2B") || Input.GetButtonDown("P1C") || Input.GetButtonDown("P2C")
            || Input.GetButtonDown("P1S") || Input.GetButtonDown("P2S")) // ya press a fekkin button
                {
                     GameObject.Find("BLACK").GetComponent<Animator>().Play("BlackFadeout");
                }
	}
        
}
