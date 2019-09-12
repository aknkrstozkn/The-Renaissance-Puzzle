using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelAncient : MonoBehaviour
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
        MainMenuManager.paintIndex = 0;

        MainMenuManager.isReady = true;

    }
    

   
}
