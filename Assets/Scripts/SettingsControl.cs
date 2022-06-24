using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsControl : MonoBehaviour
{
    public GameObject HowToPlayPanel;

    public void ExplaneGame()
    {
        HowToPlayPanel.SetActive(true);
    }

    public void ExitButton()
    {
        StartCoroutine(Exit());
    }

    IEnumerator Exit()
    {
        yield return new WaitForSeconds(0.5f);
        HowToPlayPanel.SetActive(false);
        StopCoroutine(Exit());
    }
}
