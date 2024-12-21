using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountBalanceMenu : DecisionMenu {

    private Text AccountBalance;
    private string Balance = "Balance:\n£88.88";

    public override void Construct(int bank, Image imageTemplate, Text textTemplate, Sprite[] allSprites, Font[] allFonts)
    {
        base.Construct(bank, imageTemplate, textTemplate, allSprites, allFonts);

        AccountBalance = CreateText();
        AccountBalance.text = Balance;
        AccountBalance.fontSize = 30;
        AccountBalance.transform.localScale = Vector3.zero;

        Assign("",
            new[] { "", "", "", "", "", "", "", "Back" },
            new[] { null, null, null, null, null, null, null, typeof(MainMenu) });
    }

    public override void Activate(bool isInitialMenu)
    {
        base.Activate(isInitialMenu);

        AccountBalance.transform.localScale = Vector3.one;
    }

    public override void Destroy()
    {
        base.Destroy();

        Destroy(AccountBalance.gameObject);
    }
}
