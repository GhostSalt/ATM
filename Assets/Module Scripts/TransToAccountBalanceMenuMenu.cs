using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Rnd = UnityEngine.Random;

public class TransToAccountBalanceMenuMenu : TransMenu
{
    public override void Activate(bool isInitialMenu)
    {
        base.Activate(isInitialMenu);
        StartCoroutine(RunAnimation());
    }

    private IEnumerator RunAnimation()
    {
        var duration = Rnd.Range(0.25f, 0.35f);

        float timer = 0;
        while (timer < duration)
        {
            yield return null;
            timer += Time.deltaTime;
        }

        ChangeMenu(typeof(AccountBalanceMenu));
    }
}
