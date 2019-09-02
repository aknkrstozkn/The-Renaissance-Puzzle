using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCameraController : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 mousePos;
    readonly int cameraSpeed = 1;
    float zoomRatio;
    float orthographicSize;
    float screenHeightInUnits;
    float screenWidthInUnits;
    Bounds cameraBounds;


    Vector2 mouseClickPos;
    Vector2 mouseCurrentPos;
    bool panning = false;
    /*
    void CameraMovement()
    {
        mousePos = Input.mousePosition; //We need to get the new position every frame
        zoomRatio = Camera.main.orthographicSize / orthographicSize;
        //if mouse is 50 pixels and less from the left side of the
        //screen, we move the camera in that direction at scrollSpeed
        if (mousePos.x < 50)
            if(!(transform.position.x < cameraBounds.min.x))
                 transform.Translate(-(cameraSpeed * zoomRatio), 0, 0);

        //if 50px or less from the right side, move right at scrollSpeeed
        if (mousePos.x > Screen.width - 50)
            if (!(transform.position.x > cameraBounds.max.x))
                transform.Translate((cameraSpeed * zoomRatio), 0, 0);

        //move up
        if (mousePos.y > 50)
            if (!(transform.position.y > cameraBounds.max.y))
                transform.Translate(0, (cameraSpeed * zoomRatio), 0);

        //move down
        if (mousePos.y < Screen.height - 50)
            if (!(transform.position.y < cameraBounds.min.y))
                transform.Translate(0, -(cameraSpeed * zoomRatio), 0);

    }
    */
    
    private void Start()
    {
        
        orthographicSize = Camera.main.orthographicSize;     
        transform.localPosition = new Vector3(0,0,-90);
        cameraBounds = OrthographicBounds();
        
    }
    // Update is called once per frame
    void Update()
    {        
        CameraMovement();
    }

    public Bounds OrthographicBounds()
    {
        screenHeightInUnits = Camera.main.orthographicSize * 2;
        screenWidthInUnits = screenHeightInUnits * Screen.width / (Screen.height) ;
        return new Bounds(
            Camera.main.transform.position,
            new Vector3(screenWidthInUnits, screenHeightInUnits, 0));
        
    }

    

    private void CameraMovement()
    {
        // When LMB clicked get mouse click position and set panning to true
        if (Input.GetKeyDown(KeyCode.Mouse2) && !panning)
        {
            mouseClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            panning = true;
        }
        // If LMB is already clicked, move the camera following the mouse position update
        if (panning)
        {   
            mouseCurrentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var distance = mouseCurrentPos - mouseClickPos;
            Vector3 position = transform.position - new Vector3(distance.x,distance.y,0);
            if ((cameraBounds.max.x < position.x || cameraBounds.max.y < position.y)
                || (cameraBounds.min.x > position.x || cameraBounds.min.y > position.y))
            {
                panning = false;
            }
            else
                transform.position += new Vector3(-distance.x, -distance.y, 0);

        }

        // If LMB is released, stop moving the camera
        if (Input.GetKeyUp(KeyCode.Mouse2))
            panning = false;
    }
}
