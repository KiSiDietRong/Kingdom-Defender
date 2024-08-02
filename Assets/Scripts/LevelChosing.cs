using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelChosing : MonoBehaviour
{
    public Button[] buttons;

    public GameObject levelsButton;
<<<<<<< HEAD
    public GameObject loadingPanel;
    public Slider progressBar;
=======
>>>>>>> Toàn

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
<<<<<<< HEAD
        StartCoroutine(LoadLevelAsync(levelName));
    }

    private IEnumerator LoadLevelAsync(string levelName)
    {

        loadingPanel.SetActive(true);


        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);


        progressBar.value = 0;
        progressBar.maxValue = 1;


        while (!asyncLoad.isDone)
        {

            progressBar.value = asyncLoad.progress;


            yield return null;
        }


        loadingPanel.SetActive(false);
=======
        SceneManager.LoadScene(levelName);
>>>>>>> Toàn
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