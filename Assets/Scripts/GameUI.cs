using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Image PauseButton;
    public Sprite PauseSprite;
    public Sprite PlaySprite;
    public GameObject QuitButton;

    public bool Paused;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseOrPlay();
    }

    public void PauseOrPlay()
    {
        if (Paused)
        {
            PauseButton.sprite = PauseSprite;
            QuitButton.SetActive(false);
            Paused = false;
        }
        else
        {
            PauseButton.sprite = PlaySprite;
            QuitButton.SetActive(true);
            Paused = true;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
