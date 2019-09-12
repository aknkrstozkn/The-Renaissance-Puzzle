using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SpriteGlow;
using System;

public class SwapPuzzle
{
    //Errors.....
    public static int invertedCellCount;
    public static int shiftedCellCount;
    //Random positions of cells
    private Dictionary<Vector2, GameObject> randomPositionsOfCells;
    private ArrayList truePositionsOfCells;
    private ArrayList falsePositionsOfCells;
    public static GameObject selectedCell;
    //Complexity
    private float complexityFactor;
    //To Check if rotate is enable
    public static bool isRotateEnabled;
    //Painting values
    private float pixel;
    private float heightSteps;
    private float widthSteps;
    //Storing cells
    private GameObject[] cells;
    private Texture2D[] pieces;
    //Values to build cells
    private Sprite painting;
    private int paintingIndex;
    private GameObject cellsParent;
    private int pieceCount;
    //To generate random angels for Cells
    private readonly int[] zDegrees = { 90, 180, 270 };
    //Shader for glowing effect
    private Shader glowShader;


    public void SetComplexityFactor(float complexityFactor)
    {
        this.complexityFactor = complexityFactor;
    }

    public float GetComplexityFactor()
    {
        return complexityFactor;
    }

    public int GetPieceCount()
    {
        return pieceCount;
    }

    public void SetPieceCount(int pieceCount)
    {
        this.pieceCount = pieceCount;
    }

    public int GetPaintingIndex()
    {
        return paintingIndex;
    }

    public void SetPaintingIndex(int paintingIndex)
    {
        this.paintingIndex = paintingIndex;
    }

    public GameObject[] GetCells()
    {
        return cells;
    }

    public SwapPuzzle(float complexityFactor, bool isRotateEnabled, int paintingIndex, int pieceCount, GameObject cellsParent, Shader glowShader)
    {
        SwapPuzzle.isRotateEnabled = isRotateEnabled;

        this.paintingIndex = paintingIndex;        
        this.pieceCount = pieceCount;
        this.cellsParent = cellsParent;
        this.glowShader = glowShader;
        this.complexityFactor = complexityFactor;

        painting = MainMenuManager.paintings[paintingIndex];
        cells = new GameObject[pieceCount];        
        pixel = CalculatePixel();
        heightSteps = GetSteps((float)painting.texture.height / 100f);
        widthSteps = GetSteps((float)painting.texture.width / 100f);
        invertedCellCount = pieceCount;
        shiftedCellCount = pieceCount;

        pieces = new Texture2D[pieceCount];

        truePositionsOfCells = new ArrayList();
        falsePositionsOfCells = new ArrayList();
    }
    
    private void BuildCells(bool isLoading = false, float[,] positions = null, float[] rotations = null)
    {
        ArrayList tempFalsePositionIndexes = null;
        if (isLoading.Equals(false))
            tempFalsePositionIndexes = new ArrayList(falsePositionsOfCells);

        //Saving position of cells to check adjacents
        randomPositionsOfCells = new Dictionary<Vector2, GameObject>();
        //Cell glow width
        int maxGlowWidth = pixel < 1 ? 3 :
                (pixel < 3 ? 6 : 8);
        //To generate random cell position    
        //Position properties for sprites of cells
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        Rect rec = new Rect(0, 0, pixel * 100, pixel * 100);
        Vector2 position;
        Quaternion rotation;
        for (float i = 0; i < widthSteps; i++)
        {
            float xCordinate = (pixel * i) + (pixel / 2f);
            for (float j = 0; j < heightSteps; j++)
            {
                int index = (int)((i * heightSteps) + j);
                float yCordinate = (pixel * j) + (pixel / 2f);
                Vector2 truePosition = new Vector2(xCordinate, yCordinate);

                if (isLoading.Equals(false))
                {
                    if (tempFalsePositionIndexes.Contains(index))
                    {
                        position = GenareteRandomPosition(index);
                        rotation = Quaternion.Euler(0, 0, zDegrees[UnityEngine.Random.Range(0, 3)]);
                        Debug.Log("F: " + index);
                    }
                    else
                    {
                        position = truePosition;
                        rotation = Quaternion.Euler(0, 0, 0);
                        Debug.Log("T: " + index);
                    }
                }
                else
                {
                    position = new Vector2(positions[index, 0], positions[index, 1]);
                    rotation = Quaternion.Euler(0, 0, rotations[index]);
                }
                
                BuilCell(position, rotation, index, i, j, pivot, rec, maxGlowWidth);
                randomPositionsOfCells.Add(position, cells[index]); 
            }            
        }
        
    }
    private Vector2 GenareteRandomPosition(int index)
    {
        Vector2 position;
        if (falsePositionsOfCells.Contains(index) && falsePositionsOfCells.Count > 1)
        {
            falsePositionsOfCells.Remove(index);
            int tempIndex = UnityEngine.Random.Range(0, falsePositionsOfCells.Count);
            position = IndexToPosition((int)falsePositionsOfCells[tempIndex]);
            falsePositionsOfCells.RemoveAt(tempIndex);
            falsePositionsOfCells.Add(index);
        }
        else
        {
            int tempIndex = UnityEngine.Random.Range(0, falsePositionsOfCells.Count);
            position = IndexToPosition((int)falsePositionsOfCells[tempIndex]);
            falsePositionsOfCells.RemoveAt(tempIndex);
        }
        return position;
    }
    private Vector2 IndexToPosition(int index)
    {
        int i = index / (int)heightSteps;
        int j = index % (int)heightSteps;
        float xCordinate = (i * pixel) + (pixel / 2f);
        float yCordinate = (j * pixel) + (pixel / 2f);
        
        return new Vector2(xCordinate, yCordinate);
    }
    private void SetFalsePositionsOfCells()
    {
        int tempPieceCount = pieceCount;
        int falseCount = (int)Math.Round(pieceCount / complexityFactor);

        if (isRotateEnabled.Equals(true))
            invertedCellCount = falseCount;
        else
            invertedCellCount = 0;
        shiftedCellCount = falseCount;
        
        ArrayList indexes = new ArrayList();

        for (int i = 0; i < pieceCount; i++)
            indexes.Add(i);        
        
        for (int i = 0; i < falseCount; i++)
        {
            int tempIndex = UnityEngine.Random.Range(0, tempPieceCount);
            falsePositionsOfCells.Add((int)indexes[tempIndex]);
            
            indexes.RemoveAt(tempIndex);
            tempPieceCount--;
        }        
    }
    private void CellPositions(float tempComplexityFactor)
    {   
        Vector2 position;
        for (float i = 0; i < widthSteps; i++)
        {
            float xCordinate = (pixel * i) + (pixel / 2f);
            for (float j = 0; j < heightSteps; j++)
            {
                int index = (int)((i * heightSteps) + j) + 1;
                float yCordinate = (pixel * j) + (pixel / 2f);

                position = new Vector2(x: xCordinate, y: yCordinate);
                if (index < complexityFactor)
                    truePositionsOfCells.Add(position);
                else
                {
                    falsePositionsOfCells.Add(position);
                    complexityFactor += tempComplexityFactor;
                }        
            }
        }
        
    }
    private void BuilCell(Vector2 randomCellPosition, Quaternion rotation, int index, float i, float j, Vector2 pivot, Rect rec, int maxGlowWidth)
    {
        //Cordinates for cell
        float xCordinate = (pixel * i) + (pixel / 2f);
        float yCordinate = (pixel * j) + (pixel / 2f);

        Debug.Log("index: " + index.ToString());
        Debug.Log("truePosition: " + new Vector2(xCordinate, yCordinate).ToString());
        Debug.Log("randomPosition: " + randomCellPosition.ToString());
        Debug.Log("rotation: " + rotation.ToString());

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
            rotation);
        else
            cell.transform.SetPositionAndRotation(randomCellPosition,
            Quaternion.Euler(0,0,0));


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
        swapCell.SetMaxGlowWidth(maxGlowWidth);
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

