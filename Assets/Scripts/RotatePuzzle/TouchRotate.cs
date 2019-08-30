using SpriteGlow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchRotate : MonoBehaviour
    //, IPointerDownHandler -> this is gor UI elements
{
    
    /*
     This code is for future hint system
    public void SetPuzzlePieceGlow(bool outlinesEnabled)
    {
        int width = outlinesEnabled ? 4 : 0;
        SpriteGlowEffect glowEffect = GetComponent<SpriteGlowEffect>();
        glowEffect.OutlineWidth = width;
        glowEffect.GlowBrightness = 1;
        glowEffect.GlowColor = Color.white;
    } */

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

    /*
     * This code is works for UI items
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
