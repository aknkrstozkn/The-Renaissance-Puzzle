using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 mousePos;
    readonly int cameraSpeed = 20;
    float zoomRatio;
    float orthographicSize;
    float screenHeightInUnits;
    float screenWidthInUnits;
    Bounds cameraBounds;

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

    
    private void Awake()
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
        screenWidthInUnits = screenHeightInUnits * Screen.width / Screen.height;
        return new Bounds(
            Camera.main.transform.position,
            new Vector3(screenWidthInUnits, screenHeightInUnits, 0));
        
    }
}
