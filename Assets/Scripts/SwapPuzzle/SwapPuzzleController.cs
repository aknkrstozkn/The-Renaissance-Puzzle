using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SwapPuzzleController : MonoBehaviour
{
    [SerializeField]
    private AudioClip buttonClickClip;
    [SerializeField]
    private Slider slider;

    private bool isRotateEnabled;

    public GameObject cellsParent;
    private int pieceCount;
    private SwapPuzzle swapPuzzle;
    readonly float multiplier = 0.5f;
    public Sprite painting;
    bool firstTime;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI countText;
    public static Material glowMaterial;
    public static Shader glowShader;
    float orthographSize;
    void Awake()
    {
        firstTime = true;
        
        pieceCount = MainMenuManager.pieceCount;
        painting = MainMenuManager.painting;
        isRotateEnabled = MainMenuManager.isRotateOn;
        
        /*
        pieceCount = 35;
        isRotateEnabled = true;
        */

        orthographSize = ((painting.textureRect.height / 100) * multiplier);
        
        Camera.main.orthographicSize = orthographSize;
        winText.gameObject.SetActive(false);
        swapPuzzle = new SwapPuzzle(isRotateEnabled, painting, pieceCount, cellsParent, glowMaterial, glowShader);
        swapPuzzle.BuildSwapPuzzle();

    }
    public void Exit()
    {
        MainMenuManager.isReady = false;
        SceneManager.LoadScene(0);
    }
    public void Swap()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            SwapPuzzle.selectedCell.GetComponent<SwapCell>().SwapLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            SwapPuzzle.selectedCell.GetComponent<SwapCell>().SwapRight();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            SwapPuzzle.selectedCell.GetComponent<SwapCell>().SwapForward();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            SwapPuzzle.selectedCell.GetComponent<SwapCell>().SwapBackward();
        }
    }
    private void Update()
    {
        Swap();
        
        if (SwapPuzzle.invertedCellCount > SwapPuzzle.shiftedCellCount)
            countText.text = SwapPuzzle.invertedCellCount.ToString();
        else
            countText.text = SwapPuzzle.shiftedCellCount.ToString();
            

        if (IsWin() && firstTime)
        {
            firstTime = false;
            cellsParent.GetComponent<SpriteRenderer>().sortingOrder = 1;
            winText.gameObject.SetActive(true);
            Camera.main.orthographicSize = (int)(painting.textureRect.height / 100 * multiplier);
            Camera.main.transform.position = new Vector3(0, 0, Camera.main.transform.position.z);

        }
           
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
        slider.value = 0;

    }
    public void ButtonClick()
    {
        AudioManager.instance.PlayOnce(buttonClickClip);
    }
} 
