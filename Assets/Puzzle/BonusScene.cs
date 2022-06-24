using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BonusScene : MonoBehaviour
{
    public void StartPuzzle(int PaintingNumber)
    {
        PlayerPrefs.SetInt("Painting", PaintingNumber);
        SceneManager.LoadScene("Puzzle");

    }
}
