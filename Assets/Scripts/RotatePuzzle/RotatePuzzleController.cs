using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RotatePuzzleController : MonoBehaviour
{
    //[SerializeField]
    //private AudioClip buttonClickClip;

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
        GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider>().value = 0;

    }
    /*
    public void ButtonClick()
    {
        AudioManager.instance.PlayOnce(buttonClickClip);
    }*/
} 
