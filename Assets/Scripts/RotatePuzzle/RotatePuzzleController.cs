using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RotatePuzzleController : MonoBehaviour
{
    [SerializeField]
    private AudioClip buttonClickClip;
    [SerializeField]
    private Slider slider;

    public GameObject cellsParent;
    private int pieceCount;
    private RotatePuzzle rotatePuzzle;
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
        orthographSize = ((painting.textureRect.height / 100) * multiplier);
        Camera.main.orthographicSize = orthographSize;
        winText.gameObject.SetActive(false);
        rotatePuzzle = new RotatePuzzle(painting, pieceCount, cellsParent, glowMaterial, glowShader);
        rotatePuzzle.BuildRotatePuzzle();
        ArrangeWinPaint();



    }
    void ArrangeWinPaint()
    {
        //winPaint.gameObject.SetActive(false);
        //winPaint.GetComponent<RectTransform>().sizeDelta =
        //    new Vector2(painting.texture.width, painting.texture.height);
        //winPaint.sprite = painting;
    }
    private void Update()
    {
        countText.text = RotatePuzzle.falseCellCount.ToString();
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
    public void Exit()
    {
        MainMenuManager.isReady = false;
        SceneManager.LoadScene(0);
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
