using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BoxGraph : MonoBehaviour
{
    public Color Neutral;
    public Color Selected;
    private GameObject[] answerBoxes;

    private void Awake()
    {
        answerBoxes = GetComponent<DialogueBox>().answerBoxes;
    }

    public void ChangeColor(int _index, bool _selected)
    {
        answerBoxes[_index].GetComponent<Image>().color = _selected ? Selected : Neutral;
    }
}
