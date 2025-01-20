using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Windows.Speech;

public class VNManager : MonoBehaviour
{
    public TextManagerScript textManager;
    public float dialogueScale;
    public float choiceScale;
    public Canvas canvas;
    public GameObject dialoguePanel;
    public GameObject[] buttons;
    private bool _isInChoice = false;
    public bool isPlaying = false;
    private readonly List<int> _textsToClear = new List<int>();
    Vector2 _dialoguePosition;
    private int _mainText = -1;

    readonly Vector2[] _choicePositions = new Vector2[3];
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private TaskCompletionSource<int> _choiceTask = null;

    void Start()
    {
        dialoguePanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            canvas.GetComponent<RectTransform>().rect.width);
        dialoguePanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            canvas.GetComponent<RectTransform>().rect.height * 0.2f);
        _dialoguePosition.x = Screen.width*0.05f;
        _dialoguePosition.y = Screen.height*0.85f;
        for (int i = 0; i < 3; i++)
        {
            buttons[i].GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,
                canvas.GetComponent<RectTransform>().rect.height * (0.1f + 0.2f * i), canvas.GetComponent<RectTransform>().rect.height * 0.1f);
            _choicePositions[i].y = Screen.height * (0.12f + 0.2f * i);
            _choicePositions[i].x = (float)Screen.width * 0.5f - buttons[i].GetComponent<RectTransform>().rect.width * 0.45f;
        }

        dialoguePanel.SetActive(false);
        ClearButtons();
    }

    // Update is called once per frame
    void Update()
    {
        if (_mainText == -1) return;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (!textManager.IsCompleted(_mainText))
            {
                textManager.CompleteVN(_mainText);
            }
            else if (_choiceTask != null && !_isInChoice)
            {
                SelectChoice(-1);
            }
        }
    }

    public void SelectChoice(int choice)
    {
        ClearText();
        ClearButtons();
        var task = _choiceTask;
        _choiceTask = null;
        task.SetResult(choice);
    }

    private void AddString(string value, Vector2 position, float scale, bool vn)
    {
        _textsToClear.Add(textManager.PissTextGeneration(value, position, scale, vn));
        if (vn) _mainText = _textsToClear.Last();
    }

    public Task<int> DisplayText(string text)
    {
        _isInChoice = false;
        AddString(text, _dialoguePosition, dialogueScale, true);
        _choiceTask = new TaskCompletionSource<int>();
        dialoguePanel.SetActive(true);
        return _choiceTask.Task;
    }

    public Task<int> DisplayChoice(string text, params string[] choices)
    {
        dialoguePanel.SetActive(true);
        AddString(text, _dialoguePosition, dialogueScale, true);
        _isInChoice = true;
        StartCoroutine(WaitForText(choices));
        
        
        _choiceTask = new TaskCompletionSource<int>();
        return _choiceTask.Task;
    }

    IEnumerator WaitForText(params string[] choices)
    {
        while(textManager.textArray[_textsToClear[_mainText]].completed == false)
        {
            yield return new WaitForSeconds(0.1f);
        }
        DisplayButtons(choices.Length);
        var idx = 0;
        foreach (var choice in choices)
        {
            AddString(choice, _choicePositions[idx++], choiceScale, false);
        }
    }

    void DisplayButtons(int numChoices)
    {
        for (int i = 0; i < numChoices; i++)
        {
            buttons[i].SetActive(true);
        }
    }

    void ClearButtons()
    {
        foreach (GameObject g in buttons)
        {
            g.SetActive(false);
        }
    }

    void ClearText()
    {
        foreach (var item in _textsToClear)
        {
            textManager.ClearText(item);
        }

        _textsToClear.Clear();
        _mainText = -1;
        dialoguePanel.SetActive(false);
    }
}