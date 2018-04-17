using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CommandInterpreter))]
[RequireComponent(typeof(PlayerSettings))]

public class InputGatherer : MonoBehaviour {

    //-------------------------------------GLOBAL VARIABLES-------------------------------------//
	public List<KeyCode> lastFrameMoveInput;
	public List<KeyCode> lastFrameAttackInput;

    public Vector2 lastFrameControllerMoveInput;
    public List<char> lastFrameControllerAttackInput;

	public List<KeyCode> currentMoveInput;
	public List<KeyCode> currentAttackInput;

    public Vector2 currentControllerMoveInput;
    public List<char> currentControllerAttackInput;

    public char currentMove;
    public char currentAttack;

    public char lastMove;
    public char lastAttack;

    float deadZone;
    float deadpadZone;

    CommandInterpreter ci;
    PlayerSettings ps;

    //-------------------------------------FUNCTIONS-------------------------------------//

	void Start () 
	{
		lastFrameMoveInput = new List<KeyCode> ();
		lastFrameAttackInput = new List<KeyCode> ();

        lastFrameControllerMoveInput = new Vector2();
        lastFrameControllerAttackInput = new List<char>();

		currentMoveInput = new List<KeyCode>();
		currentAttackInput = new List<KeyCode>();

        currentControllerMoveInput = new Vector2();
        currentControllerAttackInput = new List<char>();

        if (GetComponent<CommandInterpreter>())
        {
            ci = GetComponent<CommandInterpreter>();
        }

        if (GetComponent<PlayerSettings>())
        {
            ps = GetComponent<PlayerSettings>();
        }
        deadZone = 0.95f;
        deadpadZone = 0.4f;
	}

    public void resetInputs()
    {
        lastFrameMoveInput = new List<KeyCode>();
        lastFrameAttackInput = new List<KeyCode>();

        lastFrameControllerMoveInput = new Vector2();
        lastFrameControllerAttackInput = new List<char>();
    }

    /*
     * Function to get all the movement-related keys currently being pressed.
     * Does NOT check to see if the keys were pressed on the last
     * frame, or anything like that.
     */
    private List<KeyCode> getAllCurrentMovementKeys()
    {
        List<KeyCode> retVal = new List<KeyCode>();

        foreach (KeyCode k in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(k) && isDirection(k))
            {
                retVal.Add(k);
            }
        }

