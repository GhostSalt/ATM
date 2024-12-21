using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class TransMenu : Menu
{
    private Text Text;

    public override void Construct(int bank, Image imageTemplate, Text textTemplate, Sprite[] allSprites, Font[] allFonts)
    {
        base.Construct(bank, imageTemplate, textTemplate, allSprites, allFonts);

        Text = CreateText();
        Text.text = "PLEASE WAIT";
        Text.fontSize = 30;
        Text.transform.localScale = Vector3.zero;
    }

    public override void Activate(bool isInitialMenu)
    {
        Text.transform.localScale = Vector3.one;
    }

    public override void Destroy()
    {
        Destroy(Text.gameObject);
    }

    protected void SetText(string text)
    {
        Text.text = text;
    }
}
