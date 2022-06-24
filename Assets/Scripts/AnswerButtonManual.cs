using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButtonManual : MonoBehaviour 
{
    public Image answerImageDisplay;
    private AnswerData answerData;
    private QuizControl quizControl;

    void Start()
    {
        quizControl = FindObjectOfType<QuizControl>();
    }

    public void SetUp(AnswerData sprite)      //pass in answer data and store them in aswerData
    {
        answerData = sprite;
        answerImageDisplay.sprite = answerData.answerImage;
    }

    public void HandleClick()
    {
        quizControl.AnswerButtonClicked(answerData.isCorrect);   //if the answer we click is correct or not
    }
}
