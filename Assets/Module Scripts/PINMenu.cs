using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PINMenu : Menu
{
    private Text RequestText;

    private string UserInput = "";

    public PINMenu(Image imageTemplate, Text textTemplate) : base(imageTemplate, textTemplate)
    {
        RequestText = Instantiate(textTemplate, textTemplate.transform.parent);
        RequestText.fontSize = 20;
        RequestText.transform.localScale = Vector3.zero;

        DisplayText();
    }

    public override void Activate()
    {
        RequestText.transform.localScale = Vector3.one;
    }

    public override void Destroy()
    {
        Destroy(RequestText.gameObject);
    }

    private void DisplayText()
    {
        var stars = "";
        for (int i = 0; i < UserInput.Length; i++)
            stars += "*";
        RequestText.text = "\n\nPlease enter your <color=#ffff00>PIN number</color>\nand press <color=#ffff00>ENTER</color>.\n\n\n\n\n\n" + stars;
    }

    protected override void RegisterNumKeyPress(int pos)
    {
        UserInput += pos;
        Debug.Log(UserInput);
        DisplayText();
    }

}