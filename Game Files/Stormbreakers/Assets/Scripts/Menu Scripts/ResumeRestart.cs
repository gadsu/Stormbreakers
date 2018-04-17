using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeRestart : MonoBehaviour {

    public void gameResume () {
        if (GameObject.Find("Timer").GetComponent<PauseMenu>())
        {
            GameObject.Find("Timer").GetComponent<PauseMenu>().resume();
        }
        else if (GameObject.Find("Timer").GetComponent<TrainingPauseMenu>())
        {
            GameObject.Find("Timer").GetComponent<TrainingPauseMenu>().resume();
        }
    }

}
