using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelChosing : MonoBehaviour
{
    public Button[] buttons;

    public GameObject levelsButton;

    private void Awake()
    {
        ButtonToArray();
        int unlockLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for(int i = 0; i < unlockLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }
    public void OpenLevel(int lvlId)
    {
        string levelName = "Level " + lvlId;
        SceneManager.LoadScene(levelName);
    }

    void ButtonToArray()
    {
        int count = levelsButton.transform.childCount;
        buttons = new Button[count];
        for(int i = 0; i < count; i++)
        {
            buttons[i] = levelsButton.transform.GetChild(i).GetComponent<Button>();
        }
    }
}
