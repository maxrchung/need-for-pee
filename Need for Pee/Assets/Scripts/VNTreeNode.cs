using System.Collections.Generic;
using UnityEngine;

public class VNTreeNode
{
    public bool choice;
    public string dialogue;
    public Dictionary<string,int> choices;
    public bool lastMessage;

    public VNTreeNode()
    {
        choice = false;
        dialogue = "";
        lastMessage = false;
    }

    public VNTreeNode(string d, bool last)
    {
        //create a normal dialogue
        choice = false;
        dialogue = d;
        lastMessage = last;
    }

    public VNTreeNode(string d, Dictionary<string,int> cs)
    {
        //create a choice
        choice = true;
        dialogue = d;
        choices = cs;
        lastMessage = false;
    }
}
