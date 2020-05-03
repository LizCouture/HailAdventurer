using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Doozy.Engine.UI;

public class IntroManager : MonoBehaviour
{
    public static IntroManager Instance;
    public Vector3 originalPosition;
    public GameObject panel;

    public string viewCat = "Gameplay";
    public string viewName = "Intro01";

    private void Awake()
    {
         Instance = this;
        panel.SetActive(false);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void startIntro()
    {
        Debug.Log("Start Intro from INTROMANAGER");
        panel.SetActive(true);
        /*RectTransform rt = panel.GetComponent<RectTransform>();
        originalPosition = rt.anchoredPosition;
        rt.anchoredPosition = Vector3.zero;*/
    }

    public void endIntro()
    {
        Debug.Log("endIntro");
        //RectTransform rt = gameObject.GetComponent<RectTransform>();
        //Debug.Log("moving back to original position: ");
        //Debug.Log("Talking about original position: " + originalPosition.ToString());
        //rt.anchoredPosition = originalPosition;
        Debug.Log("disabling panel");
        panel.SetActive(false);
        Debug.Log("After hideView");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "InGame")
        {
            Debug.Log("Scene loaded.  Continuing");
            startIntro();
            StartCoroutine(endAfterDuration(3.0f));
        }
    }

    public IEnumerator endAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        endIntro();
        GameManager.Instance.endCurrentEvent();
    }
}
