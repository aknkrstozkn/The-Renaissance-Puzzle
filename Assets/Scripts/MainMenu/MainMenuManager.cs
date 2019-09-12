using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Toggle swapToggle;
    public Toggle rotateToggle;
    public Toggle slideToggle;

    public Slider slider;
    public static float complexityFactor;

    public Sprite[] sprites;

    public TextMeshProUGUI loadingText;

    public static int paintIndex;

    public static Sprite painting;
    public static Sprite[] paintings;
    public static int pieceCount;
    public static bool isRotateOn;

    public static bool isLoadingSave;

    public static bool isReady = false;
    void Awake()
    {
        isLoadingSave = false;
        loadingText.gameObject.SetActive(false);

        paintings = new Sprite[sprites.Length];
        painting = sprites[0];

        for (int i = 0; i < sprites.Length; i++)
        {
            paintings.SetValue(sprites[i], i); 
        }
    }

    public void LoadSaveOfSwap()
    {
        if (!File.Exists(SaveSystem.swapPuzzleSavePath))
            return;
        isLoadingSave = true;
        loadingText.gameObject.SetActive(true);
        SceneManager.LoadScene(2);
    }

    public void LoadSaveOfRotate()
    {
        if (!File.Exists(SaveSystem.rotatePuzzleSavePath))
            return;
        isLoadingSave = true;
        loadingText.gameObject.SetActive(true);
        SceneManager.LoadScene(1);
    }

    public void LoadSaveOfSlide()
    {
        if (!File.Exists(SaveSystem.slidePuzzleSavePath))
            return;
        isLoadingSave = true;
        loadingText.gameObject.SetActive(true);
        SceneManager.LoadScene(3);
    }

    public void LoadScene()

    {
        if ((swapToggle.isOn == false && rotateToggle.isOn == false && slideToggle.isOn == false) 
            || (swapToggle.isOn == true && rotateToggle.isOn == true && slideToggle.isOn == true))
        {
            isReady = false;
            return;
        }
        painting = sprites[paintIndex];
        complexityFactor = 10f / (float)slider.value; 

        if (swapToggle.isOn == true && rotateToggle.isOn == false)
        {
            isRotateOn = false;
            loadingText.gameObject.SetActive(true);
            SceneManager.LoadScene(2);
        }
        else if (swapToggle.isOn == true && rotateToggle.isOn == true)
        {
            loadingText.gameObject.SetActive(true);
            isRotateOn = true;
            SceneManager.LoadScene(2);
        }
        else if (slideToggle.isOn == true && rotateToggle.isOn == false)
        {
            isRotateOn = false;
            loadingText.gameObject.SetActive(true);
            SceneManager.LoadScene(3);
        }
        else if (slideToggle.isOn == true && rotateToggle.isOn == true)
        {
            isRotateOn = true;
            loadingText.gameObject.SetActive(true);
            SceneManager.LoadScene(3);
        }
        else
        {
            loadingText.gameObject.SetActive(true);
            SceneManager.LoadScene(1);
        }

    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
        #else
         Application.Quit();
        #endif
    }
    // Update is called once per frame
    void Update()
    {
        if (isReady)
            LoadScene();
    }
}
