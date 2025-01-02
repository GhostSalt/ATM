using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PINMenu : Menu
{
    private Text RequestText;
    private Text PINText;

    private string UserInput = "";
    private string ActualPIN = "1234";
    private string TopMessage = "Please enter your PIN\nand press ENTER.\n\n\n";

    public override void Construct(int bank, Image imageTemplate, Text textTemplate, Sprite[] allSprites, Font[] allFonts)
    {
        base.Construct(bank, imageTemplate, textTemplate, allSprites, allFonts);

        RequestText = CreateText();
        RequestText.fontSize = 20;
        RequestText.transform.localScale = Vector3.zero;
        RequestText.text = TopMessage;

        PINText = CreateText();
        PINText.fontSize = 35;
        PINText.transform.localScale = Vector3.zero;
        DisplayStars();
    }

    public override void Activate(bool isInitialMenu)
    {
        RequestText.transform.localScale = Vector3.one;
        PINText.transform.localScale = Vector3.one;
    }

    public override void Destroy()
    {
        Destroy(RequestText.gameObject);
        Destroy(PINText.gameObject);
    }

    private void DisplayStars()
    {
        var stars = "";
        for (int i = 0; i < UserInput.Length; i++)
            stars += "●";
        PINText.text = "\n\n\n" + stars;
    }

    protected override bool RegisterNumKeyPress(int pos)
    {
        if (UserInput.Length < 4)
        {
            UserInput += pos;
            DisplayStars();
            return true;
        }
        return false;
    }

    protected override bool RegisterSideKeyPress(int pos)
    {
        if (pos == 0)
        {
            ChangeMenu(typeof(TransToInitialMenuMenu));
            return true;
        }
        else if (pos == 1)
        {
            if (UserInput.Length > 0)
            {
                UserInput = UserInput.Substring(0, UserInput.Length - 1);
                DisplayStars();
                return true;
            }
        }
        else
        {
            if (UserInput == ActualPIN)
                ChangeMenu(typeof(TransToMainMenuMenu));
            else
                RequestText.text = "PIN incorrect.\n\n" + TopMessage + "\n\n";
            return true;
        }
        return false;
    }

}