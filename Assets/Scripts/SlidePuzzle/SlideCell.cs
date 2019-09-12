using SpriteGlow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideCell : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isPositionTrue;
    private Vector2 truePosition;
    private int maxGlowWidth;
    


    public void SetMaxGlowWidth(int maxGlowWidth)
    {
        this.maxGlowWidth = maxGlowWidth;
    }

    public int GetMaxGlowWidth()
    {
        return maxGlowWidth;
    }
    
    public Vector2 GetTruePosition()
    {
        return truePosition;
    }
    public void SetTruePosition(Vector2 truePosition)
    {
        this.truePosition = truePosition;
    }
    public void SetIsPositionTrue(bool isPositionTrue)
    {
        this.isPositionTrue = isPositionTrue;
    }
    public bool GetIsPositionTrue()
    {
        return isPositionTrue;
    }
    
    public void SetPuzzlePieceGlow(bool outlinesEnabled)
    {
        int width = outlinesEnabled ? maxGlowWidth : 0;
        SpriteGlowEffect glowEffect = GetComponent<SpriteGlowEffect>();
        glowEffect.OutlineWidth = width;
        glowEffect.GlowBrightness = 1;
        glowEffect.GlowColor = Color.red;
    }
    
    
}
