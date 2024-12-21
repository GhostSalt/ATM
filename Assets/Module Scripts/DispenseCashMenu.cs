using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Rnd = UnityEngine.Random;

public class DispenseCashMenu : TransMenu
{
    public override void Activate(bool isInitialMenu)
    {
        base.Activate(isInitialMenu);
        SetText("DISPENSING…");
        StartCoroutine(RunAnimation());
    }

    private IEnumerator RunAnimation()
    {
        var duration1 = 0.5f;
        var duration2 = Rnd.Range(4.5f, 5f);
        var duration3 = Rnd.Range(0.45f, 0.75f);

        float timer = 0;
        while (timer < duration1)
        {
            yield return null;
            timer += Time.deltaTime;
        }

        RemoveCard();
        DispenseCash();

        timer = 0;
        while (timer < duration2)
        {
            yield return null;
            timer += Time.deltaTime;
        }

        SetText("DISCONNECTING\nFROM SERVER…");

        timer = 0;
        while (timer < duration3)
        {
            yield return null;
            timer += Time.deltaTime;
        }

        ChangeMenu(typeof(InitialMenu));
    }
}
