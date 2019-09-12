using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RotatePuzzleData
{
    private float[] rotations;
    private int invertedCount;

    private int paintIndex;
    private int pieceCount;

    private float complexityFactor;


    public RotatePuzzleData(RotatePuzzle rotatePuzzle)
    {
        GameObject[] cells = rotatePuzzle.GetCells();
        rotations = new float[cells.Length];
        for (int i = 0; i < cells.Length; i++)
        {
            rotations.SetValue(cells[i].transform.eulerAngles.z, i);
        }
        
        this.invertedCount = RotatePuzzle.falseCellCount;
        this.paintIndex = rotatePuzzle.GetPaintingIndex();
        this.pieceCount = rotatePuzzle.GetPieceCount();
        this.complexityFactor = rotatePuzzle.GetComplexityFactor();
        
    }

    public float GetComplexityFactor()
    {
        return complexityFactor;
    }

    public void SetComplexityFactor(float complexityFactor)
    {
        this.complexityFactor = complexityFactor;
    }

    public int GetInvertedCount()
    {
        return invertedCount;
    }

    public void SetInvertedCount(int invertedCount)
    {
        this.invertedCount = invertedCount;
    }

    public int GetPieceCount()
    {
        return pieceCount;
    }

    public void SetPieceCount(int pieceCount)
    {
        this.pieceCount = pieceCount;
    }

    public int GetPaintIndex()
    {
        return paintIndex;
    }

    public void SetPaintIndex(int paintIndex)
    {
        this.paintIndex = paintIndex;
    }

    public void SetRotations(float[] rotations)
    {
        this.rotations = rotations;
    }

    public float[] GetRotations()
    {
        return rotations;
    }
}
