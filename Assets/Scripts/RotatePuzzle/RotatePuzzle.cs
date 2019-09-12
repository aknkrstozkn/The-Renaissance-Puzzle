using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SpriteGlow;
using System;

public class RotatePuzzle
{
    public static int falseCellCount;
    
    private float pixel;
    private float heightSteps;
    private float widthSteps;

    private GameObject[] cells;
    private Texture2D[] pieces;

    private Sprite painting;
    private int paintingIndex;
    private GameObject cellsParent;
    private int pieceCount;
    private readonly int[] zDegrees = {90, 180, 270};
    private ArrayList falseRotationsOfCells;
    private float complexityFactor;

    public Shader glowShader;


    public int GetPieceCount()
    {
        return pieceCount;
    }

    public float GetComplexityFactor()
    {
        return complexityFactor;
    }

    public int GetPaintingIndex()
    {
        return paintingIndex;
    }

    public GameObject[] GetCells()
    {
        return cells;
    }

    public RotatePuzzle(float complexityFactor, int paintingIndex, int pieceCount, GameObject cellsParent, Shader glowShader)
    {
        this.paintingIndex = paintingIndex;
        this.pieceCount = pieceCount;
        this.cellsParent = cellsParent;
        this.glowShader = glowShader;
        this.complexityFactor = complexityFactor;

        painting = MainMenuManager.paintings[paintingIndex];
        cells = new GameObject[pieceCount];        
        pixel = CalculatePixel();
        heightSteps = GetSteps(painting.texture.height / 100);
        widthSteps = GetSteps(painting.texture.width / 100);
        falseCellCount = pieceCount;
        pieces = new Texture2D[pieceCount];

        falseRotationsOfCells = new ArrayList();
    }
    private void SetFalsePositionsOfCells()
    {
        int tempPieceCount = pieceCount;
        int falseCount = (int)Math.Round(pieceCount / complexityFactor);
        falseCellCount = falseCount;

        ArrayList indexes = new ArrayList();

        for (int i = 0; i < pieceCount; i++)
            indexes.Add(i);

        for (int i = 0; i < falseCount; i++)
        {
            int tempIndex = UnityEngine.Random.Range(0, tempPieceCount);
            falseRotationsOfCells.Add((int)indexes[tempIndex]);

            indexes.RemoveAt(tempIndex);
            tempPieceCount--;
        }
    }
    private void BuildCells(bool isLoading = false, float[] rotations = null)
    {
        ArrayList tempFalseRotationsIndexes = null;
        if (isLoading.Equals(false))
            tempFalseRotationsIndexes = new ArrayList(falseRotationsOfCells);
        //Cell properties
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        Rect rec = new Rect(0, 0, pixel * 100, pixel * 100);
        //------------------------------
        Quaternion rotation;
        for (float i = 0; i < widthSteps; i++)
            for (float j = 0; j < heightSteps; j++)
            {
                int index = (int)((i * heightSteps) + j);

                if (isLoading.Equals(false))                
                    if (tempFalseRotationsIndexes.Contains(index))                      
                        rotation = Quaternion.Euler(0, 0, zDegrees[UnityEngine.Random.Range(0, 3)]);                   
                    else                    
                        rotation = Quaternion.Euler(0, 0, 0);              
                else
                    rotation = Quaternion.Euler(0, 0, rotations[index]);

                BuilCell(rotation, index, i, j, pivot, rec);
            }        
    }
    
    private void BuilCell(Quaternion rotation, int index, float i, float j, Vector2 pivot, Rect rec)
    {
        //----------------------Cordinates for cell--------------------------------------
        float xCordinate = (pixel * i) + (pixel / 2);
        float yCordinate = (pixel * j) + (pixel / 2);
        
        //Creating cells-----------------------------------------------------------------
        GameObject cell = new GameObject();       
       
        //-------------------------Bounding with script----------------------------------
        cell.AddComponent<TouchRotate>();
        //-------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------
        RectTransform rectTransform = cell.AddComponent<RectTransform>() as RectTransform;
        BoxCollider2D box2D = cell.AddComponent<BoxCollider2D>() as BoxCollider2D;
        
        //Creating with z: +90 position, because default z position is -180 and we want it to be 0
        cell.transform.SetPositionAndRotation(new Vector3(0, 0, 0),
            rotation);
        
        //Naming Cell and Setting his parent----------------------------------------
        cell.name = "cell_" + xCordinate.ToString() + "x" + yCordinate.ToString();
        cellsParent.name = "Parent_Of_Cells\\" + pieceCount.ToString() + "-Children";
        cell.transform.SetParent(cellsParent.transform);
        
        //Scalling------------------------------------------
        rectTransform.sizeDelta = new Vector2(pixel, pixel);
        box2D.size = new Vector2(pixel, pixel);
        cell.transform.localScale = new Vector3(1, 1, 1);
        
        //Positioning; anchorMin and anchorMax are for starting position bottom left corner of parent.
        rectTransform.anchorMin = new Vector2(0f, 0f);
        rectTransform.anchorMax = new Vector2(0f, 0f);
        rectTransform.anchoredPosition = new Vector3(x: xCordinate, y: yCordinate);
        
        //------------------------------------------Creating Sprite Texture--------------------------------------------------------------------
        Texture2D spriteTexture = new Texture2D((int)(pixel * 100f), (int)(pixel * 100f));
        var pixels = painting.texture.GetPixels((int)((pixel * 100f) * i), (int)((pixel * 100f) * j), (int)(pixel * 100f), (int)(pixel * 100f));        
        spriteTexture.SetPixels(pixels);
        spriteTexture.Apply();

        
        //Setting sprite to Cell--------------------------------------------------------------        
        SpriteRenderer spriteRenderer = cell.AddComponent<SpriteRenderer>() as SpriteRenderer;
        spriteRenderer.sprite = Sprite.Create(spriteTexture, rec, pivot);
        spriteRenderer.sortingLayerName = "Cells";
        spriteRenderer.material.shader = glowShader;
        SpriteGlowEffect glowEffect = cell.AddComponent<SpriteGlowEffect>();

        //--------------------------------
        glowEffect.OutlineWidth = 0;
        glowEffect.AlphaThreshold = 0.01f;

        //-----------------------
        //Finally, adding our cell to the list.
        cells.SetValue(cell, index);
    }     

    public void BuildRotatePuzzle()
    {
        SetFalsePositionsOfCells();
        SetCellsParentSprite();
        BuildCells();
    }

    public void LoadRotatePuzzle(RotatePuzzleData data)
    {
        falseCellCount = data.GetInvertedCount();

        SetCellsParentSprite();
        BuildCells(true, data.GetRotations());
    }

  

    private float CalculatePixel()
    {
        float area = ((float)painting.texture.height / 100f) * ((float)painting.texture.width / 100f);
        return Mathf.Sqrt(area / (float)pieceCount);
    }

    private float GetSteps(float dimension)
    {
        return dimension / pixel;
    }

    private void SetCellsParentSprite()
    {
        cellsParent.GetComponent<RectTransform>().sizeDelta =
            new Vector2(painting.texture.width / 100, painting.texture.height / 100);
        cellsParent.GetComponent<SpriteRenderer>().sprite = painting;
    }   
}



