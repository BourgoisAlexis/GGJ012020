using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.IO;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{
    #region Variables
    public TextMeshProUGUI Text;
    public GameObject[] answerBoxes;

    private BoxGraph boxGraph;

    private string name;
    private int lineIndex;
    private string[] lines;
    private bool answering;
    private int answerIndex;
    private bool canSelect;
    private int score;

    private const string path = "Assets/Dialogues/";
    //Tags
    private const string question = "/Q/";
    private const string answer = "/A/";
    private const string reaction = "/R/";
    private const string reactionEnd = "/RR/";
    private const string end = "/E/";

    //Accessors
    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }
    }
    #endregion


    private void Awake()
    {
        Text = GetComponentInChildren<TextMeshProUGUI>();
        boxGraph = GetComponent<BoxGraph>();

        for (int i = 0; i < answerBoxes.Length; i++)
            answerBoxes[i].SetActive(false);
    }

    private void Update()
    {
        if(answering)
            Answering();
    }


    public void Setup(string _name, int _index)
    {
        Name = _name;
        lineIndex = _index;
        score = 0;

        lines = File.ReadAllLines(path + Name + ".txt");
        UpdateText();
    }

    public void SkipPressed()
    {
        if (!answering)
        {
            if (lineIndex < lines.Length - 1)
                lineIndex++;

            CheckEmpties();
            if (CheckTags())
                return;

            UpdateText();
        }
        else
            SelectAnswer();
    }

    private void UpdateText()
    {
        Text.text = lines[lineIndex];
    }


    private void CheckEmpties()
    {
        while (lines[lineIndex] == "")
                lineIndex++;
    }

    private bool CheckTags()
    {
        switch(lines[lineIndex])
        {
            case question :
                DisplayAnswers();
                return true;

            case reaction :
                SkipOtherReactions();
                return true;

            case reactionEnd:
                lineIndex++;
                SkipPressed();
                return true;

            case end:
                End();
                return true;
        }
        return false;
    }


    private void DisplayAnswers()
    {
        answering = true;
        answerIndex = 0;
        canSelect = true;
        boxGraph.ChangeColor(answerIndex, true);
        string[] answers = new string[3];

        for (int i = 0; i < answers.Length; i++)
        {
            while (lines[lineIndex] != answer)
                lineIndex++;
            lineIndex ++;

            answers[i] = lines[lineIndex];
            answerBoxes[i].SetActive(true);
            answerBoxes[i].GetComponentInChildren<TextMeshProUGUI>().text = lines[lineIndex];
        }

        while (lines[lineIndex] != answer)
            lineIndex++;
        lineIndex++;
    }

    private void Answering()
    {
        if (canSelect)
        {
            if (Input.GetAxisRaw("Vertical") == 1)
            {
                if (answerIndex < answerBoxes.Length - 1)
                {
                    boxGraph.ChangeColor(answerIndex, false);
                    answerIndex++;
                    boxGraph.ChangeColor(answerIndex, true);
                    canSelect = false;
                }
            }
            else if (Input.GetAxisRaw("Vertical") == -1)
            {
                if (answerIndex > 0)
                {
                    boxGraph.ChangeColor(answerIndex, false);
                    answerIndex--;
                    boxGraph.ChangeColor(answerIndex, true);
                    canSelect = false;
                }
            }
        }
        else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) <= 0.5f)
            canSelect = true;
    }

    private void SelectAnswer()
    {
        CheckEmpties();
        lineIndex++;

        for (int i = 0; i < answerIndex; i++)
        {
            while (lines[lineIndex] != reaction)
                lineIndex++;
            lineIndex++;
        }

        for (int i = 0; i < answerBoxes.Length; i++)
        {
            boxGraph.ChangeColor(i, false);
            answerBoxes[i].SetActive(false);
        }

        answering = false;

        score += int.Parse(lines[lineIndex]);
        lineIndex++;
        UpdateText();
    }


    private void SkipOtherReactions()
    {
        while (lines[lineIndex] != reactionEnd)
            lineIndex++;

        SkipPressed();
    }

    private void End()
    {
        CameraManager.Instance.CineModeEnd();
        gameObject.SetActive(false);
    }
}
