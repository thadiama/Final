using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Pieces : MonoBehaviour
{

    private Vector3 PositionToRight;
    public bool InRight;        
    public bool selected;

    void Start()
    {
        PositionToRight = transform.position;
        transform.position = new Vector3(Random.Range(1f, 10f),Random.Range(3f, -4));      //dislpay the pieces in this area
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position,PositionToRight) < 0.5f)
        {
            if (!selected)
            {
                if (InRight == false)       //runs only one time, not in every frame
                {
                    transform.position = PositionToRight;
                    InRight = true;
                    GetComponent<SortingGroup>().sortingOrder = 0;
                    Camera.main.GetComponent<PointAndClick>().piecesInPlace++;
                }
            }
            
        }
    }
}
