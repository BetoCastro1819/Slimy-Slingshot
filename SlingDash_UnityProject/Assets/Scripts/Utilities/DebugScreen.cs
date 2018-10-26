using System;
using UnityEngine;
using UnityEngine.UI;
public class DebugScreen : MonoBehaviour
{
    public GameObject panel;
    public GameObject buttonPrefab;

    #region Singleton
    private static DebugScreen instance;
    public static DebugScreen Get()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }
    #endregion

    public void AddButton(string buttonName, Action action)
    {
        GameObject button = Instantiate(buttonPrefab);

        Text buttonText = buttonPrefab.GetComponentInChildren<Text>();
        buttonText.text = buttonName;

        button.GetComponent<Button>().onClick.AddListener(() => action());

        button.transform.parent = panel.transform;
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.D))
            panel.SetActive(!panel.activeSelf);
    }
}
