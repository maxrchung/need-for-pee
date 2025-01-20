using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VNManager : MonoBehaviour
{
    public TextManagerScript textManager; 
    public float dialogueScale;
    public float choiceScale;
    public Canvas canvas;
    public GameObject dialoguePanel;
    public GameObject[] buttons;
    public int currentIndex = 0;
    VNTreeNode[] currentVNTree;
    public bool isPlaying = false;
    int currentPissText = 0;
    KeyValuePair<int,String>[] pissChoices;
    Vector2 dialoguePosition;
    Vector2[] choicePositions = new Vector2[3];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialoguePanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,canvas.GetComponent<RectTransform>().rect.width);
        dialoguePanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,canvas.GetComponent<RectTransform>().rect.height * 0.2f);
        dialoguePosition.x = canvas.GetComponent<RectTransform>().rect.width * 0.05f;
        dialoguePosition.y = canvas.GetComponent<RectTransform>().rect.height * 0.875f;
        for(int i = 0; i < 3; i++)
        {
            buttons[i].GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,
            canvas.GetComponent<RectTransform>().rect.height * (0.1f + 0.2f * i),canvas.GetComponent<RectTransform>().rect.height * 0.1f);
            choicePositions[i].y = canvas.GetComponent<RectTransform>().rect.height * (0.1f + 0.2f * i) + 10;
            choicePositions[i].x = canvas.GetComponent<RectTransform>().rect.width/2 - buttons[i].GetComponent<RectTransform>().rect.width/2 + 25;
        }
        dialoguePanel.SetActive(false);
        ClearButtons();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(!isPlaying)
            {
                //RunDialogueTree(TestCreate());
                //^ how to activate a dialogue tree you created
            }
            else
            {
                Next();
            }
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(!isPlaying)
            {
                RunDialogueTree(TestCreate());
            }
        }
        /*
        if(isPlaying && currentVNTree[currentIndex].choice)
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                SelectChoice(0);
            }
            if(Input.GetKeyDown(KeyCode.B))
            {
                SelectChoice(1);
            }
            if(Input.GetKeyDown(KeyCode.C))
            {
                SelectChoice(2);
            }
        }
        */
    }

    public void RunDialogueTree(VNTreeNode[] tree)
    {
        //begin selected dialogue tree
        currentVNTree = tree;
        isPlaying = true;
        currentIndex = 0;
        currentPissText = textManager.PissTextGeneration(currentVNTree[currentIndex].dialogue,dialoguePosition,dialogueScale,true);
        dialoguePanel.SetActive(true);
    }

    public void Next()
    {
        //goes to next entry in tree, will not work if current entry is a choice
        if(textManager.IsCompleted(currentPissText))
        {
            if(currentVNTree[currentIndex].lastMessage)
            {
                isPlaying = false;
                textManager.ClearText(currentPissText);
                dialoguePanel.SetActive(false);
            }
            else if(currentVNTree[currentIndex].choice == false)
            {
                currentIndex++;
                currentPissText = DisplayNextLine(currentIndex);
            }
        }
        else
        {
            textManager.CompleteVN(currentPissText);
        }
    }

    public void SelectChoice(int choice)
    {
        //for selecting the choice
        if(choice < currentVNTree[currentIndex].choices.Count)
        {
            ClearButtons();
            currentIndex = currentVNTree[currentIndex].choices[pissChoices[choice].Value];
            textManager.ClearText(currentPissText);
            foreach(KeyValuePair<int,string> kvp in pissChoices)
            {
                textManager.ClearText(kvp.Key);
            }
            //currentPissText = textManager.PissTextGeneration(currentVNTree[currentIndex].dialogue,dialoguePosition,dialogueScale,true);
            currentPissText = DisplayNextLine(currentIndex);
        }
    }

    int DisplayNextLine(int displayIndex)
    {
        if(currentVNTree[displayIndex].choice)
        {
            textManager.ClearText(currentPissText);
            return DisplayChoice();
        }
        else
        {
            textManager.ClearText(currentPissText);
            return textManager.PissTextGeneration(currentVNTree[displayIndex].dialogue,dialoguePosition,dialogueScale,true);
        }
    }

    int DisplayChoice()
    {
        DisplayButtons(currentVNTree[currentIndex].choices.Count);
        pissChoices = new KeyValuePair<int,string>[currentVNTree[currentIndex].choices.Count];
        int choiceCount = 0;
        foreach(KeyValuePair<string,int> kvp in currentVNTree[currentIndex].choices)
        {
            pissChoices[choiceCount]=new KeyValuePair<int,string>(textManager.PissTextGeneration(kvp.Key,choicePositions[choiceCount],choiceScale,false),kvp.Key);
            choiceCount++;
        }
        return textManager.PissTextGeneration(currentVNTree[currentIndex].dialogue,dialoguePosition,dialogueScale,true);
    }

    void DisplayButtons(int numChoices)
    {
        for(int i = 0; i < numChoices; i++)
        {
            buttons[i].SetActive(true);
        }
    }

    void ClearButtons()
    {
        foreach(GameObject g in buttons)
        {
            g.SetActive(false);
        }
    }

    VNTreeNode[] TestCreate()
    {
        //example of what a dialogue tree looks like
        //VNTreeNode("text you want here", true = terminates dialogue after this line/false = go to next immediate node
        //after this line/Dictionary = this is a choice dialogue)
        VNTreeNode[] testTree = new VNTreeNode[7];
        testTree[0] = new VNTreeNode("test0",false);
        Dictionary<string,int> choices0 = new Dictionary<string, int>();
        choices0["a"] = 2;
        choices0["b"] = 3;
        choices0["c"] = 4;
        Dictionary<string,int> choices1 = new Dictionary<string, int>();
        choices1["d"] = 5;
        choices1["e"] = 6;
        testTree[1] = new VNTreeNode("choice?",choices0);
        testTree[2] = new VNTreeNode("testa",true);
        testTree[3] = new VNTreeNode("testb",true);
        testTree[4] = new VNTreeNode("testc?",choices1);
        testTree[5] = new VNTreeNode("testd",true);
        testTree[6] = new VNTreeNode("teste",true);

        return testTree;
    }
}
