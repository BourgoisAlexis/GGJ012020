using System.Collections.Generic;
using UnityEngine.UI;
using Pixeye.Unity;
using UnityEngine;

public class BoxGraph : MonoBehaviour
{
    #region Variables
    [Foldout("Sprites", false)]
    public Sprite[] BBM, Nug, Prat;
    public GameObject[] hearts;
    public Image BackSprite;
    public Color Neutral;
    public Color Selected;

    private Dictionary<string, Sprite[]> BackSprites = new Dictionary<string, Sprite[]>();
    private GameObject[] answerBoxes;
    #endregion


    private void Awake()
    {
        answerBoxes = GetComponent<DialogueBox>().answerBoxes;

        BackSprites.Add("BBM", BBM);
        BackSprites.Add("Nug", Nug);
        BackSprites.Add("Prat", Prat);
    }


    public void UpdateBackSprite(string _name, int _index)
    {
        if (_index < 0)
        {
            CameraManager.Instance.Shake();
            AudioManager.Instance.PlaySound("Hurt", 1);
        }
        _index++;
        Sprite[] current = BackSprites[_name];
        BackSprite.sprite = current[_index];
    }

    public void UpdateHealth(int _score)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < _score)
                hearts[i].SetActive(true);
            else
                hearts[i].SetActive(false);
        }
    }

    public void ButtonColor(int _index, bool _selected)
    {
        answerBoxes[_index].GetComponent<Image>().color = _selected ? Selected : Neutral;
    }
}