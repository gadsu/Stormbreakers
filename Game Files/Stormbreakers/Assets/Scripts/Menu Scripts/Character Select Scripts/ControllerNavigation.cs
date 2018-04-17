using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControllerNavigation : MonoBehaviour {

    private int p1index = 0;
    private int p2index = 0;
    public Button[] p1b = new Button[7];
    public Button[] p2b = new Button[7];
    private Color32 p1Color = new Color32(42, 34, 255, 255);
    private Color32 p2Color = new Color32(255, 55, 255, 255);
    private bool p1press = false;
    private bool p2press = false;
    private EventSystem es;

	// Use this for initialization
	void Start () {
        p1b[p1index].image.color = p1Color;
        p2b[p2index].image.color = p2Color;
        es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("P1E"))
        {
            p1b[p1index].Select();
            p1b[p1index].onClick.Invoke();

            if (p1index == 0 || p1index == 1)
            {
                p1b[p1index].image.color = Color.white;
                p1index = 2;
                p1b[p1index].image.color = p1Color;
                p1press = true;
            }
        }
        else if (Input.GetButtonDown("P2E"))
        {
            p2b[p2index].Select();
            p2b[p2index].onClick.Invoke();

            if (p2index == 0 || p2index == 1)
            {
                p2b[p2index].image.color = Color.white;
                p2index = 2;
                p2b[p2index].image.color = p2Color;
                p2press = true;
            }
        }
        else if (Input.GetButtonDown("P1C") || Input.GetButtonDown("P2C"))
        {
            p1b[6].Select();
            p1b[6].onClick.Invoke();
        }

        //Player1 selecting
        if ((Input.GetAxis("P1Vertical") > 0 || Input.GetAxis("P1DPadVertical") < 0) && (p1index == 0 || p1index == 1) && !p1press) //Down
        {
            p1b[p1index].image.color = Color.white;
            p1index++;
            Debug.Log("P1: " + p1b[p1index]);
            p1b[p1index].image.color = p1Color;
            p1press = true;
        }
        else if ((Input.GetAxis("P1Vertical") < 0 || Input.GetAxis("P1DPadVertical") > 0)  && (p1index != 0 ) && !p1press) //Up
        {
            p1b[p1index].image.color = Color.white;
            if (p1index == 1 || p1index == 2)
            {
                p1index--;
            }
            else
            {
                p1index = 1;
            }
            Debug.Log("P1: " + p1b[p1index]);
            p1b[p1index].image.color = p1Color;
            p1press = true;
        }
        else if ((Input.GetAxis("P1Horizontal") < 0 || Input.GetAxis("P1DPadHorizontal") < 0)  && (p1index != 0 ) && !p1press) //Left
        {
            p1b[p1index].image.color = Color.white;
            if (p1index >= 2 && p1index <= 4)
            {
                p1index++;
            }
            else if (p1index == 5)
            {
                p1index = 2;
            }
            Debug.Log("P1: " + p1b[p1index]);
            p1b[p1index].image.color = p1Color;
            p1press = true;
        }
        else if ((Input.GetAxis("P1Horizontal") > 0 || Input.GetAxis("P1DPadHorizontal") > 0)  && (p1index != 0 ) && !p1press) //Right
        {
            p1b[p1index].image.color = Color.white;
            if (p1index >= 3 && p1index <= 5)
            {
                p1index--;
            }
            else if (p1index == 2)
            {
                p1index = 5;
            }
            Debug.Log("P1: " + p1b[p1index]);
            p1b[p1index].image.color = p1Color;
            p1press = true;
        }
        else if((Input.GetAxis("P1Vertical") == 0 && Input.GetAxis("P1DPadVertical") == 0) && p1press && Input.GetAxis("P1Horizontal") == 0 && Input.GetAxis("P1DPadHorizontal") == 0)
        {
            p1press = false;
        }



        //Player2 selecting
        if ((Input.GetAxis("P2Vertical") > 0 || Input.GetAxis("P2DPadVertical") < 0) && (p2index == 0 || p2index == 1) && !p2press) //Down
        {
            p2b[p2index].image.color = Color.white;
            p2index++;
            Debug.Log("p2: " + p2b[p2index]);
            p2b[p2index].image.color = p2Color;
            p2press = true;
        }
        else if ((Input.GetAxis("P2Vertical") < 0 || Input.GetAxis("P2DPadVertical") > 0)  && (p2index != 0 ) && !p2press) //Up
        {
            p2b[p2index].image.color = Color.white;
            if (p2index == 1 || p2index == 2)
            {
                p2index--;
            }
            else
            {
                p2index = 1;
            }
            Debug.Log("p2: " + p2b[p2index]);
            p2b[p2index].image.color = p2Color;
            p2press = true;
        }
        else if ((Input.GetAxis("P2Horizontal") < 0 || Input.GetAxis("P2DPadHorizontal") < 0)  && (p2index != 0 ) && !p2press) //Left
        {
            p2b[p2index].image.color = Color.white;
            if (p2index >= 3 && p2index <= 5)
            {
                p2index--;
            }
            else if (p2index == 2)
            {
                p2index = 5;
            }
            Debug.Log("P2: " + p2b[p2index]);
            p2b[p2index].image.color = p2Color;
            p2press = true;
        }
        else if ((Input.GetAxis("P2Horizontal") > 0 || Input.GetAxis("P2DPadHorizontal") > 0)  && (p2index != 0 ) && !p2press) //Right
        {
            p2b[p2index].image.color = Color.white;
            if (p2index >= 2 && p2index <= 4)
            {
                p2index++;
            }
            else if (p2index == 5)
            {
                p2index = 2;
            }
            Debug.Log("p2: " + p2b[p2index]);
            p2b[p2index].image.color = p2Color;
            p2press = true;
        }
        else if((Input.GetAxis("P2Vertical") == 0 && Input.GetAxis("P2DPadVertical") == 0) && p2press && Input.GetAxis("P2Horizontal") == 0 && Input.GetAxis("P2DPadHorizontal") == 0)
        {
            p2press = false;
        }
	}
}