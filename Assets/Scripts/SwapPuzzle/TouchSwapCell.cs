using SpriteGlow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchSwapCell : MonoBehaviour
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
        if (SwapPuzzle.isRotateEnabled)
        {
            if (Input.GetMouseButtonDown(1))
                TurnBackward();
            else if (Input.GetMouseButtonDown(0))
            {
                SwapPuzzle.selectedCell.GetComponent<SwapCell>().SetPuzzlePieceGlow(false);
                SwapPuzzle.selectedCell = gameObject;
                gameObject.GetComponent<SwapCell>().SetPuzzlePieceGlow(true);
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                SwapPuzzle.selectedCell.GetComponent<SwapCell>().SetPuzzlePieceGlow(false);
                SwapPuzzle.selectedCell = gameObject;
                gameObject.GetComponent<SwapCell>().SetPuzzlePieceGlow(true);
                

                GameObject forward = gameObject.GetComponent<SwapCell>().GetForward();
                Vector2 forwardPos;
                if (forward == null)
                    forwardPos = new Vector2(0f, 0f);
                else
                    forwardPos = gameObject.GetComponent<SwapCell>().GetForward().GetComponent<RectTransform>().anchoredPosition;

                GameObject backward = gameObject.GetComponent<SwapCell>().GetBackward();
                Vector2 backwardPos;
                if (backward == null)
                    backwardPos = new Vector2(0f, 0f);
                else
                    backwardPos = backward.GetComponent<RectTransform>().anchoredPosition;

                GameObject left = gameObject.GetComponent<SwapCell>().GetLeft();
                Vector2 leftPos;
                if (left == null)
                    leftPos = new Vector2(0f, 0f);
                else
                    leftPos = left.GetComponent<RectTransform>().anchoredPosition;

                GameObject right = gameObject.GetComponent<SwapCell>().GetRight();
                Vector2 rightPos;
                if (right == null)
                    rightPos = new Vector2(0f, 0f);
                else
                    rightPos = right.GetComponent<RectTransform>().anchoredPosition;

                Debug.Log("This: " + gameObject.GetComponent<RectTransform>().anchoredPosition
                    +"forward: " + forwardPos
                    + " backward: " + backwardPos
                    + " left: " + leftPos
                    + " right: " + rightPos);
            }
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
            SwapPuzzle.invertedCellCount++;
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, 90));
            if (transform.eulerAngles.z < 90)
            {
                SwapPuzzle.invertedCellCount--;
            }

        }
    }
    void TurnBackward()
    {
        if (transform.eulerAngles.z < 90)
        {
            transform.Rotate(new Vector3(0, 0, -90));
            SwapPuzzle.invertedCellCount++;
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, -90));
            if (transform.eulerAngles.z < 90)
            {
                SwapPuzzle.invertedCellCount--;
            }

        }
    }
    
}
