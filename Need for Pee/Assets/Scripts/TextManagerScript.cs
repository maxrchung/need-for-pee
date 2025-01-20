using GLTFast.Schema;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Rendering;
using UI = UnityEngine.UI;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;

public class TextManagerScript : MonoBehaviour
{
    public Texture2D[] letters;
    public Texture2D[] numbers;
    public Texture2D[] punctuation;
    public float defaultTextSpeed = 0.1f;//time between next glyph appearing
    public PissText[] textArray = new PissText[10];
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < textArray.Length; i++)
        {
            textArray[i].active = false;
        }
        //PissTextGeneration("@!?.,#/\"a\"", new Vector2(10,10),0.25f,true);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < textArray.Length; i++)
        {
            if(textArray[i].active)
            {
                if(textArray[i].vn)
                {
                    if(textArray[i].currentGlyph < textArray[i].glyphs.Length)
                    {
                        textArray[i].vnTimer += Time.deltaTime;
                        if(textArray[i].vnTimer > textArray[i].textSpeed)
                        {
                            textArray[i].currentGlyph += 1;
                            textArray[i].vnTimer = 0f;
                        }
                    }
                    else
                    {
                        textArray[i].completed = true;
                    }
                }
            }
        }
    }

    public int PissTextGeneration(string s, Vector2 position, float scale, bool vn)
    {
        PissText newPiss = new PissText();
        newPiss.text = s;
        newPiss.position = position;
        newPiss.scale = scale;
        newPiss.vn = vn;
        newPiss.textSpeed = defaultTextSpeed;
        newPiss.glyphs = (s.ToUpper()).ToIntArray();
        if(newPiss.vn)
        {
            newPiss.currentGlyph = 0;
            newPiss.vnTimer = 0;
            newPiss.completed = false;
        }
        else
        {
            newPiss.currentGlyph = newPiss.glyphs.Length;
            newPiss.completed = true;
        }
        newPiss.active = true;
        return AddToTextArray(newPiss);
    }

    public int PissTextGeneration(string s, Vector2 position, float scale, bool vn, float speed)
    {
        //alternate constructor that allows you to add textspeed
        PissText newPiss = new PissText();
        newPiss.text = s;
        newPiss.position = position;
        newPiss.scale = scale;
        newPiss.vn = vn;
        newPiss.textSpeed = speed;
        newPiss.glyphs = (s.ToUpper()).ToIntArray();
        if(newPiss.vn)
        {
            newPiss.currentGlyph = 0;
            newPiss.vnTimer = 0;
            newPiss.completed = false;
        }
        else
        {
            newPiss.currentGlyph = newPiss.glyphs.Length;
            newPiss.completed = true;
        }
        newPiss.active = true;
        return AddToTextArray(newPiss);
    }

    public void ChangePosition(int index, Vector2 newPosition)
    {
        textArray[index].position = newPosition;
    }

    public void ChangeScale(int index, float newScale)
    {
        textArray[index].scale = newScale;
    }

    public void ChangeText(int index, string newText)
    {
        textArray[index].text = newText;
        textArray[index].glyphs = (newText.ToUpper()).ToIntArray();
        textArray[index].currentGlyph = textArray[index].glyphs.Length;
    }

    public void CompleteVN(int index)
    {
        textArray[index].currentGlyph = textArray[index].glyphs.Length;
        textArray[index].completed = true;
    }

    public bool IsCompleted(int index)
    {
        return textArray[index].completed;
    }

    public void ClearText(int index)
    {
        RemoveFromTextArray(index);
    }

    int AddToTextArray(PissText item)
    {
        for(int i = 0; i < textArray.Length; i++)
        {
            if(!textArray[i].active)
            {
                textArray[i] = item;
                return i;
            }
        }
        return 10;
    }

    void RemoveFromTextArray(int i)
    {
        if(textArray[i].active == true)
        {
            textArray[i].active = false;
        }
    }

    void OnGUI()
    {
        for(int texts = 0; texts < textArray.Length; texts++)
        {   
            if(textArray[texts].active)
            {
                int textPosition = 0;
                for(int i = 0; i < textArray[texts].currentGlyph; i++)
                {
                    int currentGlyph = textArray[texts].glyphs[i];
                    if(currentGlyph >= 48 && currentGlyph <= 57)
                    {
                        //numbers
                        GUI.DrawTexture(new Rect(textArray[texts].position.x + textPosition,textArray[texts].position.y,
                        numbers[currentGlyph-48].width * textArray[texts].scale,numbers[currentGlyph-48].height * textArray[texts].scale),numbers[currentGlyph-48]);
                        textPosition += (int)(numbers[currentGlyph-48].width * textArray[texts].scale);
                    }
                    else if(currentGlyph >= 65 && currentGlyph <= 90)
                    {
                        //letters
                        GUI.DrawTexture(new Rect(textArray[texts].position.x + textPosition,textArray[texts].position.y,
                        letters[currentGlyph-65].width * textArray[texts].scale,letters[currentGlyph-65].height * textArray[texts].scale),letters[currentGlyph-65]);
                        textPosition += (int)(letters[currentGlyph-65].width * textArray[texts].scale);
                    }
                    else if(currentGlyph == 32)
                    {
                        //space
                        textPosition += (int)(letters[0].width * textArray[texts].scale);
                    }
                    else if(currentGlyph == 33)
                    {
                        //emark
                        GUI.DrawTexture(new Rect(textArray[texts].position.x + textPosition, textArray[texts].position.y,
                        punctuation[0].width * textArray[texts].scale,punctuation[0].height * 2 * textArray[texts].scale),punctuation[0]);
                        textPosition += (int)(punctuation[0].width * textArray[texts].scale);
                    }
                    else if(currentGlyph == 34)
                    {
                        //quotes
                        GUI.DrawTexture(new Rect(textArray[texts].position.x + textPosition, textArray[texts].position.y,
                        punctuation[1].width * textArray[texts].scale,punctuation[1].height * textArray[texts].scale),punctuation[1]);
                        textPosition += (int)(punctuation[1].width * textArray[texts].scale);
                    }
                    else if(currentGlyph == 35)
                    {
                        //pound
                        GUI.DrawTexture(new Rect(textArray[texts].position.x + textPosition, textArray[texts].position.y,
                        punctuation[2].width * textArray[texts].scale,punctuation[2].height * textArray[texts].scale),punctuation[2]);
                        textPosition += (int)(punctuation[2].width * textArray[texts].scale);
                    }
                    else if(currentGlyph == 44)
                    {
                        //comma
                        GUI.DrawTexture(new Rect(textArray[texts].position.x + textPosition, textArray[texts].position.y,
                        punctuation[3].width * textArray[texts].scale,punctuation[3].height * 2 * textArray[texts].scale),punctuation[3]);
                        textPosition += (int)(punctuation[3].width * textArray[texts].scale);
                    }
                    else if(currentGlyph == 46)
                    {
                        //period
                        GUI.DrawTexture(new Rect(textArray[texts].position.x + textPosition, textArray[texts].position.y,
                        punctuation[4].width * textArray[texts].scale,punctuation[4].height * 2 * textArray[texts].scale),punctuation[4]);
                        textPosition += (int)(punctuation[4].width * textArray[texts].scale);
                    }
                    else if(currentGlyph == 47)
                    {
                        //fslash
                        GUI.DrawTexture(new Rect(textArray[texts].position.x + textPosition, textArray[texts].position.y,
                        punctuation[5].width * textArray[texts].scale,punctuation[5].height * 2 * textArray[texts].scale),punctuation[5]);
                        textPosition += (int)(punctuation[5].width * textArray[texts].scale);
                    }
                    else if(currentGlyph == 63)
                    {
                        //qmark
                        GUI.DrawTexture(new Rect(textArray[texts].position.x + textPosition, textArray[texts].position.y,
                        punctuation[6].width * textArray[texts].scale,punctuation[6].height * textArray[texts].scale),punctuation[6]);
                        textPosition += (int)(punctuation[6].width * textArray[texts].scale);
                    }
                    else if(currentGlyph == 64)
                    {
                        //atmark
                        GUI.DrawTexture(new Rect(textArray[texts].position.x + textPosition, textArray[texts].position.y,
                        punctuation[7].width * textArray[texts].scale,punctuation[7].height * textArray[texts].scale),punctuation[7]);
                        textPosition += (int)(punctuation[7].width * textArray[texts].scale);
                    }
                }
            }
        }
    }
}
