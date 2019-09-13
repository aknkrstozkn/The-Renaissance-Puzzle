using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SlidePuzzleController : MonoBehaviour
{
    //[SerializeField]
    //private AudioClip buttonClickClip;


    //These Values are to make puzzle
    //---------------------------
    public GameObject cellsParent;
    private SlidePuzzle slidePuzzle;
    public static Shader glowShader;
    readonly float multiplier = 0.5f;
    private float complexity;
    //Values Whoes come from Main Menu
    private bool isRotateEnabled;
    private int pieceCount;
    public Sprite painting;
    public Sprite selectedCellsSprite;
    public int paintingIndex;
    //---------------------------

    //Win Screen
    public TextMeshProUGUI winText;
    private bool firstTime;

    //To print Error count
    public TextMeshProUGUI countText;

    //To save first Cameras OrthographSize
    private float orthographSize;

    //Swap Values for delay and auto-swap
    float interval = 0.3f;
    float nextTime = 0;
    private bool isKeyUp;
    private KeyCode keyCode1;
    private KeyCode keyCode2;
    void Awake()
    {
        firstTime = true;


        winText.gameObject.SetActive(false);

        if (MainMenuManager.isLoadingSave.Equals(false))
            BuildPuzzle();
        else
            LoadPuzzle();

        

        painting = MainMenuManager.paintings[paintingIndex];
        orthographSize = ((painting.textureRect.height / 100) * multiplier);
        Camera.main.orthographicSize = orthographSize;

        

    }

    private void LoadPuzzle()
    {
        SlidePuzzleData data = SaveSystem.LoadSlidePuzzle();
        pieceCount = data.GetPieceCount();
        paintingIndex = data.GetPaintIndex();
        isRotateEnabled = data.GetIsRotateEnable();
        complexity = data.GetComplexityFactor();
        slidePuzzle = new SlidePuzzle(complexity, isRotateEnabled, paintingIndex, selectedCellsSprite, pieceCount, cellsParent, glowShader);
        slidePuzzle.LoadSlidePuzzle(data);
    }

    private void BuildPuzzle()
    {
        pieceCount = MainMenuManager.pieceCount;

        paintingIndex = MainMenuManager.paintIndex;

        isRotateEnabled = MainMenuManager.isRotateOn;
        complexity = MainMenuManager.complexityFactor;
        slidePuzzle = new SlidePuzzle(complexity, isRotateEnabled, paintingIndex, selectedCellsSprite, pieceCount, cellsParent, glowShader);
        slidePuzzle.BuildSlidePuzzle();
    }
    public void ShowPaint(Button buttonShowPaint)
    {
        int order = cellsParent.GetComponent<SpriteRenderer>().sortingOrder;
        if (order == 1)
        {
            cellsParent.GetComponent<SpriteRenderer>().sortingOrder = 0;
            buttonShowPaint.GetComponentInChildren<Text>().text = "Show Paint";
        }
        else
        {
            cellsParent.GetComponent<SpriteRenderer>().sortingOrder = 1;
            buttonShowPaint.GetComponentInChildren<Text>().text = "Hide Paint";
        }

    }


    public void Exit()
    {
        SaveSystem.SaveSlidePuzzle(slidePuzzle);

        MainMenuManager.isReady = false;
        SceneManager.LoadScene(0);
    }

    private void AutoSwap(KeyCode key)
    {
        if (key == KeyCode.A || key == KeyCode.LeftArrow)
        {
            if(slidePuzzle.GetSelectedCellsRight() != null)
                slidePuzzle.ChangingPositions(slidePuzzle.GetSelectedCellsRight());
        }

        if (key == KeyCode.RightArrow || key == KeyCode.D)
        {
            if (slidePuzzle.GetSelectedCellsLeft() != null)
                slidePuzzle.ChangingPositions(slidePuzzle.GetSelectedCellsLeft());
        }

        if (key == KeyCode.UpArrow || key == KeyCode.W)
        {
            if (slidePuzzle.GetSelectedCellsBackward() != null)
                slidePuzzle.ChangingPositions(slidePuzzle.GetSelectedCellsBackward());
        }

        if (key == KeyCode.DownArrow || key == KeyCode.S)
        {
            if (slidePuzzle.GetSelectedCellsForward() != null)
                slidePuzzle.ChangingPositions(slidePuzzle.GetSelectedCellsForward());
        }
    }
    
    public bool IsKeyUp()
    {
        if (Input.GetKeyUp(keyCode1) || Input.GetKeyUp(keyCode2))
        {
            keyCode1 = 0;
            keyCode2 = 0;
            return true;
        }
        return false;
    }
    public void Swap()
    {

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (slidePuzzle.GetSelectedCellsRight() != null)
            {
                slidePuzzle.ChangingPositions(slidePuzzle.GetSelectedCellsRight());
                keyCode1 = KeyCode.A;
                keyCode2 = KeyCode.LeftArrow;
                nextTime += (interval - (nextTime - Time.time));
            }
                
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (slidePuzzle.GetSelectedCellsLeft() != null)
            {
                slidePuzzle.ChangingPositions(slidePuzzle.GetSelectedCellsLeft());
                keyCode1 = KeyCode.D;
                keyCode2 = KeyCode.RightArrow;
                nextTime += (interval - (nextTime - Time.time));
            }
            
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (slidePuzzle.GetSelectedCellsBackward() != null)
            {
                slidePuzzle.ChangingPositions(slidePuzzle.GetSelectedCellsBackward());
                keyCode1 = KeyCode.W;
                keyCode2 = KeyCode.UpArrow;
                nextTime += (interval - (nextTime - Time.time));
            }
            
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (slidePuzzle.GetSelectedCellsForward() != null)
            {
                slidePuzzle.ChangingPositions(slidePuzzle.GetSelectedCellsForward());
                keyCode1 = KeyCode.S;
                keyCode2 = KeyCode.DownArrow;
                nextTime += (interval - (nextTime - Time.time));
            }
            
        }
    }


    private void SwapDelay()
    {
        isKeyUp = IsKeyUp();
        if (Time.time >= nextTime)
        {
            if (!isKeyUp)
                AutoSwap(keyCode1);
            nextTime += interval;
        }
    }
    private void ShowError()
    {
        if (SlidePuzzle.invertedCellCount > SlidePuzzle.shiftedCellCount)
            countText.text = SlidePuzzle.invertedCellCount.ToString();
        else
            countText.text = SlidePuzzle.shiftedCellCount.ToString();
    }

    private void SetWin()
    {
        if (IsWin() && firstTime)
        {
            firstTime = false;
            cellsParent.GetComponent<SpriteRenderer>().sortingOrder = 1;
            winText.gameObject.SetActive(true);
            Camera.main.orthographicSize = (int)(painting.textureRect.height / 100 * multiplier);
            Camera.main.transform.position = new Vector3(0, 0, Camera.main.transform.position.z);

        }
    }
    private void Update()
    {
        Swap();
        SwapDelay();

        ShowError();

        SetWin();
    }

    public bool IsWin()
    {
        bool SwapWin = (SlidePuzzle.shiftedCellCount == 0);
        bool RotateWin = (SlidePuzzle.invertedCellCount == 0);

        return (SwapWin && RotateWin);
    }
    public void CenterButtonClick()
    {
        Camera.main.orthographicSize = orthographSize;
        Camera.main.transform.position = new Vector3(0f, 0f, Camera.main.transform.position.z);
        GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider>().value = 0;

    }





    
}
