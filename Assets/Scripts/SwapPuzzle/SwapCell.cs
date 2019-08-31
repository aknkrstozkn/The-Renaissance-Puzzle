using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCell : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isPositionTrue;
    private Vector3 truePosition;

    private GameObject forward;
    private GameObject tempForward;
    private GameObject backward;
    private GameObject tempBackward;
    private GameObject right;
    private GameObject tempRight;
    private GameObject left;
    private GameObject tempLeft;

    private GameObject swapObject;
    private SwapCell swapCell;
    public void SwapForward()
    {
        if (forward.Equals(null))
            return;

        SetTempValues();
                
        SwapCell forwardCell = tempForward.GetComponent<SwapCell>();
        backward = forward;
        left = forwardCell.GetLeft();
        right = forwardCell.GetRight();
        forward = forwardCell.GetForward();

        forwardCell.SetForward(this.gameObject);
        forwardCell.SetBackward(tempBackward);
        forwardCell.SetLeft(tempLeft);
        forwardCell.SetRight(tempRight);

        if (forwardCell.GetIsPositionTrue().Equals(true))
        {
            SwapPuzzle.shiftedCellCount++;
            forwardCell.SetIsPositionTrue(false);
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
                forwardCell.SetIsPositionTrue(false);
            }
            else
            {
                if (truePosition.Equals(tempForward.transform.position))
                {
                    SwapPuzzle.shiftedCellCount--;
                    SetIsPositionTrue(true);
                    if (forwardCell.GetTruePosition().Equals(gameObject.transform.position))
                    {
                        SwapPuzzle.shiftedCellCount--;
                        forwardCell.SetIsPositionTrue(true);
                    }

                }
                else
                {
                    if (forwardCell.GetTruePosition().Equals(gameObject.transform.position))
                    {
                        SwapPuzzle.shiftedCellCount--;
                        forwardCell.SetIsPositionTrue(true);
                    }
                }
            }
        }
        Vector3 tempPosition = gameObject.transform.position;
        gameObject.transform.position = tempForward.transform.position;
        tempForward.transform.position = tempPosition;
    }
    
    private void SetTempValues()
    {
        tempForward = forward;
        tempBackward = backward;
        tempLeft = left;
        tempRight = right;
    }
    public Vector3 GetTruePosition()
    {
        return truePosition;
    }
    public void SetTruePosition(Vector3 truePosition)
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
        if (backward.Equals(null))
            return;

        SetTempValues();

        swapObject = backward;
        swapCell = swapObject.GetComponent<SwapCell>();
        forward = backward;
        left = swapCell.GetLeft();
        right = swapCell.GetRight();
        backward = swapCell.GetBackward();

        swapCell.SetForward(tempForward);
        swapCell.SetBackward(this.gameObject);
        swapCell.SetLeft(tempLeft);
        swapCell.SetRight(tempRight);

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
                if (truePosition.Equals(swapObject.transform.position))
                {
                    SwapPuzzle.shiftedCellCount--;
                    SetIsPositionTrue(true);
                    if (swapCell.GetTruePosition().Equals(gameObject.transform.position))
                    {
                        SwapPuzzle.shiftedCellCount--;
                        swapCell.SetIsPositionTrue(true);
                    }

                }
                else
                {
                    if (swapCell.GetTruePosition().Equals(gameObject.transform.position))
                    {
                        SwapPuzzle.shiftedCellCount--;
                        swapCell.SetIsPositionTrue(true);
                    }
                }
            }
        }
        Vector3 tempPosition = gameObject.transform.position;
        gameObject.transform.position = swapObject.transform.position;
        swapObject.transform.position = tempPosition;
    }

    public void SwapLeft()
    {
        if (left.Equals(null))
            return;

        SetTempValues();

        swapObject = left;
        swapCell = swapObject.GetComponent<SwapCell>();

        forward = swapCell.GetForward();
        left = swapCell.GetLeft();
        right = swapObject;
        backward = swapCell.GetBackward();

        swapCell.SetForward(tempForward);
        swapCell.SetBackward(tempBackward);
        swapCell.SetLeft(this.gameObject);
        swapCell.SetRight(tempRight);

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
                if (truePosition.Equals(swapObject.transform.position))
                {
                    SwapPuzzle.shiftedCellCount--;
                    SetIsPositionTrue(true);
                    if (swapCell.GetTruePosition().Equals(gameObject.transform.position))
                    {
                        SwapPuzzle.shiftedCellCount--;
                        swapCell.SetIsPositionTrue(true);
                    }

                }
                else
                {
                    if (swapCell.GetTruePosition().Equals(gameObject.transform.position))
                    {
                        SwapPuzzle.shiftedCellCount--;
                        swapCell.SetIsPositionTrue(true);
                    }
                }
            }
        }
        Vector3 tempPosition = gameObject.transform.position;
        gameObject.transform.position = swapObject.transform.position;
        swapObject.transform.position = tempPosition;
    }
}
