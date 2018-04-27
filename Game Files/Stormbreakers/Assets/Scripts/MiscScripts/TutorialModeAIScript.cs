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
    Transform otherPlayer;
    public GameObject p1TextBox;
    public GameObject p2TextBox;
    public GameObject narrTextBox;

    private bool finishedActing;

	// Use this for initialization
	void Start () 
    {
        ttm = GameObject.Find("player1L").GetComponent<TutorialTaskManager>();
        tds = GameObject.Find("Camera").GetComponent<TextDisplayScript>();
        timeForNextAction = 0;
        anim = GetComponent<Animator>();
        otherPlayer = GameObject.Find("player1L").transform;

        finishedActing = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        GetComponent<CharacterState>().fillHealth();

        if (timeForNextAction < Time.frameCount && tds.taskComplete == false)
        {
            if (ttm.counter == 7)
            {
                anim.Play("236A");
                timeForNextAction = Time.frameCount + delayBetweenActions;
            }
            else if (ttm.counter == 8)
            {
                
                GetComponent<CharacterAttacks>().twoC();
                timeForNextAction = Time.frameCount + delayBetweenActions;
            }
        }
        else if (ttm.counter == 8 && !p1TextBox.activeInHierarchy && !p2TextBox.activeInHierarchy && !narrTextBox.activeInHierarchy && !anim.GetCurrentAnimatorStateInfo(0).IsName("2C"))
        {
            Debug.Log("Should be walking??");
            if (otherPlayer.transform.position.x > gameObject.transform.position.x)
            {
                //Debug.Log("Should be walking Right??");
                anim.Play("walk");
                gameObject.GetComponent<CharacterMovement>().walk('6');
            }
            else if (otherPlayer.transform.position.x < gameObject.transform.position.x)
            {
                //Debug.Log("Should be walking Left??");
                anim.Play("walk");
                gameObject.GetComponent<CharacterMovement>().walk('4');
            }
        }
        else
        {
            if (ttm.counter > 8 && !finishedActing)
            {
                Debug.Log("I really hope we're playing the idle animation by now...");
                anim.Play("idle");
                finishedActing = true;
            }
            gameObject.GetComponent<CharacterMovement>().walk('5');
        }
	}
}
