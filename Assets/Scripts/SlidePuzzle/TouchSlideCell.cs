using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchSlideCell : MonoBehaviour
{
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            TurnBackward();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            TurnForward();
        }
    }
    void TurnForward()
    {

        if (transform.eulerAngles.z < 90)
        {
            transform.Rotate(new Vector3(0, 0, 90));
            SlidePuzzle.invertedCellCount++;
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, 90));
            if (transform.eulerAngles.z < 90)
            {
                SlidePuzzle.invertedCellCount--;
            }

        }
    }
    void TurnBackward()
    {
        if (transform.eulerAngles.z < 90)
        {
            transform.Rotate(new Vector3(0, 0, -90));
            SlidePuzzle.invertedCellCount++;
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, -90));
            if (transform.eulerAngles.z < 90)
            {
                SlidePuzzle.invertedCellCount--;
            }

        }
    }

}
