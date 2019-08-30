using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlowEffect : MonoBehaviour
{
    public float width;
    public float duration;
    public float m_Delay;
    //delay is the time between each shine;
    public Material mat = null;
    void Start()
    {

        // either you have to have this exact address in your resource folder or change it to where ever 
        // you saved your shiny material, remember that the "Shiny" last bit here is just an example I used for
        // the material name, you have to use the name which you have given your material with shiny shader on it.
        // Take note that you have to add a "Resourse" folder in your assets folder in order for the script to work.
        

        Renderer renderer = gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = mat;
        }
        else
        {
            Image img = gameObject.GetComponent<Image>();
            if (img != null)
            {
                img.material = mat;
            }
            else
            {
                Debug.LogWarning("Cannot get the Renderer or Image component!");
            }
        }

        mat.SetFloat("_ShineWith", width);
        StopAllCoroutines();
        StartCoroutine(shineRoutine(mat, duration, m_Delay));
    }

    static IEnumerator shineRoutine(Material mat, float duration, float m_Delay)
    {
        if (mat != null)
        {
            float location = 0f;
            float interval = 0.04f;
            float offsetVal = interval / duration;
            while (true)
            {
                yield return new WaitForSeconds(interval);
                mat.SetFloat("_ShineLocation", location);
                location += offsetVal;
                if (location > 1f)
                {
                    yield return new WaitForSeconds(m_Delay);
                    location = 0f;
                }
            }

        }
        else
        {
            Debug.LogWarning("There is no Material parameter!");
        }
    }
}