using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlQuizData : MonoBehaviour       //is MonoBehaviour because is attached to the game
{
    public Levels[] allLevelData;            //to manage the number of levels i have in the game

    private PlayerAdvance playerAdvance;

    void Start()
    {
        DontDestroyOnLoad(gameObject);      //when i load the game scene i don't want to destroy quiz control from persistant scene
        LoadPlayerAdvance();                //when the game starts takes the highest score and stores it in playerAdvance

        SceneManager.LoadScene("Menu"); 
    }

    public Levels GetCurrentLevelData()      //provides its level's data
    {
        return allLevelData [playerAdvance.currentLevel];       //sees in whitch level we are on
    }

    public int GetBestScore()        //display the highest score in the end of our level
    {
        return  playerAdvance.bestScore; 
    }

    public void NewPlayerScore(int newScore) 
    {
        // If newScore is higher than playerAdvance.bestScore, update playerAdvance with the new value and call SavePlayerAdvance()
        if (newScore > playerAdvance.bestScore)
        {
            playerAdvance.bestScore = newScore;
            SavePlayerAdvance();        
        }
    }

    //checks if there are more levels left to finish the game
    public bool HasMoreLevels()
    {
        return (allLevelData.Length - 1 > playerAdvance.currentLevel);      
    }

    public void GetNextLevel() 
    {
        if (HasMoreLevels())        //if we have more levels increment current level
        {
            playerAdvance.currentLevel++;

            SaveCurrentLevel();     //save current level to player prefs so next time we load the game we can load at this level
        }
    }

    // This function could be extended easily to handle any additional data we wanted to store in our PlayerAdvance object
    public void LoadPlayerAdvance()
    {
        playerAdvance = new PlayerAdvance();      // Create a new PlayerAdvance object

        if (PlayerPrefs.HasKey("bestScore"))      // If PlayerPrefs contains a key called "bestScore", set the value of playerAdvance.bestScore using the value associated with that key
        {
            playerAdvance.bestScore = PlayerPrefs.GetInt("bestScore"); 

        }

        if (PlayerPrefs.HasKey("currentLevel"))  // If PlayerPrefs contains a key called "current", set the value of playerAdvance.current using the value associated with that key
        {
            playerAdvance.currentLevel = PlayerPrefs.GetInt("currentLevel"); 
        }
    }

    //resets levels if i want to restart the game 
    public void ResetCurrentLevel()
    {
        playerAdvance.currentLevel = 0;     //reset current level back to 0 meaning the start of the levels and save to player prefs
        SaveCurrentLevel();
    }

    private void SavePlayerAdvance()
    {
        PlayerPrefs.SetInt("bestScore", playerAdvance.bestScore);       // Save the value playerAdvance.bestScore to PlayerPrefs, with a key of "bestScore"
    }

    private void SaveCurrentLevel()
    {
        PlayerPrefs.SetInt("currentLevel", playerAdvance.currentLevel);     // Save the value playerAdvance.currentLevel to PlayerPrefs, with a key of "currentLevel"
    }
}
