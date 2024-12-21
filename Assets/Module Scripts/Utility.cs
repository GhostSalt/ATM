using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour {

    public static Sprite FindSprite(string name, Sprite[] allSprites)
    {
        foreach (var sprite in allSprites)
            if (sprite.name == name)
                return sprite;
        return null;
    }

    public static Font FindFont(int ix, Font[] allFonts)
    {
        if (ix > allFonts.Length - 1)
            return null;
        return allFonts[ix];
    }
}
