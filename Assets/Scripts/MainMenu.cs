﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ShowCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
