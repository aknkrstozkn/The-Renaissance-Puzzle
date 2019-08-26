using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchRotate : MonoBehaviour, IPointerDownHandler
{
    

    

    public void OnPointerDown(PointerEventData eventData)
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
       Debug.Log("Angle: " + transform.eulerAngles.z);
       Debug.Log("Count: " + RotatePuzzle.falseCellCount);
    }
    
}
