using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RotatePuzzle
{
    public static int falseCellCount = 70;
    
    private int pixel;
    private int heightSteps;
    private int widthSteps;

    private Image[] cells;
    private Texture2D[] pieces;

    private Sprite painting;
    private Image cellPrefab;
    private Image cellsParent;
    private int pieceCount;

    private readonly int[] zDegrees = { 0, 90, 180, 270 };

    public Image[] GetCells()
    {
        return cells;
    }
    public RotatePuzzle(Sprite painting, int pieceCount, Image cellPrefab, Image cellsParent)
    {
        this.painting = painting;
        this.pieceCount = pieceCount;
        this.cellPrefab = cellPrefab;
        this.cellsParent = cellsParent;

        cells = new Image[pieceCount];        
        pixel = CalculatePixel();
        heightSteps = GetSteps(painting.texture.height);
        widthSteps = GetSteps(painting.texture.width);
        falseCellCount = pieceCount;
        pieces = new Texture2D[pieceCount];
        Debug.Log("cells: " + cells +
            " pixel: " + pixel +
            " heightSteps: " + heightSteps +
            " widthSteps: " + widthSteps +
            " falseCellCount: " + falseCellCount);
    }
    private void BuildPieces()    {

        for (int i = 0; i < widthSteps; i++)
        {
            for (int j = 0; j < heightSteps; j++)
            {
                int index = (i * heightSteps) + j;
                pieces[index] = new Texture2D(pixel, pixel);
                var pixels = painting.texture.GetPixels(pixel * i, pixel * j, pixel, pixel);
                pieces[index].SetPixels(pixels);
                pieces[index].Apply();
            }
        }
    }
    public void BuildRotatePuzzle()
    {
        SetCellsParentSprite();
        BuildPieces();
        BuildCells();
        CountFalseCells();

        //cellsParent.enabled = false;

    }
    private void BuildCells()
    {
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        Rect rec = new Rect(0, 0, pieces[0].width, pieces[0].height);
        for (int i = 0; i < widthSteps; i++)
        {
            for (int j = 0; j < heightSteps; j++)
            {
                int index = (i * heightSteps) + j;

                Image cell = Image.Instantiate(cellPrefab, new Vector3(0, 0, +90)
                    , Quaternion.Euler(0, 0, zDegrees[Random.Range(0, 4)]));
                cell.transform.SetParent(cellsParent.transform);
                cell.GetComponent<RectTransform>().sizeDelta = new Vector2(pixel, pixel);                
                cell.transform.localScale = new Vector3(1, 1, 1);
                cell.rectTransform.anchoredPosition = new Vector2(x: (pixel * i) + ((float)pixel / 2),
                    y: (pixel * j) + ((float)pixel / 2));
                cell.sprite = Sprite.Create(pieces[index], rec, pivot);
                cells.SetValue(cell, index);
            }

        }
    }
    
    private void CountFalseCells()
    {
        foreach (Image cell in cells)
        {
            if (cell.transform.eulerAngles.z < 90)
            {
                falseCellCount--;
            }
        }

    }
    private int CalculatePixel()
    {
        int area = painting.texture.height * painting.texture.width;
        return (int)Mathf.Sqrt(area / pieceCount);
    }
    private int GetSteps(int dimension)
    {
        return dimension / pixel;
    }
    private void SetCellsParentSprite()
    {
        cellsParent.GetComponent<RectTransform>().sizeDelta =
            new Vector2(painting.texture.width, painting.texture.height);
        cellsParent.sprite = painting;
    }   
}



