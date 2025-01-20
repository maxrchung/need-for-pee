using UnityEngine;

public struct PissText
{
    public bool active;
    public string text;
    public Vector2 position;    
    public float scale; //scale is a float in which 1 = ~250 pixels
    public int[] glyphs;
    public int currentGlyph;
    public bool vn;
    public float vnTimer;
    public bool completed;
    public float textSpeed;
    public bool skippable;
}
