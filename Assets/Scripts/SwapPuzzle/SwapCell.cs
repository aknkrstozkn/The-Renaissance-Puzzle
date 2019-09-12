using SpriteGlow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCell : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isPositionTrue;
    private Vector2 truePosition;
    private int maxGlowWidth;
    private GameObject forward;
    private GameObject tempForward;
    private GameObject backward;
    private GameObject tempBackward;
    private GameObject right;
    private GameObject tempRight;
    private GameObject left;
    private GameObject tempLeft;


    public void SetMaxGlowWidth(int maxGlowWidth)
    {
        this.maxGlowWidth = maxGlowWidth;
    }

    public int GetMaxGlowWidth()
    {
        return maxGlowWidth;   
    }
    private void SetTempValues()
    {
        tempForward = forward;
        tempBackward = backward;
        tempLeft = left;
        tempRight = right;
    }
    public Vector2 GetTruePosition()
    {
        return truePosition;
    }
    public void SetTruePosition(Vector2 truePosition)
    {
        this.truePosition = truePosition;
    }
    public void SetIsPositionTrue(bool isPositionTrue)
    {
        this.isPositionTrue = isPositionTrue;
    }
    public bool GetIsPositionTrue()
    {
        return isPositionTrue;
    }
    

    public GameObject GetForward()
    {
        return forward;
    }

    public void SetForward(GameObject forward)
    {
        this.forward = forward;
    }
    public GameObject GetBackward()
    {
        return backward;
    }

    public void SetBackward(GameObject backward)
    {
        this.backward = backward;
    }
    public GameObject GetRight()
    {
        return right;
    }

    public void SetRight(GameObject right)
    {
        this.right = right;
    }

    public GameObject GetLeft()
    {
        return left;
    }

    public void SetLeft(GameObject left)
    {
        this.left = left;
    }

    public void SwapBackward()
    {
        if (backward == null)
            return;

        SetTempValues();

        GameObject swapObject = backward;
        SwapCell swapCell = swapObject.GetComponent<SwapCell>();

        SetForwardAdjacentsValue(swapObject);
        SetLeftAdjacentsValue(swapObject);
        SetRightAdjacentsValue(swapObject);

        swapCell.SetBackwardAdjacentsValue(gameObject);
        swapCell.SetRightAdjacentsValue(gameObject);
        swapCell.SetLeftAdjacentsValue(gameObject);

        forward = backward;
        backward = swapCell.GetBackward();
        left = swapCell.GetLeft();
        right = swapCell.GetRight();
        

        swapCell.SetForward(tempForward);
        swapCell.SetBackward(this.gameObject);
        swapCell.SetLeft(tempLeft);
        swapCell.SetRight(tempRight);

        ChangingPositions(swapObject, swapCell);
    }
    public void SetPuzzlePieceGlow(bool outlinesEnabled)
    {
        
        int width = outlinesEnabled ? maxGlowWidth : 0;
        SpriteGlowEffect glowEffect = GetComponent<SpriteGlowEffect>();
        glowEffect.OutlineWidth = width;
        glowEffect.GlowBrightness = 1;
        glowEffect.GlowColor = Color.red;
    }
    public void SwapLeft()
    {
        if (left == null)
            return;

        SetTempValues();

        GameObject swapObject = left;
        SwapCell swapCell = swapObject.GetComponent<SwapCell>();

        SetForwardAdjacentsValue(swapObject);
        SetRightAdjacentsValue(swapObject);
        SetBackwardAdjacentsValue(swapObject);

        swapCell.SetForwardAdjacentsValue(gameObject);
        swapCell.SetLeftAdjacentsValue(gameObject);
        swapCell.SetBackwardAdjacentsValue(gameObject);

        SetForward(swapCell.GetForward());
        SetRight(swapObject);
        SetLeft(swapCell.GetLeft());
        SetBackward(swapCell.GetBackward());

        swapCell.SetForward(tempForward);
        swapCell.SetBackward(tempBackward);
        swapCell.SetLeft(this.gameObject);
        swapCell.SetRight(tempRight);

        ChangingPositions(swapObject, swapCell);
    }

    public void SwapRight()
    {
        if (right == null)
            return;

        SetTempValues();

        GameObject swapObject = right;
        SwapCell swapCell = swapObject.GetComponent<SwapCell>();

        SetForwardAdjacentsValue(swapObject);
        SetLeftAdjacentsValue(swapObject);
        SetBackwardAdjacentsValue(swapObject);

        swapCell.SetForwardAdjacentsValue(gameObject);
        swapCell.SetRightAdjacentsValue(gameObject);
        swapCell.SetBackwardAdjacentsValue(gameObject);

        SetForward(swapCell.GetForward());
        SetLeft(swapObject);
        SetRight(swapCell.GetRight());
        SetBackward(swapCell.GetBackward());

        swapCell.SetForward(tempForward);
        swapCell.SetBackward(tempBackward);
        swapCell.SetLeft(tempLeft);
        swapCell.SetRight(this.gameObject);

        ChangingPositions(swapObject, swapCell);
    }
    
    public void SwapForward()
    {
        if (forward == null)
            return;

        SetTempValues();

        GameObject swapObject = forward;
        SwapCell swapCell = swapObject.GetComponent<SwapCell>();

        SetBackwardAdjacentsValue(swapObject);
        SetLeftAdjacentsValue(swapObject);
        SetRightAdjacentsValue(swapObject);

        swapCell.SetForwardAdjacentsValue(gameObject);
        swapCell.SetLeftAdjacentsValue(gameObject);
        swapCell.SetRightAdjacentsValue(gameObject);

        SetForward(swapCell.GetForward());
        SetLeft(swapCell.GetLeft());
        SetRight(swapCell.GetRight());
        SetBackward(swapObject);

        swapCell.SetForward(gameObject);
        swapCell.SetBackward(tempBackward);
        swapCell.SetLeft(tempLeft);
        swapCell.SetRight(tempRight);

        ChangingPositions(swapObject, swapCell); 
    }
    public void SetRightAdjacentsValue(GameObject cell)
    {
        if(GetComponent<SwapCell>().GetRight() != null)
            GetComponent<SwapCell>().GetRight().GetComponent<SwapCell>().SetLeft(cell);
    }
    public void SetLeftAdjacentsValue(GameObject cell)
    {
        if (GetComponent<SwapCell>().GetLeft() != null)
            GetComponent<SwapCell>().GetLeft().GetComponent<SwapCell>().SetRight(cell);
    }
    public void SetForwardAdjacentsValue(GameObject cell)
    {
        if (GetComponent<SwapCell>().GetForward() != null)
            GetComponent<SwapCell>().GetForward().GetComponent<SwapCell>().SetBackward(cell);
    }
    public void SetBackwardAdjacentsValue(GameObject cell)
    {
        if (GetComponent<SwapCell>().GetBackward() != null)
            GetComponent<SwapCell>().GetBackward().GetComponent<SwapCell>().SetForward(cell);
    }
    private void ChangingPositions(GameObject swapObject, SwapCell swapCell)
    {
        if (swapCell.GetIsPositionTrue().Equals(true))
        {
            SwapPuzzle.shiftedCellCount++;
            swapCell.SetIsPositionTrue(false);
            if (isPositionTrue.Equals(true))
            {
                SwapPuzzle.shiftedCellCount++;
                SetIsPositionTrue(false);
            }

        }
        else
        {
            if (isPositionTrue.Equals(true))
            {
                SwapPuzzle.shiftedCellCount++;
                SetIsPositionTrue(false);
            }
            else
            {
                if (truePosition.Equals(swapObject.GetComponent<RectTransform>().anchoredPosition))
                {
                    SwapPuzzle.shiftedCellCount--;
                    SetIsPositionTrue(true);
                    if (swapCell.GetTruePosition().Equals(gameObject.GetComponent<RectTransform>().anchoredPosition))
                    {
                        SwapPuzzle.shiftedCellCount--;
                        swapCell.SetIsPositionTrue(true);
                    }

                }
                else
                {
                    if (swapCell.GetTruePosition().Equals(gameObject.GetComponent<RectTransform>().anchoredPosition))
                    {
                        SwapPuzzle.shiftedCellCount--;
                        swapCell.SetIsPositionTrue(true);
                    }
                }
            }
        }
        Vector2 tempPosition = gameObject.GetComponent<RectTransform>().anchoredPosition;
        gameObject.GetComponent<RectTransform>().anchoredPosition = swapObject.GetComponent<RectTransform>().anchoredPosition;
        swapObject.GetComponent<RectTransform>().anchoredPosition = tempPosition;
    }
}
