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
    public Canvas canvas;
    public Texture2D[] letters;
    public Texture2D[] numbers;
    public GameObject textPanel;
    public float textSpeed = 0.1f;//time between next glyph appearing
    Dictionary<int,float> vnTimers = new Dictionary<int, float>();
    Dictionary<int,int> currentGlyphTracker = new Dictionary<int, int>();
    public List<PissText> textList = new List<PissText>();
    PissText[] textArray = new PissText[10];
    int textArrayCount;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < textArray.Length; i++)
        {
            textArray[i].active = false;
        }
        canvas = gameObject.GetComponent<Canvas>();
        PissTextGeneration("test 123", new Vector2(10,10),0.25f,true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            ClearText(0);
        }
        for(int i = 0; i < textArray.Length; i++)
        {
            if(textArray[i].active)
            {
                if(textArray[i].vn)
                {
                    if(textArray[i].currentGlyph < textArray[i].glyphs.Length)
                    {
                        textArray[i].vnTimer += Time.deltaTime;
                        if(textArray[i].vnTimer > textSpeed)
                        {
                            textArray[i].currentGlyph += 1;
                            textArray[i].vnTimer = 0f;
                        }
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
        newPiss.glyphs = (s.ToUpper()).ToIntArray();
        if(newPiss.vn)
        {
            //currentGlyphTracker[textList.Count] = 0;
            //vnTimers[textList.Count] = 0f;
            newPiss.currentGlyph = 0;
            newPiss.vnTimer = 0;
        }
        else
        {
            //currentGlyphTracker[textList.Count] = newPiss.glyphs.Length;
            newPiss.currentGlyph = newPiss.glyphs.Length;
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
                        Graphics.DrawTexture(new Rect(textArray[texts].position.x + textPosition,textArray[texts].position.y,
                        numbers[currentGlyph-48].width * textArray[texts].scale,numbers[currentGlyph-48].height * textArray[texts].scale),numbers[currentGlyph-48]);
                        textPosition += (int)(numbers[currentGlyph-48].width * textArray[texts].scale);
                    }
                    else if(currentGlyph >= 65 && currentGlyph <= 90)
                    {
                        //letters
                        Graphics.DrawTexture(new Rect(textArray[texts].position.x + textPosition,textArray[texts].position.y,
                        letters[currentGlyph-65].width * textArray[texts].scale,letters[currentGlyph-65].height * textArray[texts].scale),letters[currentGlyph-65]);
                        textPosition += (int)(letters[currentGlyph-65].width * textArray[texts].scale);
                    }
                    else if(currentGlyph == 32)
                    {
                        textPosition += (int)(letters[0].width * textArray[texts].scale);
                    }
                }
            }
        }
        
    }
}