        SetFalsePositionsOfCells();
        BuildCells();

        SetSwapCellsAdjacents();

        selectedCell = randomPositionsOfCells[new Vector2((pixel / 2f), (pixel / 2f))];
        selectedCell.GetComponent<SwapCell>().SetPuzzlePieceGlow(true);
    }
    public void LoadSwapPuzzle(SwapPuzzleData data)
    {
        SetCellsParentSprite();

        if (data.GetIsRotateEnable().Equals(true))
            invertedCellCount = data.GetInvertedCount();
        else
            invertedCellCount = 0;

        shiftedCellCount = data.GetShiftedCount();

        BuildCells(true, data.GetRandomPozitions(), data.GetRotations());

        SetSwapCellsAdjacents();

        selectedCell = cells[data.GetIndexOfSelectedPuzzle()];
        selectedCell.GetComponent<SwapCell>().SetPuzzlePieceGlow(true);
    }
    private void SetSwapCellsAdjacents()
    {
        foreach (GameObject cell in cells)
        {
            SwapCell swapCell = cell.GetComponent<SwapCell>();

            float xCordinate = cell.GetComponent<RectTransform>().anchoredPosition.x;
            float yCordinate = cell.GetComponent<RectTransform>().anchoredPosition.y;
            

            if (randomPositionsOfCells.ContainsKey(new Vector2(xCordinate, yCordinate + pixel)))
                swapCell.SetForward(randomPositionsOfCells[new Vector2(xCordinate, yCordinate + pixel)]);         
               
            if(randomPositionsOfCells.ContainsKey(new Vector2(xCordinate, yCordinate - pixel)))
                swapCell.SetBackward(randomPositionsOfCells[new Vector2(xCordinate, yCordinate - pixel)]);

            if(randomPositionsOfCells.ContainsKey(new Vector2(xCordinate + pixel, yCordinate)))
                swapCell.SetRight(randomPositionsOfCells[new Vector2(xCordinate + pixel, yCordinate)]);

            if (randomPositionsOfCells.ContainsKey(new Vector2(xCordinate - pixel, yCordinate)))
                swapCell.SetLeft(randomPositionsOfCells[new Vector2(xCordinate - pixel, yCordinate)]);

        }
    }

    

    private float CalculatePixel()
    {
        float area = ((float)painting.texture.height) * ((float)painting.texture.width);
        return Mathf.Sqrt(area / (float)(pieceCount)) / 100f;
    }
    private float GetSteps(float dimension)
    {
        return (dimension / pixel);
    }
    private void SetCellsParentSprite()
    {
        cellsParent.GetComponent<RectTransform>().sizeDelta =
            new Vector2(painting.texture.width / 100, painting.texture.height / 100);
        cellsParent.GetComponent<SpriteRenderer>().sprite = painting;
    }   
}



