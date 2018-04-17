using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(CommandInterpreter))]

public class InputDisplay : MonoBehaviour {

    string str = "";
    int len = 0;

    public void displayInput(char inputs)
    {
        char player = GetComponent<PlayerSettings>().getPlayerNumber();
        while (str.Length > 10)
        {
            str = str.Substring(1);
        }
            
        if (inputs == '1')
        {
            str += "\u2199";
            len++;
        }
        else if (inputs == '2')
        {
            str += "\u2193";
            len++;
        }
        else if (inputs == '3')
        {
            str += "\u2198";
            len++;
        }
        else if (inputs == '4')
        {
            str += "\u2190";
            len++;
        }
        else if (inputs == '6')
        {
            str += "\u2192";
            len++;
        }
        else if (inputs == '7')
        {
            str += "\u2196";
            len++;
        }
        else if (inputs == '8')
        {
            str += "\u2191";
            len++;
        }
        else if (inputs == '9')
        {
            str += "\u2197";
            len++;
        }
        else if (inputs == 'A')
        {
            str += "\u24B6";
            len++;
        }
        else if (inputs == 'B')
        {
            str += "\u24B7";
            len++;
        }
        else if (inputs == 'C')
        {
            str += "\u24B8";
            len++;
        }
        else if (inputs == 'E')
        {
            str += "\u24BA";
            len++;
        }

        if (GameObject.Find("P" + player + "InputText"))
        {
            Text txt = GameObject.Find("P" + player + "InputText").GetComponent<Text>();
            txt.text = System.Text.RegularExpressions.Regex.Unescape(str);
        }

        //Debug.Log(str);
    }

    /*public void displayInput(char player, char[] inputs)
    {
        if (inputs.Length > 0)
        {
            string s = "";
            for (int i = 0; i < inputs.Length; i++)
            {
                if (inputs[i] == '1')
                {
                    s += "\u2199";
                }
                else if (inputs[i] == '2')
                {
                    s += "\u2193";
                }
                else if (inputs[i] == '3')
                {
                    s += "\u2198";
                }
                else if (inputs[i] == '4')
                {
                    s += "\u2190";
                }
                else if (inputs[i] == '6')
                {
                    s += "\u2192";
                }
                else if (inputs[i] == '7')
                {
                    s += "\u2196";
                }
                else if (inputs[i] == '8')
                {
                    s += "\u2191";
                }
                else if (inputs[i] == '9')
                {
                    s += "\u2197";
                }
                else if (inputs[i] == 'A')
                {
                    s += "\u24B6";
                }
                else if (inputs[i] == 'B')
                {
                    s += "\u24B7";
                }
                else if (inputs[i] == 'C')
                {
                    s += "\u24B8";
                }
                else if (inputs[i] == 'E')
                {
                    s += "\u24BA";
                }
            }
            Text txt = GameObject.Find("P" + player + "InputText").GetComponent<Text>();
            txt.text = System.Text.RegularExpressions.Regex.Unescape(s);
            Debug.Log(s);
        }
    }*/

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
