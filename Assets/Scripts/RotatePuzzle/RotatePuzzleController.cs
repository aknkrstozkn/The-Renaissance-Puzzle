using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RotatePuzzleController : MonoBehaviour
{
    public AudioClip buttonClickClip;    

    public Image cellPrefab;
    public Image cellsParent;
    private int pieceCount;
    private RotatePuzzle rotatePuzzle;

    public Sprite painting;    

    public TextMeshProUGUI winText;
    
    void Awake()
    {
        pieceCount = 72;
        float multiplier = 0.25f;
        Camera.main.orthographicSize = (int)(painting.textureRect.height * multiplier);
        Debug.Log(multiplier);
        winText.gameObject.SetActive(false);
        rotatePuzzle = new RotatePuzzle(painting, pieceCount, cellPrefab, cellsParent);
        rotatePuzzle.BuildRotatePuzzle();

        
    }
    
    private void Update()
    {
        if (IsWin())
            winText.gameObject.SetActive(true);
    }

    public bool IsWin()
    {
        if (RotatePuzzle.falseCellCount == 0)
            return true;

        return false;
    }
    public void ButtonClick()
    {
        AudioManager.instance.PlayOnce(buttonClickClip);
    }
} 
