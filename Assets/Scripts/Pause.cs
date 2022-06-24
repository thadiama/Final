using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public static bool quizPaused = false;

    [SerializeField] GameObject pausePanel;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))     //we press escape in keyboard
        {
            if (quizPaused)
            {
                ResumeQuiz();
            }
            else
            {
                PauseQuiz();
            }
        }
        
    }

    public void ResumeQuiz()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        quizPaused = false;
    }

    void PauseQuiz()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        quizPaused = true;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
        //Time.timeScale = 1f;
    }
}
