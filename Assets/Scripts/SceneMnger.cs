using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMnger : MonoBehaviour
{
    public void LoadInfo()
    {
        SceneManager.LoadScene("InfoScene");
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public static void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameOverScene");
    }
}
