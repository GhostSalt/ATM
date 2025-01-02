using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Rnd = UnityEngine.Random;

public class TransToWithdrawalMenuMenu : TransMenu
{
    public override void Activate(bool isInitialMenu)
    {
        base.Activate(isInitialMenu);
        StartCoroutine(RunAnimation());
    }

    private IEnumerator RunAnimation()
    {
        var duration = Rnd.Range(1.5f, 1.75f);

        float timer = 0;
        while (timer < duration)
        {
            yield return null;
            timer += Time.deltaTime;
        }

        ChangeMenu(typeof(WithdrawalMenu));
    }
}
