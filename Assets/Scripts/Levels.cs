using System.Collections;
using UnityEngine;

[System.Serializable]
public class Levels
{
    public string name;                 //name of the round
    public int timeLmtSec;              //time to comlete the round
    public int GainedPointsCorrect;     //points for correct answer
    public Question[] questions;        //array of questions for the round
}
