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
    public Image winPaint;
    private int pieceCount;
    private RotatePuzzle rotatePuzzle;
    readonly float multiplier = 0.25f;
    public Sprite painting;
    bool firstTime;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI countText;

    void Awake()
    {
        firstTime = true;
        pieceCount = 72;        
        Camera.main.orthographicSize = (int)(painting.textureRect.height * multiplier);
        Debug.Log(multiplier);
        winText.gameObject.SetActive(false);
        rotatePuzzle = new RotatePuzzle(painting, pieceCount, cellPrefab, cellsParent);
        rotatePuzzle.BuildRotatePuzzle();
        ArrangeWinPaint();



    }
    void ArrangeWinPaint()
    {
        winPaint.gameObject.SetActive(false);
        winPaint.GetComponent<RectTransform>().sizeDelta =
            new Vector2(painting.texture.width, painting.texture.height);
        winPaint.sprite = painting;
    }
    private void Update()
    {
        countText.text = RotatePuzzle.falseCellCount.ToString();
        if (IsWin() && firstTime)
        {
            firstTime = false;
            winPaint.gameObject.SetActive(true);
            winText.gameObject.SetActive(true);
            cellsParent.gameObject.SetActive(false);
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
