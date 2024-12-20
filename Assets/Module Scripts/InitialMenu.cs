using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InitialMenu : Menu
{
    private Image Logo;
    private Text Text;
    private Image Advert;

    private List<Sprite> PossibleAds = new List<Sprite>();

    private Coroutine AdCycleCoroutine;
    private List<Sprite> ChosenAds;
    private bool IsPlayingAds = false;

    public override void Construct(Image imageTemplate, Text textTemplate, Sprite[] allSprites)
    {
        base.Construct(imageTemplate, textTemplate, allSprites);

        Logo = Instantiate(imageTemplate, imageTemplate.transform.parent);
        Logo.transform.localScale = Vector3.zero;

        Text = Instantiate(textTemplate, textTemplate.transform.parent);
        Text.text = "\n\n\n\n\n\n\nPlease insert your card";
        Text.fontSize = 20;
        Text.transform.localScale = Vector3.zero;

        Advert = Instantiate(imageTemplate, imageTemplate.transform.parent);
        Advert.transform.localScale = Vector3.zero;

        int i = 0;
        Sprite tempSprite = FindSprite("ad 0");
        while (tempSprite != null && i < 1000) // Wanna make sure this isn't an infinite loop.
        {
            i++;
            PossibleAds.Add(tempSprite);
            tempSprite = FindSprite("ad " + i);
        }

        ChosenAds = PossibleAds.ToList().Shuffle().Take(3).ToList();
    }

    public override void Activate(bool isInitialMenu)
    {
        Logo.transform.localScale = Vector3.one;
        Text.transform.localScale = Vector3.one;
        
        if (AdCycleCoroutine != null)
            StopCoroutine(AdCycleCoroutine);
        AdCycleCoroutine = StartCoroutine(AdCycle(isInitialMenu));
        IsPlayingAds = true;
    }

    public override void Destroy()
    {
        if (AdCycleCoroutine != null)
            StopCoroutine(AdCycleCoroutine);
        IsPlayingAds = false;

        Destroy(Logo.gameObject);
        Destroy(Text.gameObject);
        Destroy(Advert.gameObject);
    }

    protected override void RegisterModuleFocus()
    {
        if (IsPlayingAds)
        {
            if (AdCycleCoroutine != null)
                StopCoroutine(AdCycleCoroutine);
            IsPlayingAds = false;
            Advert.transform.localScale = Vector3.zero;
        }
    }

    protected override void RegisterModuleDefocus()
    {
        if (!IsPlayingAds)
        {
            if (AdCycleCoroutine != null)
                StopCoroutine(AdCycleCoroutine);
            AdCycleCoroutine = StartCoroutine(AdCycle());
            IsPlayingAds = true;
        }
    }

    protected override void RegisterCardInsert()
    {
        ChangeMenu(typeof(PINMenu));
    }

    private IEnumerator AdCycle(bool happenImmediately = false, float interval = 10f)
    {
        float timer = 0;
        if (!happenImmediately)
        {
            while (timer < interval)
            {
                yield return null;
                timer += Time.deltaTime;
            }
        }
        Advert.transform.localScale = Vector3.one;
        while (true)
            foreach (var ad in ChosenAds)
            {
                Advert.sprite = ad;
                timer = 0;
                while (timer < interval)
                {
                    yield return null;
                    timer += Time.deltaTime;
                }
            }
    }

}
