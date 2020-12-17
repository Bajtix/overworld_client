using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Console : MonoBehaviour
{
    public GameObject consolePanel;
    public TextMeshProUGUI consoleText;
    public TMP_InputField inputField;

    public void OnSubmit(string s)
    {
        inputField.text = "";
        consoleText.text += "\n> " + s;

        ClientSend.ConsoleCommand(s);
    }

    public void ReceivedText(string s)
    {
        consoleText.text += "\n< " + s;
    }
}
