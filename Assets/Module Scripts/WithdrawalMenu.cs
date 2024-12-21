using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WithdrawalMenu : DecisionMenu {

    private int CorrectAnswer = 3;

    public override void Construct(int bank, Image imageTemplate, Text textTemplate, Sprite[] allSprites, Font[] allFonts)
    {
        base.Construct(bank, imageTemplate, textTemplate, allSprites, allFonts);

        Assign("Select amount",
            new[] { "£10", "£20", "£30", "£50", "£100", "£200", "Other", "Back" },
            new[] { typeof(DispenseCashMenu), typeof(DispenseCashMenu), typeof(DispenseCashMenu), typeof(DispenseCashMenu),
                    typeof(DispenseCashMenu), typeof(DispenseCashMenu), typeof(WithdrawOtherMenu), typeof(TransToMainMenuMenu) },
            Enumerable.Range(0, 8).Where(x => x != CorrectAnswer).ToList());
    }
}
