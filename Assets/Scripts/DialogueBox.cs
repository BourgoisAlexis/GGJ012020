using TMPro;
using System.IO;
using UnityEngine;
using System.Collections;

public class DialogueBox : MonoBehaviour
{
    #region Variables
    public TextMeshProUGUI Text;
    public GameObject[] answerBoxes;

    private BoxGraph boxGraph;

    private string characterName;
    private int lineIndex;
    private string[] lines;
    private bool answering;
    private int answerIndex;

    private bool canSelect;
    private bool canAct;
    private int score;

    //Tags
    private const string question = "/Q/";
    private const string answer = "/A/";
    private const string answerEnd = "/AA/";
    private const string reaction = "/R/";
    private const string reactionEnd = "/RR/";
    private const string end = "/E/";

    //private const string path = "GGJ_01_2020_Data/Resources/";
    private const string path = "Assets/Resources/";
    #endregion


    private void Awake()
    {
        boxGraph = GetComponent<BoxGraph>();

        for (int i = 0; i < answerBoxes.Length; i++)
            answerBoxes[i].SetActive(false);
    }

    private void Update()
    {
        if(answering)
            Answering();
    }


    public void Use(string _name, int _index)
    {
        characterName = _name;
        lineIndex = _index;
        score = 2;

        boxGraph.UpdateHealth(score);
        boxGraph.UpdateBackSprite(characterName, 0);

        lines = File.ReadAllLines(path + characterName + ".txt");
        StartCoroutine(UpdateText());
    }

    public void SkipPressed()
    {
        if(canAct)
        {
            if (!answering)
            {
                if (lineIndex < lines.Length - 1)
                    lineIndex++;
                CheckEmpties();

                if (CheckTags())
                    return;

                StartCoroutine(UpdateText());
            }
            else
                SelectAnswer();
        }
    }

    private IEnumerator UpdateText()
    {
        int current = 0;
        string toDisplay = "";
        int length = lines[lineIndex].Length;
        canAct = false;

        while(current < length)
        {
            yield return new WaitForFixedUpdate();
            toDisplay += lines[lineIndex][current];
            current++;
            Text.text = toDisplay;
        }

        canAct = true;
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
        canSelect = true;
        answerIndex = 0;
        boxGraph.ButtonColor(answerIndex, true);

        for (int i = 0; i < answerBoxes.Length; i++)
        {
            while (lines[lineIndex] != answer)
                lineIndex++;
            lineIndex ++;

            answerBoxes[i].SetActive(true);
            answerBoxes[i].GetComponentInChildren<TextMeshProUGUI>().text = lines[lineIndex];
        }

        while (lines[lineIndex] != answerEnd)
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
                    boxGraph.ButtonColor(answerIndex, false);
                    answerIndex++;
                    boxGraph.ButtonColor(answerIndex, true);
                    canSelect = false;
                }
            }
            else if (Input.GetAxisRaw("Vertical") == -1)
            {
                if (answerIndex > 0)
                {
                    boxGraph.ButtonColor(answerIndex, false);
                    answerIndex--;
                    boxGraph.ButtonColor(answerIndex, true);
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
            boxGraph.ButtonColor(i, false);
            answerBoxes[i].SetActive(false);
        }

        answering = false;

        boxGraph.UpdateBackSprite(characterName, int.Parse(lines[lineIndex]));

        score += int.Parse(lines[lineIndex]);
        score = Mathf.Clamp(score, 0, 3);

        boxGraph.UpdateHealth(score);

        if (score <= 0)
            CameraManager.Instance.Player.GetComponent<PlayerController>().Death();

        lineIndex++;
        StartCoroutine(UpdateText());
    }


    private void SkipOtherReactions()
    {
        while (lines[lineIndex] != reactionEnd)
            lineIndex++;

        SkipPressed();
    }

    private void End()
    {
        CameraManager.Instance.CineMode(false, null);
        gameObject.SetActive(false);
    }
}