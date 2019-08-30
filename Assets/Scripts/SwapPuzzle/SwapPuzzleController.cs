using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwapPuzzleController : MonoBehaviour
{
    [SerializeField]
    private AudioClip buttonClickClip;
    [SerializeField]
    private Slider slider;

    private bool isRotateEnabled;

    public GameObject cellsParent;
    private int pieceCount;
    private SwapPuzzle rotatePuzzle;
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
        pieceCount = 35;
        isRotateEnabled = false;

        orthographSize = ((painting.textureRect.height / 100) * multiplier);
        Camera.main.orthographicSize = orthographSize;
        winText.gameObject.SetActive(false);
        rotatePuzzle = new SwapPuzzle(isRotateEnabled, painting, pieceCount, cellsParent, glowMaterial, glowShader);
        rotatePuzzle.BuildRotatePuzzle();

    }
    
    private void Update()
    {
        countText.text = SwapPuzzle.invertedCellCount.ToString();
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
        if (RotatePuzzle.falseCellCount == 0)
            return true;

        return false;
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
