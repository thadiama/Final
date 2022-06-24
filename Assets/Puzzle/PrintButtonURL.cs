using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintButtonURL : MonoBehaviour
{
    public string URL;
    public void OpenPuzzle()
    {
        Application.OpenURL(URL);
    }
}
