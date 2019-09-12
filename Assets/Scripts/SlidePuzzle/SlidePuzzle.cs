using SpriteGlow;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePuzzle
{
    //Errors.....
    public static int invertedCellCount;
    public static int shiftedCellCount;
    //Random positions of cells
    private Dictionary<Vector2, GameObject> randomPositionsOfCells;
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

    public SlidePuzzle(float complexityFactor, bool isRotateEnabled, int paintingIndex, int pieceCount, GameObject cellsParent, Shader glowShader)
    {
        SlidePuzzle.isRotateEnabled = isRotateEnabled;

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
        //cells[pieceCount - 1].GetComponent<SpriteRenderer>().sprite = null;
        cells[pieceCount - 1].GetComponent<SpriteRenderer>().color = Color.blue;


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
        int tempPieceCount = pieceCount - 1;
        int falseCount = (int)Math.Round(tempPieceCount / complexityFactor);

        if (isRotateEnabled.Equals(true))
            invertedCellCount = falseCount;
        else
            invertedCellCount = 0;
        shiftedCellCount = falseCount;

        ArrayList indexes = new ArrayList();

        for (int i = 0; i < tempPieceCount; i++)
            indexes.Add(i);

        for (int i = 0; i < falseCount; i++)
        {
            int tempIndex = UnityEngine.Random.Range(0, tempPieceCount);
            falsePositionsOfCells.Add((int)indexes[tempIndex]);

            indexes.RemoveAt(tempIndex);
            tempPieceCount--;
        }
    }
   
    private void BuilCell(Vector2 randomCellPosition, Quaternion rotation, int index, float i, float j, Vector2 pivot, Rect rec, int maxGlowWidth)
    {
        //Cordinates for cell
        float xCordinate = (pixel * i) + (pixel / 2f);
        float yCordinate = (pixel * j) + (pixel / 2f);

        //Creating cells
        GameObject cell = new GameObject();

        //Bounding with script
        if(isRotateEnabled.Equals(true))
            if(index != pieceCount -1)
                cell.AddComponent<TouchSlideCell>();
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
        //spriteRenderer.color = Color.blue;
        spriteRenderer.sortingLayerName = "Cells";
        spriteRenderer.material.shader = glowShader;
        //GlowEffect----------------
        SpriteGlowEffect glowEffect = cell.AddComponent<SpriteGlowEffect>();
        glowEffect.OutlineWidth = 0;
        glowEffect.AlphaThreshold = 0.01f;
        //-------------------------
        //-----SlideCell Values---------
        SlideCell slideCell = cell.AddComponent<SlideCell>();
        slideCell.SetTruePosition(new Vector2(xCordinate, yCordinate));
        slideCell.SetIsPositionTrue(slideCell.GetTruePosition().Equals(randomCellPosition));
        slideCell.SetMaxGlowWidth(maxGlowWidth);
        //-----------------------
        //Finally, adding our cell to the list.
        cells.SetValue(cell, index);
    }
    public void BuildSlidePuzzle()
    {
        SetCellsParentSprite();

        SetFalsePositionsOfCells();
        BuildCells();

        selectedCell = cells[pieceCount - 1];
    }
    public void LoadSlidePuzzle(SlidePuzzleData data)
    {
        SetCellsParentSprite();

        if (isRotateEnabled.Equals(true))
            invertedCellCount = data.GetInvertedCount();
        else
            invertedCellCount = 0;
        shiftedCellCount = data.GetShiftedCount();

        BuildCells(true, data.GetRandomPozitions(), data.GetRotations());

        selectedCell = cells[pieceCount - 1];
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

    public GameObject GetSelectedCellsForward()
    {
        float xCordinate = selectedCell.GetComponent<RectTransform>().anchoredPosition.x;
        float yCordinate = selectedCell.GetComponent<RectTransform>().anchoredPosition.y;

        Vector2 forwardLocation = new Vector2(xCordinate, y: yCordinate + pixel);
        
        if (randomPositionsOfCells.ContainsKey(forwardLocation))
            return randomPositionsOfCells[forwardLocation];
        else
            return null;
        
    }

    public GameObject GetSelectedCellsBackward()
    {
        float xCordinate = selectedCell.GetComponent<RectTransform>().anchoredPosition.x;
        float yCordinate = selectedCell.GetComponent<RectTransform>().anchoredPosition.y;

        Vector2 backwardLocation = new Vector2(xCordinate, y: yCordinate - pixel);

        if (randomPositionsOfCells.ContainsKey(backwardLocation))
            return randomPositionsOfCells[backwardLocation];
        else
            return null;

    }

    public GameObject GetSelectedCellsRight()
    {
        float xCordinate = selectedCell.GetComponent<RectTransform>().anchoredPosition.x;
        float yCordinate = selectedCell.GetComponent<RectTransform>().anchoredPosition.y;

        Vector2 rightLocation = new Vector2(x: xCordinate + pixel, y: yCordinate);

        if (randomPositionsOfCells.ContainsKey(rightLocation))
            return randomPositionsOfCells[rightLocation];
        else
            return null;

    }

    public GameObject GetSelectedCellsLeft()
    {
        float xCordinate = selectedCell.GetComponent<RectTransform>().anchoredPosition.x;
        float yCordinate = selectedCell.GetComponent<RectTransform>().anchoredPosition.y;

        Vector2 leftLocation = new Vector2(x: xCordinate - pixel, y: yCordinate);

        if (randomPositionsOfCells.ContainsKey(leftLocation))
            return randomPositionsOfCells[leftLocation];
        else
            return null;

    }

    public void ChangingPositions(GameObject slideObject)
    {
        SlideCell slideCell = slideObject.GetComponent<SlideCell>();

        if (slideCell.GetIsPositionTrue().Equals(true))
        {
            SlidePuzzle.shiftedCellCount++;
            slideCell.SetIsPositionTrue(false);
            if (SlidePuzzle.selectedCell.GetComponent<SlideCell>().GetIsPositionTrue().Equals(true))
            {
                SlidePuzzle.shiftedCellCount++;
                SlidePuzzle.selectedCell.GetComponent<SlideCell>().SetIsPositionTrue(false);
            }

        }
        else
        {
            if (SlidePuzzle.selectedCell.GetComponent<SlideCell>().GetIsPositionTrue().Equals(true))
            {
                SlidePuzzle.shiftedCellCount++;
                SlidePuzzle.selectedCell.GetComponent<SlideCell>().SetIsPositionTrue(false);
            }
            else
            {
                if (SlidePuzzle.selectedCell.GetComponent<SlideCell>().GetTruePosition().Equals(slideObject.GetComponent<RectTransform>().anchoredPosition))
                {
                    SlidePuzzle.shiftedCellCount--;
                    SlidePuzzle.selectedCell.GetComponent<SlideCell>().SetIsPositionTrue(true);
                    if (slideCell.GetTruePosition().Equals(SlidePuzzle.selectedCell.GetComponent<RectTransform>().anchoredPosition))
                    {
                        SlidePuzzle.shiftedCellCount--;
                        slideCell.SetIsPositionTrue(true);
                    }

                }
                else
                {
                    if (slideCell.GetTruePosition().Equals(SlidePuzzle.selectedCell.GetComponent<RectTransform>().anchoredPosition))
                    {
                        SlidePuzzle.shiftedCellCount--;
                        slideCell.SetIsPositionTrue(true);
                    }
                }
            }
        }
        Vector2 tempPosition = SlidePuzzle.selectedCell.GetComponent<RectTransform>().anchoredPosition;

        randomPositionsOfCells[slideObject.GetComponent<RectTransform>().anchoredPosition] = selectedCell;
        randomPositionsOfCells[tempPosition] = slideObject;

        SlidePuzzle.selectedCell.GetComponent<RectTransform>().anchoredPosition = slideObject.GetComponent<RectTransform>().anchoredPosition;
        slideObject.GetComponent<RectTransform>().anchoredPosition = tempPosition;

       
    }
}
