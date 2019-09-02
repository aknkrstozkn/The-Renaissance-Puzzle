using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFranz : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        MainMenuManager.pieceCount = int.Parse(gameObject.name);
        MainMenuManager.painting = MainMenuManager.paintings[2];
        MainMenuManager.isReady = true;

        Debug.Log(MainMenuManager.pieceCount);

    }
}
