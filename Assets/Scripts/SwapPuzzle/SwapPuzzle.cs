using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SpriteGlow;

public class SwapPuzzle
{
    public static int invertedCellCount;
    public static int shiftedCellCount;

    private bool isRotateEnabled;

    private int pixel;
    private int heightSteps;
    private int widthSteps;

    private GameObject[] cells;
    private Texture2D[] pieces;

    private Sprite painting;
    private GameObject cellsParent;
    private int pieceCount;

    private readonly int[] zDegrees = { 0, 90, 180, 270 };

    public Material glowMaterial;
    public Shader glowShader;

    public GameObject[] GetCells()
    {
        return cells;
    }
    public SwapPuzzle(bool isRotateEnabled, Sprite painting, int pieceCount, GameObject cellsParent, Material glowMaterial, Shader glowShader)
    {
        this.isRotateEnabled = isRotateEnabled;
        
        this.painting = painting;
        this.pieceCount = pieceCount;
        this.cellsParent = cellsParent;
        this.glowMaterial = glowMaterial;
        this.glowShader = glowShader;

        cells = new GameObject[pieceCount];        
        pixel = CalculatePixel();
        heightSteps = GetSteps(painting.texture.height / 100);
        widthSteps = GetSteps(painting.texture.width / 100);

        invertedCellCount = pieceCount;
        shiftedCellCount = pieceCount;

        pieces = new Texture2D[pieceCount];
    }
    private void BuildCells()    {
        //Cell properties
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        Rect rec = new Rect(0, 0, pixel * 100, pixel * 100);
        //------------------------------
        Vector3[] ramdomCellPosition = randomCellPositions();
        for (int i = 0; i < widthSteps; i++)
            for (int j = 0; j < heightSteps; j++)
            {
                int index = (i * heightSteps) + j;

                BuilCell(ramdomCellPosition[index], index, i, j, pivot, rec);
            }
        
    }

    private Vector3[] randomCellPositions()
    {
        ArrayList indexes = new ArrayList();
        for (int k = 0; k < pieceCount; k++)
            indexes.Add(k);
                
        int tempPieceCount = pieceCount;

        Vector3[] cellPositions = new Vector3[pieceCount];
        for (int i = 0; i < widthSteps; i++)
            for (int j = 0; j < heightSteps; j++)
            {
                int tempIndex = Random.Range(0, tempPieceCount);
                int index = (int)indexes[tempIndex];
                indexes.RemoveAt(tempIndex);
                tempPieceCount--;

                float xCordinate = (pixel * i) + ((float)pixel / 2);
                float yCordinate = (pixel * j) + ((float)pixel / 2);

                cellPositions.SetValue(new Vector3(xCordinate, yCordinate, 0f), index);
            }
        return cellPositions;
    }

    private RuntimeAnimatorController animatorController = null;
    private void BuilCell(Vector3 randomCellPosition, int index, int i, int j, Vector2 pivot, Rect rec)
    {
        //Cordinates for cell
        float xCordinate = (pixel * i) + ((float)pixel / 2);
        float yCordinate = (pixel * j) + ((float)pixel / 2);
        //Creating cells
        GameObject cell = new GameObject();       
       
        //Bounding with script
        cell.AddComponent<TouchRotate>();
        //---------------------

        RectTransform rectTransform = cell.AddComponent<RectTransform>() as RectTransform;
        BoxCollider2D box2D = cell.AddComponent<BoxCollider2D>() as BoxCollider2D;
        //Creating with z: +90 position, because default z position is -180 and we want it to be 0
        if (isRotateEnabled)
            cell.transform.SetPositionAndRotation(randomCellPosition,
            Quaternion.Euler(0, 0, zDegrees[Random.Range(0, 4)]));
        else
            cell.transform.SetPositionAndRotation(randomCellPosition,
            Quaternion.Euler(0, 0, 0));

        //Naming Cell and Setting his parent
        cell.name = "cell_" + xCordinate.ToString() + "x" + yCordinate.ToString();
        cellsParent.name = "Parent_Of_Cells\\" + pieceCount.ToString() + "-Children";
        cell.transform.SetParent(cellsParent.transform);
        //Scalling
        rectTransform.sizeDelta = new Vector2(pixel, pixel);
        box2D.size = new Vector2(pixel, pixel);
        cell.transform.localScale = new Vector3(1, 1, 1);
        //Positioning; anchorMin and anchorMax are for starting position bottom left corner of parent.
        rectTransform.anchorMin = new Vector2(0f, 0f);
        rectTransform.anchorMax = new Vector2(0f, 0f);
        rectTransform.anchoredPosition = new Vector3(x: xCordinate, y: yCordinate);
        //Creating Sprite Texture---------------------------------------------------------------------
        Texture2D spriteTexture = new Texture2D(pixel * 100, pixel * 100);
        var pixels = painting.texture.GetPixels((pixel * 100) * i, (pixel * 100) * j, (pixel * 100), (pixel * 100));        
        spriteTexture.SetPixels(pixels);
        spriteTexture.Apply();
        //------------------------------------------------------------------------------------
        //Setting sprite to Cell        
        SpriteRenderer spriteRenderer = cell.AddComponent<SpriteRenderer>() as SpriteRenderer;
        spriteRenderer.sprite = Sprite.Create(spriteTexture, rec, pivot);
        spriteRenderer.sortingLayerName = "Cells";
        spriteRenderer.material = glowMaterial;
        spriteRenderer.material.shader = glowShader;
        SpriteGlowEffect glowEffect = cell.AddComponent<SpriteGlowEffect>();

        glowEffect.OutlineWidth = 0;
        glowEffect.AlphaThreshold = 0.01f;

        //Finally, adding our cell to the list.
        cells.SetValue(cell, index);
    }
    public void BuildRotatePuzzle()
    {
        SetCellsParentSprite();
        BuildCells();
        CountFalseCells();

        //cellsParent.enabled = false;

    }
    
    
    private void CountFalseCells()
    {
        foreach (GameObject cell in cells)
        {
            if (cell.transform.eulerAngles.z < 90)
            {
                invertedCellCount--;
            }
        }

    }
    private int CalculatePixel()
    {
        int area = (painting.texture.height / 100) * (painting.texture.width / 100);
        return (int)Mathf.Sqrt(area / pieceCount);
    }
    private int GetSteps(int dimension)
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



