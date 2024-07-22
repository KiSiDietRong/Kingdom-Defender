using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class LogInScript : MonoBehaviour
{
    public TMP_InputField userName;
    public TMP_InputField password;
    public TextMeshProUGUI notification;

    public void LoginButton()
    {
        StartCoroutine(LogIn());
    }
    private IEnumerator LogIn()
    {
        WWWForm form = new WWWForm();
        form.AddField("user: ", userName.text);
        form.AddField ("password", password.text);

        UnityWebRequest www = UnityWebRequest.Post("https://fpl.expvn.com/dangky.php", form);
        yield return www.SendWebRequest();

        if(!www.isDone)
        {
            notification.text = "Log in not successful";
        }
        else
        {
            string get = www.downloadHandler.text;

            switch(get)
            {
                case "exist": notification.text = "Account already exist"; break;
                case "OK": notification.text = "Log in successful. Please return and Sign in"; break;
                case "ERROR": notification.text = "Log in failed"; break;
                default: notification.text = "Failed to connect server"; break;
            }
        }
    }
}
