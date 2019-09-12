using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideSliderController : MonoBehaviour
{
    // Start is called before the first frame update
    readonly float wheelNumber = 10f;
    float orthographSize;
    Camera swapCamera;
    void Start()
    {
        swapCamera = Camera.main;

        orthographSize = swapCamera.orthographicSize * 100;
        gameObject.GetComponent<Slider>().value = 0;
        gameObject.GetComponent<Slider>().maxValue = orthographSize - 1;
        gameObject.GetComponent<Slider>().minValue = 0;

    }

    public void OnValueChange()
    {
        swapCamera.orthographicSize =
            (orthographSize - gameObject.GetComponent<Slider>().value) / 100;

    }

    // Update is called once per frame
    void OnValueChangeWheel()
    {
        var d = Input.GetAxis("Mouse ScrollWheel");
        if (d > 0f)
        {

            gameObject.GetComponent<Slider>().value += wheelNumber;
        }
        else if (d < 0f)
        {
            gameObject.GetComponent<Slider>().value -= wheelNumber;
        }
    }
    void Update()
    {
        OnValueChangeWheel();

    }
}