        return retVal;
    }

    private Vector2 getCurrentControllerDirectionInput()
    {
        if (Input.GetAxis("P" + ps.getPlayerNumber() + "Horizontal") > deadZone  || Input.GetAxis("P" + ps.getPlayerNumber() + "DPadHorizontal") > deadpadZone)
        {
            if (Input.GetAxis("P" + ps.getPlayerNumber() + "Vertical") > deadZone || Input.GetAxis("P" + ps.getPlayerNumber() + "DPadVertical") < -deadpadZone)
            {
                return new Vector2(1, -1); //3
            }
            else if (Input.GetAxis("P" + ps.getPlayerNumber() + "Vertical") < (-1.0f * deadZone) || Input.GetAxis("P" + ps.getPlayerNumber() + "DPadVertical") > deadpadZone) 
            {
                return new Vector2(1, 1); //9
            }
            else
            {
                return new Vector2(1, 0); //6
            }
        }
        else if (Input.GetAxis("P" + ps.getPlayerNumber() + "Horizontal") < (-1.0f * deadZone) || Input.GetAxis("P" + ps.getPlayerNumber() + "DPadHorizontal") < -deadpadZone)
        {
            if (Input.GetAxis("P" + ps.getPlayerNumber() + "Vertical") > deadZone || Input.GetAxis("P" + ps.getPlayerNumber() + "DPadVertical") < -deadpadZone)
            {
                return new Vector2(-1, -1); //1
            }
            else if (Input.GetAxis("P" + ps.getPlayerNumber() + "Vertical") < (-1.0f * deadZone) || Input.GetAxis("P" + ps.getPlayerNumber() + "DPadVertical") > deadpadZone)
            {
                return new Vector2(-1, 1); //7
            }
            else
            {
                return new Vector2(-1, 0); //4
            }
        }
        else if (Input.GetAxis("P" + ps.getPlayerNumber() + "Vertical") > deadZone || Input.GetAxis("P" + ps.getPlayerNumber() + "DPadVertical") < -deadpadZone)
        {
            return new Vector2(0, -1); //2
        }
        else if (Input.GetAxis("P" + ps.getPlayerNumber() + "Vertical") < (-1.0f * deadZone) || Input.GetAxis("P" + ps.getPlayerNumber() + "DPadVertical") > deadpadZone)
        {
            return new Vector2(0, 1); //8
        }

        return new Vector2();
    }

    public List<char> getCurrentControllerAttackInput()
    {
        List<char> retVal = new List<char>();

        if (gameObject.name.Contains("player1"))
        {
            if(Input.GetButtonDown("P1E"))
            {
                retVal.Add('E');
            }

            if(Input.GetButtonDown("P1C"))
            {
                retVal.Add('C');
            }

            if(Input.GetButtonDown("P1B"))
            {
                retVal.Add('B');
            }

            if(Input.GetButtonDown("P1A"))
            {
                retVal.Add('A');
            }
        }
        else if (gameObject.name.Contains("player2"))
        {
            if(Input.GetButtonDown("P2E"))
            {
                retVal.Add('E');
            }

            if(Input.GetButtonDown("P2C"))
            {
                retVal.Add('C');
            }

            if(Input.GetButtonDown("P2B"))
            {
                retVal.Add('B');
            }

            if(Input.GetButtonDown("P2A"))
            {
                retVal.Add('A');
            }
        }

        return retVal;
    }

    /*
     * Same as above, but this one works for attacks.
     */
    private List<KeyCode> getAllCurrentAttackKeys()
    {
        List<KeyCode> retVal = new List<KeyCode>();

        foreach (KeyCode k in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(k) && isAttack(k))
            {
                retVal.Add(k);
            }
        }

        return retVal;
    }


    /*
     * Simple helper function to tell if a key passed is a directional input or not
     */
    public bool isDirection(KeyCode k)
    {
        if (k == KeyCode.LeftArrow || k == KeyCode.RightArrow || k == KeyCode.UpArrow || k == KeyCode.DownArrow)
        {
            return true;
        }

        return false;
    }

    public bool isDirection(char c)
    {
        if (c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
        {
            return true;
        }

        return false;
    }

    /*
     * Helper function to check if a key is an attack key or not.
     */
    public bool isAttack(KeyCode k)
    {
        if (k == KeyCode.A || k == KeyCode.B || k == KeyCode.C || k == KeyCode.E)
        {
            return true;
        }

        return false;
    }

    public bool isAttack(char c)
    {
        if (c == 'A' || c == 'B' || c == 'C' || c == 'E' || c == 'S' || c == 'T')
        {
            return true;
        }

        return false;
    }
        
    //Update is called once per frame
	void Update ()
    {

        lastFrameMoveInput = currentMoveInput;
        lastFrameAttackInput = currentAttackInput;

        lastFrameControllerMoveInput = currentControllerMoveInput;
        lastFrameControllerAttackInput = currentControllerAttackInput;

        lastMove = currentMove;
        lastAttack = currentAttack;

        currentMoveInput = getAllCurrentMovementKeys();
        currentAttackInput = getAllCurrentAttackKeys();

        currentControllerMoveInput = getCurrentControllerDirectionInput();
        currentControllerAttackInput = getCurrentControllerAttackInput();


        if (currentControllerMoveInput != Vector2.zero)
        {
            if (isDirection(GameSettings.convertStuffToInput(currentControllerMoveInput)))
            {
                currentMove = GameSettings.convertStuffToInput(currentControllerMoveInput);
            }

            if (currentMove != lastMove)
            {
                ci.addToInputQueue(currentMove);
                GetComponent<InputDisplay>().displayInput(currentMove);
            }
        }
        else if (currentMoveInput.Count > 0)
        {
            if (isDirection(GameSettings.convertStuffToInput(currentMoveInput)))
            {
                currentMove = GameSettings.convertStuffToInput(currentMoveInput);
            }

            if (currentMove != lastMove)
            {
                ci.addToInputQueue(currentMove);
                GetComponent<InputDisplay>().displayInput(currentMove);
            }
        }

        else
        {
            currentMove = ' ';
        }


        if (currentControllerAttackInput.Count != 0)
        {
            if (isAttack(GameSettings.convertStuffToInput(currentControllerAttackInput)))
            {
                currentAttack = GameSettings.convertStuffToInput(currentControllerAttackInput);
            }

            if (currentAttack != lastAttack)
            {
                ci.addToInputQueue(currentAttack);
                GetComponent<InputDisplay>().displayInput(currentAttack);
            }
        }
        else if (currentAttackInput.Count > 0) 
        {
            currentAttack = GameSettings.convertStuffToInput (currentAttackInput);
            if (currentAttack != lastAttack)
            {
                ci.addToInputQueue(currentAttack);
                GetComponent<InputDisplay>().displayInput(currentAttack);
            }

        }
        else
        {
            currentAttack = ' ';
        }

        ci.interpretMoves ();
	}  
}
