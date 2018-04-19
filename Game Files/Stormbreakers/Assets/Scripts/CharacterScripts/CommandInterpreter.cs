/*
 * Command Interpreter used to interpret commands. Proccesses special
 * motion inputs, dash inputs, etc.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterAttacks))]
[RequireComponent(typeof(ElementAttacks))]
[RequireComponent(typeof(PlayerSettings))]
[RequireComponent(typeof(CharacterState))]
[RequireComponent(typeof(ElementAttacks))]
[RequireComponent(typeof(InputDisplay))]
[RequireComponent(typeof(Animator))]

public class CommandInterpreter : MonoBehaviour 
{

    /*
     * Queue for all the inputs the user has given in the past 20 frames
     */
    private InputQueue inputQueue;
    private CharacterMovement cm;
    private CharacterAttacks ca;
    private PlayerSettings ps;
    private CharacterState cs;
    private ElementAttacks ea;
    private InputDisplay id;
    private Animator anim;


    public GameObject inputbox;

    private const int inputLeniencyValue = 5; //The higher this is, the more lenient the inputs are, but having it too high will
    //mess stuff up. Programmatically, it's the number of inputs checked when seeing if a motion is in the queue or not
    private const int framesToQueueClear = 5; //How many frames after an attack input should the queue clear?

    private bool vAxisIsHeld;
    private bool displayInput;
    public bool charging;
    public bool isInterpretingInputs;
    public bool crouching;

    private char elementChoice;

    private float deadZone = 0.5f;

    private GameObject superFlashPrefab;
    private GameObject superFlash;


    public class InputQueue
    {
        //-------------------VARIABLES------------------//
        private Queue<char> inputQueue;
        private Queue<int> frameReferenceQueue;

        private char lastDirectionInput = ' ';

        private const int maxNumberOfFrames = 30; //Maximum number of frames before old inputs get deleted. Since the game's running at 
        //60FPS, 30 frames is half of a second.



        //-------------------FUNCTIONS------------------//

        // Empty constructor
        public InputQueue()
        {
            inputQueue = new Queue<char>();
            frameReferenceQueue = new Queue<int>();
        }

        // Constructor for when a queue of inputs is passed in.
        // Effectively just adds the maxNumberOfFrames parameter to an already existing queue
        public InputQueue(Queue<char> q)
        {
            inputQueue = q;
            frameReferenceQueue = new Queue<int>();

            foreach (char c in inputQueue)
            {
                frameReferenceQueue.Enqueue(Time.frameCount);
            }
        }

        public InputQueue(List<char> l)
        {
            inputQueue = new Queue<char>();
            frameReferenceQueue = new Queue<int>();
            foreach(char i in l)
            {
                inputQueue.Enqueue(i);
            }

            foreach (char c in inputQueue)
            {
                frameReferenceQueue.Enqueue(Time.frameCount);
            }
        }

        // Method to add an input to the queue. Works with inputs being passed.
        public void addToInputQueue(char c)
        {
            deleteOldInputs();
            inputQueue.Enqueue(c);
            frameReferenceQueue.Enqueue(Time.frameCount);

            //char[] queue = inputQueue.getInputQueue ().ToArray();
             
        }

        // Returns the inputQueue when asked
        public Queue<char> getInputQueue()
        {
            deleteOldInputs();
            return inputQueue;
        }

        public char getLastDirectionInput()
        {
            return lastDirectionInput;
        }

        public Queue<int>getFrameReferenceQueue()
        {
            deleteOldInputs();
            return frameReferenceQueue;
        }

        // I feel like a lot of these comments are just repeating the name of the function
        public void deleteOldInputs()
        {
            if (inputQueue.Count != 0)
            {
                char[] tempArray = inputQueue.ToArray();
                if (tempArray[tempArray.Length - 1] == '4' || tempArray[tempArray.Length - 1] == '6')
                {
                    lastDirectionInput = tempArray[tempArray.Length - 1];
                }
            }


            while(inputQueue.Count > 0 && frameReferenceQueue.Count > 0 && (Time.frameCount - frameReferenceQueue.Peek() > maxNumberOfFrames))
            {
                inputQueue.Dequeue();
                frameReferenceQueue.Dequeue();
            }
        
        }

        public void printInputQueue()
        {
            foreach (char c in inputQueue)
            {
                Debug.Log(c);
            }
            Debug.Log("---------------------------------------------------------");
        }

        private bool isAttack(char c)
        {
            if (c == 'A' || c == 'B' || c == 'C' || c == 'E' || c == 'S' || c == 'T')
            {
                return true;
            }

            return false;
        }

        private bool hasAttacks()
        {
            foreach (char c in inputQueue)
            {
                if (isAttack(c))
                {
                    return true;
                }
            }

            return false;
        }

        public void clearInputQueue()
        {
            while (inputQueue.Count > 0 || frameReferenceQueue.Count > 0)
            {
                if (inputQueue.Count > 0)
                {
                    inputQueue.Dequeue();
                }

                if (frameReferenceQueue.Count > 0)
                {
                    frameReferenceQueue.Dequeue();
                }
            }
        }

        public void clearAttacksFromInputQueue()
        {
            Debug.Log("This is getting called");

            InputQueue tempQueue = new InputQueue();

            foreach(char c in inputQueue)
            {
                if (!isAttack(c))
                {
                    tempQueue.addToInputQueue(c);
                }

                inputQueue.Dequeue();
                frameReferenceQueue.Dequeue();
            }

            inputQueue = tempQueue.inputQueue;
            frameReferenceQueue = tempQueue.frameReferenceQueue;
        }
    }

    // Use this for initialization
    void Start ()  
    {
        if (GetComponent<CharacterMovement>())
        {
            cm = GetComponent<CharacterMovement>(); 
        }
        if (GetComponent<CharacterAttacks>())
        {
            ca = GetComponent<CharacterAttacks>();
        }
        if (GetComponent<PlayerSettings>())
        {
            ps = GetComponent<PlayerSettings>();
        }
        if (GetComponent<CharacterState>())
        {
            cs = GetComponent<CharacterState>();
        }
        if (GetComponent<ElementAttacks>())
        {
            ea = GetComponent<ElementAttacks>();
        }
        if(GetComponent<Animator>())
        {
            anim = GetComponent<Animator>();
        }

        id = GetComponent<InputDisplay>();

        if (GameObject.Find("CharacterChoices"))
        {
            if (ps.getPlayerNumber() == '1')
            {
                elementChoice = GameObject.Find("CharacterChoices").GetComponent<CharacterChoices>().getP1Element();
            }
            else if (ps.getPlayerNumber() == '2')
            {
                elementChoice = GameObject.Find("CharacterChoices").GetComponent<CharacterChoices>().getP2Element();
            }
        }
        else
        {
            Debug.Log(gameObject.name + ps.getPlayerNumber()); //IF NO CHARACTER CHOICES -- for testing
            if (ps.getPlayerNumber() == '1')
            {
                elementChoice = 'l';
            }
            else if (ps.getPlayerNumber() == '2')
            {
                elementChoice = 'd';
            }
        }

        inputQueue = new InputQueue();
        isInterpretingInputs = true;
        vAxisIsHeld = false;

        superFlashPrefab = Resources.Load("MiscPrefabs/SuperFlash", typeof(GameObject)) as GameObject;
    }


    /*public void clearAttacksFromInputQueue()
    {
        if (inputQueue.getInputQueue().Count != 0)
        {
            char[] tempInputArray = inputQueue.getInputQueue().ToArray();
            int[] tempFrameReferenceArray = inputQueue.getFrameReferenceQueue().ToArray();


            while (inputQueue.getInputQueue().Count != 0 && inputQueue.getFrameReferenceQueue().Count != 0)
            {
                inputQueue.getInputQueue().Dequeue();
                inputQueue.getFrameReferenceQueue().Dequeue();
            }

            for (int i = tempInputArray.Length-1; i >= 0; i--)
            {
                if (!GetComponent<InputGatherer>().isAttack(tempInputArray[i]))
                {
                    inputQueue.getInputQueue().Enqueue(tempInputArray[i]);
                    inputQueue.getFrameReferenceQueue().Enqueue(tempFrameReferenceArray[i]);
                }
            }
        }
    }*/

    public char getElementChoice()
    {
        return elementChoice;
    }

    public IEnumerator clearInputQueue()
    {
        for (int i = 0; i < framesToQueueClear; i++)
        {
            yield return new WaitForEndOfFrame();
        }

        inputQueue.clearInputQueue();
    }


    public IEnumerator clearAttacksFromInputQueue()
    {
        for (int i = 0; i < framesToQueueClear; i++)
        {
            yield return new WaitForEndOfFrame();
        }

        inputQueue.clearAttacksFromInputQueue();
    }

    public bool has6()
    {
        if (cs.isOnLeft())
        {
            if (Input.GetAxis("P" + ps.getPlayerNumber() + "Horizontal") > 0.95f || Input.GetAxis("P" + ps.getPlayerNumber() + "DPadHorizontal") > 0.4f)
            {
                return true;
            }
        }
        else
        {
            if (Input.GetAxis("P" + ps.getPlayerNumber() + "Horizontal") < -0.95f || Input.GetAxis("P" + ps.getPlayerNumber() + "DPadHorizontal") < -0.4f)
            {
                return true;
            }
        }

        return false;
    }

    public bool has236()
    {
        char[] queue = inputQueue.getInputQueue().ToArray();

        if (cs.isOnLeft())
        {
            if (queue.Length - inputLeniencyValue >= 0)
            {
                for (int i = queue.Length - inputLeniencyValue; i < queue.Length; i++)
                {
                    if (queue[i] == '2')
                    {
                        for (int x = i; x < queue.Length; x++)
                        {
                            if (queue[x] == '3')
                            {
                                for (int n = x; n < queue.Length; n++)
                                {
                                    if (queue[n] == '6')
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < queue.Length; i++)
                {
                    if (queue[i] == '2')
                    {
                        for (int x = i; x < queue.Length; x++)
                        {
                            if (queue[x] == '3')
                            {
                                for (int n = x; n < queue.Length; n++)
                                {
                                    if (queue[n] == '6')
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }
        else if (!cs.isOnLeft())
        {
            if (queue.Length - inputLeniencyValue >= 0)
            {
                for (int i = queue.Length - inputLeniencyValue; i < queue.Length; i++)
                {
                    if (queue[i] == '2')
                    {
                        for (int x = i; x < queue.Length; x++)
                        {
                            if (queue[x] == '1')
                            {
                                for (int n = x; n < queue.Length; n++)
                                {
                                    if (queue[n] == '4')
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < queue.Length; i++)
                {
                    if (queue[i] == '2')
                    {
                        for (int x = i; x < queue.Length; x++)
                        {
                            if (queue[x] == '1')
                            {
                                for (int n = x; n < queue.Length; n++)
                                {
                                    if (queue[n] == '4')
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        return false;
    }

    public bool hasSuperInput()
    {
        if (has236())
        {
            char[] queue = inputQueue.getInputQueue().ToArray();
            int len = queue.Length;

            if (inputQueue.getInputQueue().Contains('S') || inputQueue.getInputQueue().Contains('T'))
            {
                return true;
            }
        }

        return false;
    }

    public bool has214()
    {
        char[] queue = inputQueue.getInputQueue().ToArray();

        if (cs.isOnLeft())
        {
            if (queue.Length - inputLeniencyValue >= 0)
            {
                for (int i = queue.Length - inputLeniencyValue; i < queue.Length - 2; i++)
                {

                    if (queue[i] == '2')
                    {
                        for (int x = i; x < queue.Length - 1; x++)
                        {
                            if (queue[x] == '1')
                            {
                                for (int n = x; n < queue.Length; n++)
                                {
                                    if (queue[n] == '4')
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                    
                }
            }
            else
            {
                for (int i = 0; i < queue.Length - 2; i++)
                {
                    
                    if (queue[i] == '2')
                    {
                        for (int x = i; x < queue.Length - 1; x++)
                        {
                            if (queue[x] == '1')
                            {
                                for (int n = x; n < queue.Length; n++)
                                {
                                    if (queue[n] == '4')
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                    
                }
            }

        }
        else
        {
            if (queue.Length - inputLeniencyValue >= 0)
            {
                for (int i = queue.Length - inputLeniencyValue; i < queue.Length - 2; i++)
                {
                   
                    if (queue[i] == '2')
                    {
                        for (int x = i; x < queue.Length - 1; x++)
                        {
                            if (queue[x] == '3')
                            {
                                for (int n = x; n < queue.Length; n++)
                                {
                                    if (queue[n] == '6')
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                    
                }
            }
            else
            {
                for (int i = 0; i < queue.Length - 2; i++)
                {
                    if (queue[i] == '2')
                    {
                        for (int x = i; x < queue.Length - 1; x++)
                        {
                            if (queue[x] == '3')
                            {
                                for (int n = x; n < queue.Length; n++)
                                {
                                    if (queue[n] == '6')
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    public bool has623()
    {
        char[] queue = inputQueue.getInputQueue().ToArray();

        if (cs.isOnLeft())
        {
            if (queue.Length - inputLeniencyValue >= 0)
            {
                for (int i = queue.Length - inputLeniencyValue; i < queue.Length - 2; i++)
                {

                    if (queue[i] == '6')
                    {
                        for (int x = i; x < queue.Length - 1; x++)
                        {
                            if (queue[x] == '2')
                            {
                                for (int n = x; n < queue.Length; n++)
                                {
                                    if (queue[n] == '3')
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }

                }
            }
            else
            {
                for (int i = 0; i < queue.Length - 2; i++)
                {

                    if (queue[i] == '6')
                    {
                        for (int x = i; x < queue.Length - 1; x++)
                        {
                            if (queue[x] == '2')
                            {
                                for (int n = x; n < queue.Length; n++)
                                {
                                    if (queue[n] == '3')
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }

                }
            }

        }
        else
        {
            if (queue.Length - inputLeniencyValue >= 0)
            {
                for (int i = queue.Length - inputLeniencyValue; i < queue.Length - 2; i++)
                {

                    if (queue[i] == '4')
                    {
                        for (int x = i; x < queue.Length - 1; x++)
                        {
                            if (queue[x] == '2')
                            {
                                for (int n = x; n < queue.Length; n++)
                                {
                                    if (queue[n] == '1')
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }

                }
            }
            else
            {
                for (int i = 0; i < queue.Length - 2; i++)
                {
                    if (queue[i] == '4')
                    {
                        for (int x = i; x < queue.Length - 1; x++)
                        {
                            if (queue[x] == '2')
                            {
                                for (int n = x; n < queue.Length; n++)
                                {
                                    if (queue[n] == '1')
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    public bool has41236() //Still need to fix this
    {

        char[] queue = inputQueue.getInputQueue ().ToArray();
        for (int i = 0; i < queue.Length - 4; i++)
        {
            if (cs.isOnLeft())
            {
                if (queue[i] == '4' && queue[i + 1] == '1' && queue[i + 2] == '2' && queue[i + 3] == '3' && queue[i + 4] == '6')
                {
                    Debug.Log("41236!");
                    return true;
                }
            }
            else if (!cs.isOnLeft())
            {
                if (queue[i] == '6' && queue[i + 1] == '3' && queue[i + 2] == '2' && queue[i + 3] == '1' && queue[i + 4] == '4')
                {
                    Debug.Log("41236!");
                    return true;
                }
            }
        }
        return false;

    }

    public bool hasUpInput()
    {
        return (inputQueue != null && (inputQueue.getInputQueue().Contains('7') || inputQueue.getInputQueue().Contains('8') || inputQueue.getInputQueue().Contains('9')));
    }

    public bool hasDash()
    {
        //inputQueue.deleteOldInputs();
        char[] queue = inputQueue.getInputQueue().ToArray();
        for (int i = 0; i < queue.Length - 1; i++)
        {
            if (queue[i] == queue[i + 1] && (queue[i] == '4' || queue[i] == '6'))
            {
                return true;
            }
        }
        return false;
    }

    public bool isJumping()
    {
        //inputQueue.deleteOldInputs();


        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("P" + ps.getPlayerNumber() + "Vertical") < 0 || Input.GetAxis("P" + ps.getPlayerNumber() + "DPadVertical") > 0)
        {
            if (!vAxisIsHeld)
            {
                vAxisIsHeld = true;
                return true;
            }
            else if (!cm.isinAir())
            {
                return true;
            }
        }
        else
        {
            vAxisIsHeld = false;
            return false;
        }

        return false;
    }

    public bool isCrouching()
    {
        if (!cm.isinAir())
        {
            return Input.GetKey(KeyCode.DownArrow) || (Input.GetAxis("P" + ps.getPlayerNumber() + "Vertical") > 0 || Input.GetAxis("P" + ps.getPlayerNumber() + "DPadVertical") < 0);
        }

        return false;
    }
   

    public bool isWalking()
    {
        //inputQueue.deleteOldInputs();
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)) || Input.GetAxis("P" + ps.getPlayerNumber() + "Horizontal") != 0 || Input.GetAxis("P" + ps.getPlayerNumber() + "DPadHorizontal") != 0)
        {
            if ((Input.GetKey(KeyCode.DownArrow) && Input.GetAxis("P" + ps.getPlayerNumber() + "Vertical") == 0 || Input.GetAxis("P" + ps.getPlayerNumber() + "DPadVertical") == 0))
            {
                //Debug.Log(gameObject.name);
                return true;
            }
        }
        return false;
    }

    private bool checkCombinationSuper()
    {
        return true;
    }

    //This function will probably be either ridiculously huge
    //or just be a bunch of other function calls.
    public void interpretMoves()
    {

        inputQueue.deleteOldInputs();
        if (cm.isHit || anim.GetCurrentAnimatorStateInfo(0).IsName("hit") || !isInterpretingInputs)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("idle") && cm.canMove && !cm.isinAir())
            {
                isInterpretingInputs = true;
            }

            StartCoroutine("clearInputQueue");
        }
        /*if (has236() && has2B()) //Combination Supers
        {
            
        }*/
        else if (cm.canMove || isCrouching())
        {
            
            if (gameObject.name.Contains("2"))
            {
                
                //Debug.Log(Time.frameCount);
            }
            if (hasSuperInput() && (cs.special >= 100 || cs.specialLevel > 0) && !cm.isinAir() && !isJumping() && superFlash == null && !ea.superPrefabActive()) //Element specific super
            {
                if (elementChoice == 'l')
                {
                    if (superFlash == null)
                    {
                        superFlash = Instantiate(superFlashPrefab, transform.position, transform.rotation, gameObject.transform);
                    }

                    if (gameObject.name.Contains("L"))
                    {
                        anim.Play("LSuper");
                    }
                    else
                    {
                        ea.LSuper();
                    }

                }
                else if (elementChoice == 'd')
                {
                    if (superFlash == null)
                    {
                        superFlash = Instantiate(superFlashPrefab, transform.position, transform.rotation, gameObject.transform);
                    }

                    if (gameObject.name.Contains("L"))
                    {
                        anim.Play("DSuper");
                    }
                    else
                    {
                        ea.DSuper();
                    }
                }
                else if (elementChoice == 's')
                {
                    if (superFlash == null)
                    {
                        superFlash = Instantiate(superFlashPrefab, transform.position, transform.rotation, gameObject.transform);
                    }

                    if (gameObject.name.Contains("L"))
                    {
                        anim.Play("SSuper");
                    }
                    else
                    {
                        ea.SSuper();
                    }

                }
                else if (elementChoice == 't')
                {
                    if (superFlash == null)
                    {
                        superFlash = Instantiate(superFlashPrefab, transform.position, transform.rotation, gameObject.transform);
                    }
                    ea.TSuper();
                }

                StartCoroutine("clearInputQueue");
            }
            //       else if (has236() && (A+B || B+C || A+C)) // Character specific super

            else if ((has623()) && inputQueue.getInputQueue().Contains('C') && !GetComponent<CharacterMovement>().isPlayingImmobileAnimation() && !cm.isinAir() && !isJumping())
            {
                isInterpretingInputs = false;
                anim.Play("623C");

                StartCoroutine("clearInputQueue");
            }
            else if ((has623()) && inputQueue.getInputQueue().Contains('B') && !GetComponent<CharacterMovement>().isPlayingImmobileAnimation() && !cm.isinAir() && !isJumping())
            {
                isInterpretingInputs = false;
                anim.Play("623B");

                StartCoroutine("clearInputQueue");
            }
            else if ((has623()) && inputQueue.getInputQueue().Contains('A') && !GetComponent<CharacterMovement>().isPlayingImmobileAnimation() && !cm.isinAir() && !isJumping())
            {
                isInterpretingInputs = false;
                anim.Play("623A");

                StartCoroutine("clearInputQueue");
            }
            else if (((has236() && !ea.special236PrefabActive()) || (has214() && !ea.special214PrefabActive())) && inputQueue.getInputQueue().Contains('E') && !cm.isinAir() && !isJumping()) // special attacks
            {
                isInterpretingInputs = false;
                Debug.Log("236/214E");
                if (has236())
                {
                    if (elementChoice == 'l')
                    {
                        anim.Play("L236E");
                    }
                    else if (elementChoice == 'd')
                    {
                        if (gameObject.name.Contains("L"))
                        {
                            anim.Play("D236E");
                        }
                        else
                        {
                            ea.D236E();
                        }
                    }
                    else if (elementChoice == 's')
                    {
                        ea.S236E();
                    }
                    else if (elementChoice == 't')
                    {
                        ea.T236E();
                    }
                }
                else if (has214())
                {
                    if (elementChoice == 'l')
                    {
                        ea.L214E();
                    }
                    else if (elementChoice == 'd')
                    {
                        if (gameObject.name.Contains("L"))
                        {
                            anim.Play("D214E");
                        }
                        else
                        {
                            ea.D214E();
                        }
                    }
                    else if (elementChoice == 's')
                    {
                        ea.S214E();
                    }
                    else if (elementChoice == 't')
                    {
                        if (gameObject.name.Contains("L"))
                        {
                            anim.Play("T214E");
                        }
                        else
                        {
                            ea.D214E();
                        }
                    }
                }

                StartCoroutine("clearInputQueue");
            }
            else if ((has236() || has214()) && inputQueue.getInputQueue().Contains('C') && !cm.isinAir() && !isJumping() && ca.fb == null && (!cm.isPlayingImmobileAnimation())) // special attacks
            {
                isInterpretingInputs = false;
                if (has236())
                {
                    anim.Play("236C");
                }
                else if (has214())
                {
                    ca.special214C();
                }

                StartCoroutine("clearInputQueue");
            }
            else if ((has236() || has214()) && inputQueue.getInputQueue().Contains('B') && !cm.isinAir() && !isJumping() && ca.fb == null && !cm.isPlayingImmobileAnimation()) // special attacks
            {
                isInterpretingInputs = false;
                if (has236())
                {
                    anim.Play("236B");
                }
                else if (has214())
                {
                    ca.special214B();
                }
                StartCoroutine("clearInputQueue"); 
            }
            else if ((has236() || has214()) && inputQueue.getInputQueue().Contains('A') && !cm.isinAir() && !isJumping() && ca.fb == null && (!cm.isPlayingImmobileAnimation())) // special attacks
            {
                isInterpretingInputs = false;
                if (has236())
                {
                    anim.Play("236A");
                }
                else if (has214())
                {
                    ca.special214A();
                }
                StartCoroutine("clearInputQueue");
            }
            else if (inputQueue.getInputQueue().Contains('T') && !(hasSuperInput() && (cs.special >= 100 || cs.specialLevel > 0) && !cm.isinAir() && !isJumping())) //throws
            {
                isInterpretingInputs = false;
                ca.nThrow();
                StartCoroutine("clearInputQueue");
            }
            else if (inputQueue.getInputQueue().Contains('E'))  //Element attacks
            {
                if (isCrouching())
                {
                    ca.twoE();
                }
                else if (cm.isinAir())
                {
                    ca.jE();
                }
                else if (cm.canMove && !ca.isAttacking())
                {
                    //ca.fiveE();
                    Debug.Log("Charge!");
                    charging = true;
                }
                StartCoroutine("clearInputQueue");
            }
            else if (inputQueue.getInputQueue().Contains('C')) //Heavy attacks
            {
                
                if (isCrouching())
                {
                    ca.twoC();
                }
                else if (cm.isinAir() || isJumping())
                {
                    ca.jC();
                }
                else
                {
                    ca.fiveC();
                }
                StartCoroutine("clearInputQueue");
            }
            else if (inputQueue.getInputQueue().Contains('B'))  //Medium attacks
            {
                if (isCrouching())
                {
                    ca.twoB();
                }
                else if (cm.isinAir() || isJumping())
                {
                    ca.jB();
                }
                else if (has6() && cm.dashDirection == '5')
                {
                    ca.sixB();
                }
                else
                {
                    ca.fiveB();
                }
                StartCoroutine("clearInputQueue");
            }
            else if (inputQueue.getInputQueue().Contains('A')) //Light attacks
            {
                if (isCrouching())
                {
                    ca.twoA();
                }
                else if (cm.isinAir()  || isJumping())
                {
                    ca.jA();
                }
                else
                {
                    ca.fiveA();
                }
                StartCoroutine("clearInputQueue");
            }
            else if (isJumping())
            {
                cm.jump();
            }
            /*else if (isCrouching())
        {
            cs.crouch();
        }*/
            else if (hasDash())
            {
                char[] queue = inputQueue.getInputQueue().ToArray();
                int i = queue.Length - 2;
                if (queue[i] == queue[i + 1] && (queue[i] == '4' || queue[i] == '6'))
                {
                    cm.dash(queue[i]);
                }
            }
            else if (isWalking())
            {
                if ((Input.GetKey(KeyCode.RightArrow) ||
                    Input.GetAxis("P" + ps.getPlayerNumber() + "Horizontal") > deadZone ||
                    Input.GetAxis("P" + ps.getPlayerNumber() + "DPadHorizontal") > deadZone))
                {
                    cm.walk('6');
                }
                else if (Input.GetKey(KeyCode.LeftArrow) ||
                         Input.GetAxis("P" + ps.getPlayerNumber() + "Horizontal") < -deadZone ||
                         Input.GetAxis("P" + ps.getPlayerNumber() + "DPadHorizontal") < -deadZone)
                {
                    cm.walk('4');
                }

                if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
                {
                    cm.walk('5');
                }
            }
            else
            {
                cm.walk('5');
            }
        }
        else if (ca.hasHit) //START FROM HERE ON WEDNESDAY
        {
            
            if (hasSuperInput() && (cs.special >= 100 || cs.specialLevel > 0) && !cm.isinAir() && !isJumping() && ca.canCancel("super")) //Element specific super
            {
                Debug.Log("has Super Input");
                if (elementChoice == 'l')
                {
                    if (superFlash == null)
                    {
                        superFlash = Instantiate(superFlashPrefab, transform.position, transform.rotation, gameObject.transform);
                    }

                    if (gameObject.name.Contains("L"))
                    {
                        anim.Play("LSuper");
                    }
                    else
                    {
                        ea.LSuper();
                    }

                }
                else if (elementChoice == 'd')
                {
                    if (superFlash == null)
                    {
                        superFlash = Instantiate(superFlashPrefab, transform.position, transform.rotation, gameObject.transform);
                    }

                    if (gameObject.name.Contains("L"))
                    {
                        anim.Play("DSuper");
                    }
                    else
                    {
                        ea.DSuper();
                    }
                }
                else if (elementChoice == 's')
                {
                    if (superFlash == null)
                    {
                        superFlash = Instantiate(superFlashPrefab, transform.position, transform.rotation, gameObject.transform);
                    }

                    if (gameObject.name.Contains("L"))
                    {
                        anim.Play("SSuper");
                    }
                    else
                    {
                        ea.SSuper();
                    }

                }
                else if (elementChoice == 't')
                {
                    if (superFlash == null)
                    {
                        superFlash = Instantiate(superFlashPrefab, transform.position, transform.rotation, gameObject.transform);
                    }
                    ea.TSuper();
                }

                StartCoroutine("clearInputQueue");

            }
            else if (has236() && inputQueue.getInputQueue().Contains('E') && !cm.isinAir() && !isJumping() && ca.fb == null && (!cm.isPlayingImmobileAnimation() || ca.canCancel("236C")))
            {
                Debug.Log("Has 236E");
                anim.Play("236C");
            }
            else if (has214() && inputQueue.getInputQueue().Contains('E') && !cm.isinAir() && !isJumping() && ca.fb == null && (!cm.isPlayingImmobileAnimation() || ca.canCancel("236C")))
            {
                Debug.Log("Has 214E");
                anim.Play("236C");
            }
            else if (has236() && inputQueue.getInputQueue().Contains('C') && !cm.isinAir() && !isJumping() && ca.fb == null && ca.canCancel("236C"))
            {
                Debug.Log("Test");
                anim.Play("236C");
            }
            else if (has214() && inputQueue.getInputQueue().Contains('C') && !cm.isinAir() && !isJumping() && ca.fb == null && (!cm.isPlayingImmobileAnimation() || ca.canCancel("236C")))
            {
                ca.special214C();
            }
            else if (has623() && inputQueue.getInputQueue().Contains('C') && !cm.isinAir() && !isJumping() && ca.fb == null && (!cm.isPlayingImmobileAnimation() || ca.canCancel("236C")))
            {
                anim.Play("236C");
            }
            else if (has236() && inputQueue.getInputQueue().Contains('B') && !cm.isinAir() && !isJumping() && ca.fb == null && (!cm.isPlayingImmobileAnimation() || ca.canCancel("236C")))
            {
                anim.Play("236C");
            }
            else if (has214() && inputQueue.getInputQueue().Contains('B') && !cm.isinAir() && !isJumping() && ca.fb == null && (!cm.isPlayingImmobileAnimation() || ca.canCancel("236C")))
            {
                ca.special214B();
            }
            else if (has623() && inputQueue.getInputQueue().Contains('B') && !cm.isinAir() && !isJumping() && ca.fb == null && (!cm.isPlayingImmobileAnimation() || ca.canCancel("236C")))
            {
                anim.Play("236C");
            }
            else if (has236() && inputQueue.getInputQueue().Contains('A') && !cm.isinAir() && !isJumping() && ca.fb == null && (!cm.isPlayingImmobileAnimation() || ca.canCancel("236C")))
            {
                anim.Play("236C");
            }
            else if (has214() && inputQueue.getInputQueue().Contains('A') && !cm.isinAir() && !isJumping() && ca.fb == null && (!cm.isPlayingImmobileAnimation() || ca.canCancel("236C")))
            {
                ca.special214A();
            }
            else if (has623() && inputQueue.getInputQueue().Contains('A') && !cm.isinAir() && !isJumping() && ca.fb == null && (!cm.isPlayingImmobileAnimation() || ca.canCancel("236C")))
            {
                anim.Play("236C");
            }
                
            else if (inputQueue.getInputQueue().Contains('C')) //Heavy attacks
            {
                if (isCrouching())
                {
                    ca.twoC();
                }
                else if (cm.isinAir())
                {
                    ca.jC();
                }
                else
                {
                    ca.fiveC();
                }
                StartCoroutine("clearInputQueue");
            }
            else if (inputQueue.getInputQueue().Contains('B'))  //Medium attacks
            {
                if (isCrouching())
                {
                    ca.twoB();
                }
                else if (cm.isinAir())
                {
                    ca.jB();
                }
                else
                {
                    ca.fiveB();
                }
                StartCoroutine("clearInputQueue");
            }
            else if (inputQueue.getInputQueue().Contains('A')) //Light attacks
            {
                if (isCrouching())
                {
                    ca.twoA();
                }
                else if (cm.isinAir())
                {
                    ca.jA();
                }
                else
                {
                    ca.fiveA();
                }
                StartCoroutine("clearInputQueue");
            }
        }

        else
        {
            cm.walk('5');
        }
    }

    public void addToInputQueue(char c)
    {
        inputQueue.addToInputQueue(c);
    }

    // Update is called once per frame
    void Update ()
    {
        if(Input.GetButtonDown("P1LB") || Input.GetButtonDown("P2LB"))
        {
            inputQueue.printInputQueue();
            if (!displayInput)
            {
                displayInput = true;
                inputbox.SetActive(true);
            }
            else
            {
                displayInput = false;
                inputbox.SetActive(false);
            }
        }

        if (displayInput)
        {
            /*char[] queue = inputQueue.getInputQueue ().ToArray();
            id.displayInput(ps.getPlayerNumber(), queue); */
        }

        if (isCrouching() && !anim.GetCurrentAnimatorStateInfo(0).IsName("knockedDown") && !anim.GetCurrentAnimatorStateInfo(0).IsName("knockdownDelay")
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("gettingUp") /* && (!ca.isAttacking() || ca.hasHit)*/)
        {
            if (!ca.isAttacking())
            {
                ca.crouch();
            }

            cm.setCanMove(false);
            crouching = true;
            //Debug.Log(crouching);
        }
        else if(!isCrouching() && crouching && !ca.isAttacking() && !anim.GetCurrentAnimatorStateInfo(0).IsName("knockedDown") && !anim.GetCurrentAnimatorStateInfo(0).IsName("knockdownDelay")
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("gettingUp")/*anim.GetCurrentAnimatorStateInfo(0).IsTag("CrouchAttack")*/)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("charge") )
            {
                anim.Play("stand");
            }

            cm.setCanMove(true);
            crouching = false;
            //Debug.Log(crouching);
        }

        if (charging && !anim.GetCurrentAnimatorStateInfo(0).IsName("hit"))
        {
            ca.fiveE();
			cm.setCanMove (false);
        }
        if (Input.GetButtonUp("P" + ps.getPlayerNumber() + "E"))
        {
            charging = false;
            ca.frames = 0;
            cm.setCanMove(true);
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("hit"))
        {
            charging = false;
            ca.frames = 0;
        }

        if (cs.healthBar.value < 0)
        {
            anim.Play("death");
        }
    
        if ((has236() || has214()) && inputQueue.getInputQueue().Contains('C') && !cm.isinAir() && !isJumping() && ca.fb == null && (!cm.isPlayingImmobileAnimation() || ca.canCancel("236C")))
        {
            Debug.Log("Throwing Fireball!" + Time.frameCount);
        }

    }

   
}