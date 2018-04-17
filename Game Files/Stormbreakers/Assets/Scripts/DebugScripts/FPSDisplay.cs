using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSDisplay : MonoBehaviour {

    float deltaTime = 0.0f;
    bool active = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
	}

    void OnGUI()
    {
        if (active)
        {
            int w = Screen.width;
            int h = Screen.height;
            
            GUIStyle style = new GUIStyle();
            
            Rect rect = new Rect(0, 0, w, h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            if (fps < 59)
            {
                text = text + " WARNING! LOW FPS!!";
            }

            if (fps >= 61)
            {
                text = "";
            }
            
            GUI.Label(rect, text, style);

        }

    }
}
