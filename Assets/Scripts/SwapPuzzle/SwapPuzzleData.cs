using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SwapPuzzleData
{
    // Start is called before the first frame update
    private int indexOfSelectedPuzzle;
    private int invertedCount;
    private int shiftedCount;

    private float[,] randomPozitions;
    private float[,] truePozitions;
    private float[] rotations;
    private float complexityFactor;

    private int paintIndex;
    private int pieceCount;

    private bool isRotateEnable;

    public void SetComplexityFactor(float complexityFactor)
    {
        this.complexityFactor = complexityFactor;
    }

    public float GetComplexityFactor()
    {
        return complexityFactor;
    }

    public bool GetIsRotateEnable()
    {
        return isRotateEnable;
    }

    public void SetIsRotateEnable(bool isRotateEnable)
    {
        this.isRotateEnable = isRotateEnable;
    }

    public void SetPieceCount(int pieceCount)
    {
        this.pieceCount = pieceCount;
    }

    public int GetPieceCount()
    {
        return pieceCount;
    }

    public void SetPaintIndex(int paintIndex)
    {
        this.paintIndex = paintIndex;
    }

    public int GetPaintIndex()
    {
        return paintIndex;
    }

    public void SetRotations(float[] rotations)
    {
        this.rotations = rotations;
    }

    public float[] GetRotations()
    {
        return rotations;
    }

    public void SetTruePozitions(float[,] truePozitions)
    {
        this.truePozitions = truePozitions;
    }

    public float[,] GetTruePozitions()
    {
        return truePozitions;
    }

    public void SetRandompozitions(float[,] randomPozitions)
    {
        this.randomPozitions = randomPozitions;
    }

    public float[,] GetRandomPozitions()
    {
        return randomPozitions;
    }

    public void SetIndexOfSelectedPuzzle(int indexOfSelectedPuzzle)
    {
        this.indexOfSelectedPuzzle = indexOfSelectedPuzzle;
    }

    public int GetIndexOfSelectedPuzzle()
    {
        return indexOfSelectedPuzzle;
    }

    public void SetInvertedCount(int invertedCount)
    {
        this.invertedCount = invertedCount;
    }

    public int GetInvertedCount()
    {
        return invertedCount;
    }

    public void SetShiftedCount(int shiftedCount)
    {
        this.shiftedCount = shiftedCount;
    }

    public int GetShiftedCount()
    {
        return shiftedCount;
    }

    public SwapPuzzleData(SwapPuzzle swapPuzzle)
    {
        
        invertedCount = SwapPuzzle.invertedCellCount;
        shiftedCount = SwapPuzzle.shiftedCellCount;
        isRotateEnable = SwapPuzzle.isRotateEnabled;

        paintIndex = swapPuzzle.GetPaintingIndex();
        pieceCount = swapPuzzle.GetPieceCount();

        GameObject[] cells = swapPuzzle.GetCells();
        randomPozitions = new float[cells.Length, 2];
        truePozitions = new float[cells.Length, 2];
        rotations = new float[cells.Length];
        complexityFactor = swapPuzzle.GetComplexityFactor();

        for (int i = 0; i < cells.Length; i++)
        {
            GameObject cell = cells[i];

            float positionX = cell.GetComponent<RectTransform>().anchoredPosition.x;
            float positionY = cell.GetComponent<RectTransform>().anchoredPosition.y;            

            randomPozitions[i, 0] = positionX;
            randomPozitions[i, 1] = positionY;

            float truePositionY = cell.GetComponent<SwapCell>().GetTruePosition().y;
            float truePositionX = cell.GetComponent<SwapCell>().GetTruePosition().x;

            truePozitions[i, 0] = truePositionX;
            truePozitions[i, 1] = truePositionY;

            rotations[i] = cell.transform.rotation.z;

            if (cell.GetComponent<RectTransform>().anchoredPosition.Equals(
                SwapPuzzle.selectedCell.GetComponent<RectTransform>().anchoredPosition))
                indexOfSelectedPuzzle = i;
        }
    }
}   
