using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchRotate : MonoBehaviour
    //, IPointerDownHandler
{

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            TurnForward();
            Debug.Log("Right");
        }
        else if (Input.GetMouseButtonDown(0))
        {
            TurnBackward();
            Debug.Log("Left");
        }
    }
    /*
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            TurnForward();
        }
        else if(eventData.button == PointerEventData.InputButton.Left)
        {
            TurnBackward();
        }
        Debug.Log("Angle: " + transform.eulerAngles.z);
       Debug.Log("Count: " + RotatePuzzle.falseCellCount);
    }
    */
    void TurnForward()
    {
        
        if (transform.eulerAngles.z < 90)
        {
            transform.Rotate(new Vector3(0, 0, 90));
            RotatePuzzle.falseCellCount++;
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, 90));
            if (transform.eulerAngles.z < 90)
            {
                RotatePuzzle.falseCellCount--;
            }

        }
    }
    void TurnBackward()
    {
        if (transform.eulerAngles.z < 90)
        {
            transform.Rotate(new Vector3(0, 0, -90));
            RotatePuzzle.falseCellCount++;
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, -90));
            if (transform.eulerAngles.z < 90)
            {
                RotatePuzzle.falseCellCount--;
            }

        }
    }
    
}
