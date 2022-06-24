using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoControl : MonoBehaviour
{
    public void OpenInfo(string URL)
    {
        Application.OpenURL(URL);
    }
}
