using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SwapPuzzleController : MonoBehaviour
{
    //[SerializeField]
    //private AudioClip buttonClickClip;

    
    //These Values are to make puzzle
    //---------------------------
    public GameObject cellsParent;
    private SwapPuzzle swapPuzzle;
    public static Shader glowShader;
    readonly float multiplier = 0.5f;
    private float complexity;
    //Values Whoes come from Main Menu
    private bool isRotateEnabled;
    private int pieceCount;
    public Sprite painting;
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
    private KeyCode keyCode;
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
        SwapPuzzleData data = SaveSystem.LoadSwapPuzzle();
        pieceCount = data.GetPieceCount();
        paintingIndex = data.GetPaintIndex();
        isRotateEnabled = data.GetIsRotateEnable();
        complexity = data.GetComplexityFactor();
        swapPuzzle = new SwapPuzzle(complexity, isRotateEnabled, paintingIndex, pieceCount, cellsParent, glowShader);
        swapPuzzle.LoadSwapPuzzle(data);
    }

    private void BuildPuzzle()
    {
        pieceCount = MainMenuManager.pieceCount;
        
        paintingIndex = MainMenuManager.paintIndex;        

        isRotateEnabled = MainMenuManager.isRotateOn;
        complexity = 1.0f / 0.3f;
        swapPuzzle = new SwapPuzzle(complexity, isRotateEnabled, paintingIndex, pieceCount, cellsParent, glowShader);
        swapPuzzle.BuildSwapPuzzle();
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
        SaveSystem.SaveSwapPuzzle(swapPuzzle);

        MainMenuManager.isReady = false;
        SceneManager.LoadScene(0);
    }

    private void AutoSwap(KeyCode key)
    {
        if (key == KeyCode.A || key == KeyCode.LeftArrow)
        {
            SwapPuzzle.selectedCell.GetComponent<SwapCell>().SwapLeft();
        }

        if (key == KeyCode.RightArrow || key == KeyCode.D)
        {
            SwapPuzzle.selectedCell.GetComponent<SwapCell>().SwapRight();
        }

        if (key == KeyCode.UpArrow || key == KeyCode.W)
        {
            SwapPuzzle.selectedCell.GetComponent<SwapCell>().SwapForward();
        }

        if (key == KeyCode.DownArrow || key == KeyCode.S)
        {
            SwapPuzzle.selectedCell.GetComponent<SwapCell>().SwapBackward();
        }
    }
    public bool IsKeyUp()
    {   if (Input.GetKeyUp(keyCode))
        {
            keyCode = 0;
            return true;
        }
        return false;
    }
    public void Swap()
    {
        
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SwapPuzzle.selectedCell.GetComponent<SwapCell>().SwapLeft();
            keyCode = KeyCode.A;
            nextTime += (interval - (nextTime - Time.time));
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            SwapPuzzle.selectedCell.GetComponent<SwapCell>().SwapRight();
            keyCode = KeyCode.D;
            nextTime += (interval - (nextTime - Time.time));
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            SwapPuzzle.selectedCell.GetComponent<SwapCell>().SwapForward();
            keyCode = KeyCode.W;
            nextTime += (interval - (nextTime - Time.time));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            SwapPuzzle.selectedCell.GetComponent<SwapCell>().SwapBackward();
            keyCode = KeyCode.S;
            nextTime += (interval - (nextTime - Time.time));
        }
    }
    

    private void SwapDelay()
    {
        isKeyUp = IsKeyUp();
        if (Time.time >= nextTime)
        {
            if (!isKeyUp)
                AutoSwap(keyCode);
            nextTime += interval;
        }
    }
    private void ShowError()
    {
        if (SwapPuzzle.invertedCellCount > SwapPuzzle.shiftedCellCount)
            countText.text = SwapPuzzle.invertedCellCount.ToString();
        else
            countText.text = SwapPuzzle.shiftedCellCount.ToString();
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
        bool SwapWin = (SwapPuzzle.shiftedCellCount == 0);
        bool RotateWin = (SwapPuzzle.invertedCellCount == 0);        

        return (SwapWin && RotateWin);
    }
    public void CenterButtonClick()
    {
        Camera.main.orthographicSize = orthographSize;
        Camera.main.transform.position = new Vector3(0f, 0f, Camera.main.transform.position.z);
        GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider>().value = 0;

    }
    /**
    public void ButtonClick()
    {
        AudioManager.instance.PlayOnce(buttonClickClip);
    }
    **/
} 
