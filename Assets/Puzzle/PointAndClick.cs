using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PointAndClick : MonoBehaviour
{
    public Sprite[] Paintings;

    public GameObject PuzzleOverPanel;
    public GameObject PieceSelected;
    public GameObject PrintPanel;

    public AudioSource BackGroundMusic;

    public Image currentPuzzleDisplay;

    int orderInLayer = 1;
    public int piecesInPlace = 0;

    void Start()
    {
        for (int i = 0; i < 36; i++)
        {
            GameObject.Find("Piece (" + i + ")").transform.Find("Puzzle").GetComponent<SpriteRenderer>().sprite = Paintings[PlayerPrefs.GetInt("Painting")];
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))        //we want each piece to be clicable
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.transform.CompareTag("Puzzle"))
            {
                if (!hit.transform.GetComponent<Pieces>().InRight)
                {
                    PieceSelected = hit.transform.gameObject;
                    PieceSelected.GetComponent<Pieces>().selected = true;
                    PieceSelected.GetComponent<SortingGroup>().sortingOrder = orderInLayer;     //the piece we select each time to be on top of the others
                    orderInLayer++;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))      //we want to be able to drop each piece
        {
            if (PieceSelected != null)
            {
                PieceSelected.GetComponent<Pieces>().selected = false;
                PieceSelected = null;
            }
        }

        if (PieceSelected != null)       //we want the pieces to be movable
        {
            Vector3 MousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);   //we want the vector z to be 0 all the time
            PieceSelected.transform.position = new Vector3(MousePoint.x, MousePoint.y, 0);
        }

        if (piecesInPlace == 36)
        {
            currentPuzzleDisplay.sprite = Paintings[PlayerPrefs.GetInt("Painting")];
            StartCoroutine(LevelOver());
        }

        IEnumerator LevelOver()
        {
            yield return new WaitForSeconds(1);
            PuzzleOverPanel.SetActive(true);
            BackGroundMusic.Stop();
            StopCoroutine(LevelOver());
        }
}

    public void NextPuzzle()
    {
        PlayerPrefs.SetInt("Painting", PlayerPrefs.GetInt("Painting")+1);
        StartCoroutine(LoadPuzzle());
    }

    IEnumerator LoadPuzzle()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Puzzle");
        StopCoroutine(LoadPuzzle());
    }

    public void BacktoStart()   //star Art Quiz from the top
    {
        SceneManager.LoadScene("Menu");
    }

    public void PrintPuzzle()   //the application will load a panel with the puzzle we just solved 
    {
        StartCoroutine(PrintAfterOne());
    }

    IEnumerator PrintAfterOne()
    {
        yield return new WaitForSeconds(1);
        PrintPanel.SetActive(true);
        StopCoroutine(PrintAfterOne());
    }
}
