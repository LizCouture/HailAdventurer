using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class MessageManager : MonoBehaviour 
{
    public TextMeshProUGUI MessageText;
    public GameObject MessagePanel;

    public static MessageManager Instance;

    void Awake()
    {
        Instance = this;
        MessagePanel.SetActive(false);
    }

    public void ShowMessage(string Message, float Duration)
    {
        StartCoroutine(ShowMessageCoroutine(Message, Duration));
    }

    IEnumerator ShowMessageCoroutine(string Message, float Duration)
    {
        //Debug.Log("Showing some message. Duration: " + Duration);
        MessageText.text = Message;
        MessagePanel.SetActive(true);

        yield return new WaitForSeconds(Duration);

        MessagePanel.SetActive(false);
        //TODO: Command.CommandExecutionComplete();
    }

    // TEST PURPOSES ONLY
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            ShowMessage("Choose 2 Items To Sell", 3f);
        
        if (Input.GetKeyDown(KeyCode.S))
            ShowMessage("Sell It!", 3f);
    }
}
