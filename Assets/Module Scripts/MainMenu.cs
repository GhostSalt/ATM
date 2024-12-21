using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : DecisionMenu {

    public override void Construct(int bank, Image imageTemplate, Text textTemplate, Sprite[] allSprites, Font[] allFonts)
    {
        base.Construct(bank, imageTemplate, textTemplate, allSprites, allFonts);

        Assign("Select an option",
            new[] { "Cash Withdrawal", "Check Balance", "", "", "", "", "", "Return Card" },
            new[] { typeof(WithdrawalMenu), typeof(TransToAccountBalanceMenuMenu), null, null, null, null, null, typeof(TransToInitialMenuMenu) });
    }
}
