using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class TutorialModeAIScript : MonoBehaviour {

    TutorialTaskManager ttm;
    TextDisplayScript tds;
    const int delayBetweenActions = 120; //2 seconds
    int timeForNextAction;
    Animator anim;
	// Use this for initialization
	void Start () 
    {
        ttm = GameObject.Find("player1L").GetComponent<TutorialTaskManager>();
        tds = GameObject.Find("Camera").GetComponent<TextDisplayScript>();
        timeForNextAction = 0;
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (timeForNextAction < Time.frameCount && tds.taskComplete == false)
        {
            if (ttm.counter == 7)
            {
                anim.Play("236A");
                timeForNextAction = Time.frameCount + delayBetweenActions;
            }
            else if (ttm.counter == 8)
            {
                anim.Play("2C");
                timeForNextAction = Time.frameCount + delayBetweenActions;
            }
        }
	}
}
