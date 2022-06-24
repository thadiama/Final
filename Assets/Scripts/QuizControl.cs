using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class QuizControl : MonoBehaviour 
{
    public Text questionDisplayTxt; 
    public Image answerImageDisplay;
    public Text levelNameDisplayTxt;
    public Text scoreDisplayTxt; 
    public Text timeRemainDisplayTxt;
    public SimpleObjectPool answerButtonObjectPool;     //we need that because we are going to be interacting with the pool
    public Transform answerButtonParent;

    public GameObject questionDisplay;        
    public GameObject levelEndDisplay; 
    public GameObject nextLevelDisplay; 
    public Text bestScoreDisplay;

    public GameObject bonusCountdownDisplay;        //for the Panel display
    public Text countdownDisplayTxt;                //for the text display
    public int timer;

    private ControlQuizData controlQuizData; 
    private Levels currentLevelData; 
    private Question[] questionPool;

    public AudioSource BackGroundMusic;
    public AudioSource CorrectAnswerMusic;
    public AudioSource WrongAnswerMusic;
    public AudioSource LevelOverMusic;

    private bool isLevelActive;     //if there are more questions to finish the level
    private float timeRemain;       //the time that remanes for the level
    private int questionIndex;      //what is the number of question that we are on
    private int playerScore;

    private string currentLevelName;       //which is the current level we are playing

    private List<GameObject> answerButtonGameObjects = new List<GameObject>();      //list to store the buttons we remove


    // Start is called before the first frame update
    void Start()
    {
        controlQuizData = FindObjectOfType<ControlQuizData>();      //start from persistant scene and find controlquizdata with q&a
        SetUpLevel(); 
    }

    public void SetUpLevel() 
    {
        currentLevelData = controlQuizData.GetCurrentLevelData();   //get the level data from controlquizdata store them in the currentleveldata
        questionPool = currentLevelData.questions;                  //store the questions we are gonna be asking 
        timeRemain = currentLevelData.timeLmtSec; 
        UpdateTimeRemain();

        playerScore = 0;             //refreshes the score to 0 for its level
        questionIndex = 0;          //start of the begining of the list of our questions 

        currentLevelName = currentLevelData.name;         //which level we play
        ShowLevelName();            

        ShowScore(); 
        ShowQuestion();
        isLevelActive = true;       //level has started

    }

    private void ShowLevelName()
    {
        levelNameDisplayTxt.text = "" + currentLevelName.ToString();
    }

    private void ShowScore()   
    {
        scoreDisplayTxt.text = "   :   " + playerScore.ToString(); 
    }

    private void ShowQuestion()
    {
        RemoveAnswerButtons();
        Question question = questionPool[questionIndex]; 
        questionDisplayTxt.text = question.questionTxt;    

        //loop all the aswers
        for (int i = 0; i < question.answers.Length; i++)  
        {
            GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();     //that is the button we are gonna work with, get me another button that's not been used 
            answerButtonGameObjects.Add(answerButtonGameObject);                        //this removes the buttons of previous answers
            answerButtonGameObject.transform.SetParent(answerButtonParent);             //we are gonna parent it to the answerButtonPannel

            AnswerButtonManual answerButtonManual = answerButtonGameObject.GetComponent<AnswerButtonManual>();    //this is for reference to the AnswerButtonManual script
            answerButtonManual.SetUp(question.answers[i]);
        }
    }

    private void RemoveAnswerButtons()          //remove the answer buttons we don't need return them to the pool 
    {
        while (answerButtonGameObjects.Count > 0)
        {
            answerButtonObjectPool.ReturnObject(answerButtonGameObjects[0]);        //whatever object is diactivated and get redy to be recycled and reused
            answerButtonGameObjects.RemoveAt(0);                                    //remove it from the list of active button gameObjects
        }
    }

   
    public void AnswerButtonClicked(bool isCorrect)     //passes in wether or not the answer clicked is correct add points
    {
        if (isCorrect)
        {
            CorrectAnswerMusic.Play();
            playerScore += currentLevelData.GainedPointsCorrect;
            scoreDisplayTxt.text = "   :   " + playerScore.ToString();
        }
        else
        {
            WrongAnswerMusic.Play();
        }

        if (questionPool.Length > questionIndex + 1)
        {
            StartCoroutine(WaitForSeconds());     //waits 1.5 second before moves to next question
        }
        else 
        {
            EndLevel();
        }
    }

    IEnumerator WaitForSeconds()   // waits 1.5 seconds before moves to next question
    {
        yield return new WaitForSeconds(1.5f);
        questionIndex++;
        StopCoroutine(WaitForSeconds());
        ShowQuestion();
    }

    public void EndLevel() 
    {
        isLevelActive = false;

        BackGroundMusic.Stop();
        StartCoroutine(Wait1Second());
        
        controlQuizData.NewPlayerScore(playerScore); 
        bestScoreDisplay.text = "Best Score : " + controlQuizData.GetBestScore().ToString();    //display best score to levelOverPanel

        StartCoroutine(WaitASecond());

        //we press the button to move to next level if we have one
        if (controlQuizData.HasMoreLevels()) 
        {
            nextLevelDisplay.SetActive(true);       //if we do the next level button is activated
        }
        else
        {
            nextLevelDisplay.SetActive(false);      //if we don't the next level button is disactivated
            StartCoroutine(ManageTimer());
        }
    }

    IEnumerator ManageTimer()
    {
        yield return new WaitForSeconds(3.5f);
        bonusCountdownDisplay.SetActive(true);

        while (timer > 0)
        {
            countdownDisplayTxt.text = "Bonus Level in  " + timer.ToString();
            yield return new WaitForSeconds(1);
            timer--;
        }

        bonusCountdownDisplay.SetActive(false);
        SceneManager.LoadScene("Bonus");
        StopCoroutine(ManageTimer());
    }

    IEnumerator WaitASecond()      //waits 1 second activates LevelOver Panel
    {
        yield return new WaitForSeconds(1);
        questionDisplay.SetActive(false);           // disactivate the QuestionPanel
        levelEndDisplay.SetActive(true);            // activate the levelOverPanel
        StopCoroutine(WaitASecond());
    }

    IEnumerator Wait1Second()      //waits 1 second before LevelOver Music comes on 
    {
        yield return new WaitForSeconds(1);
        LevelOverMusic.Play();
        StopCoroutine(Wait1Second());
        StartCoroutine(Wait2Seconds());     
    }

    IEnumerator Wait2Seconds()      //waits 2 seconds before BGMusic comes back on 
    {
        yield return new WaitForSeconds(2);
        BackGroundMusic.Play();
        StopCoroutine(Wait2Seconds());
    }

    public void GoToNextLevel() 
    {
        controlQuizData.GetNextLevel();         //move to next level

        SetUpLevel();                           //reset variables for the next level 

        questionDisplay.SetActive(true);        // activate the QuestionPanel
        levelEndDisplay.SetActive(false);      // disactivate the LevelOverPanel
    }

    public void ReturnToMenu()                 //when we finish the game we return to the Menu
    {
        controlQuizData.ResetCurrentLevel();  //and we return to level one if we start again
        SceneManager.LoadScene("Menu");
    }

    private void UpdateTimeRemain()
    {
        timeRemainDisplayTxt.text = "Time : " + Mathf.Round(timeRemain).ToString(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (isLevelActive) 
        {
                timeRemain -= Time.deltaTime;       //calculate the available time remaining
                UpdateTimeRemain();

            if (timeRemain <= 0f) 
                {
                    EndLevel();
                }
        }
    }
}
