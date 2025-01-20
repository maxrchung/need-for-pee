using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
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
        _dialoguePosition.x = canvas.GetComponent<RectTransform>().rect.width * 0.05f;
        _dialoguePosition.y = canvas.GetComponent<RectTransform>().rect.height * 0.875f;
        for (int i = 0; i < 3; i++)
        {
            buttons[i].GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,
                canvas.GetComponent<RectTransform>().rect.height * (0.1f + 0.2f * i),
                canvas.GetComponent<RectTransform>().rect.height * 0.1f);
            _choicePositions[i].y = canvas.GetComponent<RectTransform>().rect.height * (0.1f + 0.2f * i) + 10;
            _choicePositions[i].x = canvas.GetComponent<RectTransform>().rect.width / 2 -
                buttons[i].GetComponent<RectTransform>().rect.width / 2 + 25;
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
        return _choiceTask.Task;
    }

    public Task<int> DisplayChoice(string text, params string[] choices)
    {
        _isInChoice = true;
        DisplayButtons(choices.Length);
        var idx = 0;
        foreach (var choice in choices)
        {
            AddString(choice, _choicePositions[idx++], choiceScale, false);
        }

        AddString(text, _dialoguePosition, dialogueScale, true);
        _choiceTask = new TaskCompletionSource<int>();
        return _choiceTask.Task;
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
    }
}