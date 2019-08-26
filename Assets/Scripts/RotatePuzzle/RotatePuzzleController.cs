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
    readonly float multiplier = 0.25f;
    public Sprite painting;    

    public TextMeshProUGUI winText;
    
    void Awake()
    {
        pieceCount = 35;        
        Camera.main.orthographicSize = (int)(painting.textureRect.height * multiplier);
        Debug.Log(multiplier);
        winText.gameObject.SetActive(false);
        rotatePuzzle = new RotatePuzzle(painting, pieceCount, cellPrefab, cellsParent);
        rotatePuzzle.BuildRotatePuzzle();

        
    }
    
    private void Update()
    {
        if (IsWin())
        {
            winText.gameObject.SetActive(true);
            Camera.main.orthographicSize = (int)(painting.textureRect.height * multiplier);
            Camera.main.transform.localPosition = new Vector3(0, 0, -90);
        }
           
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
