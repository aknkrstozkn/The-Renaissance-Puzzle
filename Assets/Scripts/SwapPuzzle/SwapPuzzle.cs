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

    public static Dictionary<Vector2, GameObject> positionOfCells;
    public static GameObject selectedCell;
    
    public static bool isRotateEnabled;

    private float pixel;
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
        SwapPuzzle.isRotateEnabled = isRotateEnabled;
        
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
        positionOfCells = new Dictionary<Vector2, GameObject>();
        //Cell properties
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        Rect rec = new Rect(0, 0, pixel * 100, pixel * 100);
        //------------------------------
        Vector2[] ramdomCellPosition = RandomCellPositions();
        for (float i = 0; i < widthSteps; i++)
        {
            
            float xCordinate = (pixel * i) + ((float)pixel / 2);
            for (float j = 0; j < heightSteps; j++)
            {
                int index = (int)((i * heightSteps) + j);
                float yCordinate = (pixel * j) + ((float)pixel / 2);

                BuilCell(ramdomCellPosition[index], index, i, j, pivot, rec);
                positionOfCells.Add(ramdomCellPosition[index], cells[index]);

            }
            
        }
        selectedCell = positionOfCells[new Vector2((pixel / 2f),(pixel / 2f))];
        selectedCell.GetComponent<SwapCell>().SetPuzzlePieceGlow(true);
    }

    private Vector2[] RandomCellPositions()
    {
        ArrayList indexes = new ArrayList();
        for (int k = 0; k < pieceCount; k++)
            indexes.Add(k);
                
        int tempPieceCount = pieceCount;

        Vector2[] cellPositions = new Vector2[pieceCount];
        for (float i = 0; i < widthSteps; i++)
        {
            float xCordinate = ((float)pixel * i) + ((float)pixel / 2f);
            for (float j = 0; j < heightSteps; j++)
            {
                int tempIndex = Random.Range(0, tempPieceCount);
                int index = (int)indexes[tempIndex];
                indexes.RemoveAt(tempIndex);
                tempPieceCount--;


                float yCordinate = ((float)pixel * j) + ((float)pixel / 2f);

                cellPositions.SetValue(new Vector2(xCordinate, yCordinate), index);
                
            }
        }
        return cellPositions;
    }

    private RuntimeAnimatorController animatorController = null;
    private void BuilCell(Vector2 randomCellPosition, int index, float i, float j, Vector2 pivot, Rect rec)
    {
        //Cordinates for cell
        float xCordinate = (pixel * i) + (pixel / 2f);
        float yCordinate = (pixel * j) + (pixel / 2f);
        //Creating cells
        GameObject cell = new GameObject();       
       
        //Bounding with script
        cell.AddComponent<TouchSwapCell>();        
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
        rectTransform.anchoredPosition = new Vector3(x: randomCellPosition.x, y: randomCellPosition.y);
        //Creating Sprite Texture---------------------------------------------------------------------
        Texture2D spriteTexture = new Texture2D((int)(pixel * 100f), (int)(pixel * 100f));
        var pixels = painting.texture.GetPixels((int)((pixel * 100) * i), (int)((pixel * 100) * j), (int)(pixel * 100f), (int)(pixel * 100f));        
        spriteTexture.SetPixels(pixels);
        spriteTexture.Apply();
        //------------------------------------------------------------------------------------
        //Setting sprite to Cell        
        SpriteRenderer spriteRenderer = cell.AddComponent<SpriteRenderer>() as SpriteRenderer;
        spriteRenderer.sprite = Sprite.Create(spriteTexture, rec, pivot);
        spriteRenderer.sortingLayerName = "Cells";
        spriteRenderer.material = glowMaterial;
        spriteRenderer.material.shader = glowShader;
        //GlowEffect----------------
        SpriteGlowEffect glowEffect = cell.AddComponent<SpriteGlowEffect>();
        glowEffect.OutlineWidth = 0;
        glowEffect.AlphaThreshold = 0.01f;
        //-------------------------
        //-----SwapCell Values---------
        SwapCell swapCell = cell.AddComponent<SwapCell>();
        swapCell.SetTruePosition(new Vector2(xCordinate, yCordinate));
        swapCell.SetIsPositionTrue(swapCell.GetTruePosition().Equals(randomCellPosition));
        swapCell.SetForward(null);
        swapCell.SetBackward(null);
        swapCell.SetLeft(null);
        swapCell.SetRight(null);
        //-----------------------
        //Finally, adding our cell to the list.
        cells.SetValue(cell, index);
    }
    public void BuildSwapPuzzle()
    {
        SetCellsParentSprite();
        BuildCells();
        CountFalseCells();
        SetSwapCellsAdjacents();
    }

    private void SetSwapCellsAdjacents()
    {
        foreach (GameObject cell in cells)
        {
            SwapCell swapCell = cell.GetComponent<SwapCell>();

            float xCordinate = cell.GetComponent<RectTransform>().anchoredPosition.x;
            float yCordinate = cell.GetComponent<RectTransform>().anchoredPosition.y;

            Debug.Log("x: " + xCordinate.ToString() + " y: " + yCordinate.ToString());

            

            if (positionOfCells.ContainsKey(new Vector2(xCordinate, yCordinate + pixel)))
                swapCell.SetForward(positionOfCells[new Vector2(xCordinate, yCordinate + pixel)]);         
               
            if(positionOfCells.ContainsKey(new Vector2(xCordinate, yCordinate - pixel)))
                swapCell.SetBackward(positionOfCells[new Vector2(xCordinate, yCordinate - pixel)]);

            if(positionOfCells.ContainsKey(new Vector2(xCordinate + pixel, yCordinate)))
                swapCell.SetRight(positionOfCells[new Vector2(xCordinate + pixel, yCordinate)]);

            if (positionOfCells.ContainsKey(new Vector2(xCordinate - pixel, yCordinate)))
                swapCell.SetLeft(positionOfCells[new Vector2(xCordinate - pixel, yCordinate)]);

        }
    }

    private void CountFalseCells()
    {
        foreach (GameObject cell in cells)
        {
            if (cell.transform.eulerAngles.z < 90)
                invertedCellCount--;
            if (cell.GetComponent<SwapCell>().GetIsPositionTrue().Equals(true))
                shiftedCellCount--;            
        }

    }
    private float CalculatePixel()
    {
        float area = ((float)painting.texture.height / 100f) * ((float)painting.texture.width / 100f);
        return Mathf.Sqrt(area / pieceCount);
    }
    private int GetSteps(float dimension)
    {
        return (int)(dimension / pixel);
    }
    private void SetCellsParentSprite()
    {
        cellsParent.GetComponent<RectTransform>().sizeDelta =
            new Vector2(painting.texture.width / 100, painting.texture.height / 100);
        cellsParent.GetComponent<SpriteRenderer>().sprite = painting;
    }   
}



