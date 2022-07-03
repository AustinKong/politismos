using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameSpeed : MonoBehaviour
{
    [HideInInspector]
    public TMP_Text speedUI;

    public static GameSpeed instance;
    private void Awake()
    {
        instance = this;
        Time.timeScale = 1f;
    }

    public float speed = 1f;

    public void ToggleGameSpeed()
    {
        switch (speed)
        {
            case 1f:
                speed = 2f;
                break;
            case 2f:
                speed = 4f;
                break;
            case 4f:
                speed = 1f;
                break;
        }

        UpdateSpeedUI();
        Time.timeScale = speed;
    } 

    public void UpdateSpeedUI()
    {
        speedUI.text = "x" + speed.ToString();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
