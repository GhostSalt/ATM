using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WithdrawOtherMenu : Menu
{
    private Text RequestText;
    private Text PINText;

    private string UserInput = "";
    private string ExpectedInput = "12345";
    private string TopMessage = "Enter required amount\non the keypad:\n\n\n\n";

    private string FormatCurrency(string userInput)
    {
        if (UserInput.Length <= 2)
            return "£0." + "00".Substring(0, 2 - userInput.Length) + userInput;
        else
            return "£" + userInput.Substring(0, userInput.Length - 2) + "." + userInput.Substring(userInput.Length - 2, 2);
    }

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
        SetDisplayText();
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

    private void SetDisplayText()
    {
        PINText.text = "\n\n\n" + FormatCurrency(UserInput);
    }

    protected override bool RegisterNumKeyPress(int pos)
    {
        if (UserInput.Length < 5)
        {
            if (UserInput == "0")
                UserInput = pos.ToString();
            else
                UserInput += pos;
            SetDisplayText();
            return true;
        }
        return false;
    }

    protected override bool RegisterSideKeyPress(int pos)
    {
        if (pos == 0)
        {
            ChangeMenu(typeof(TransToWithdrawalMenuMenu));
            return true;
        }
        else if (pos == 1)
        {
            if (UserInput.Length > 0)
            {
                UserInput = UserInput.Substring(0, UserInput.Length - 1);
                SetDisplayText();
                return true;
            }
        }
        else
        {
            if (UserInput == ExpectedInput)
                ChangeMenu(typeof(DispenseCashMenu));
            return true;
        }
        return false;
    }

}