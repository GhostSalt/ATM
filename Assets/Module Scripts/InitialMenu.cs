using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitialMenu : Menu
{
	private Image Logo;
	private Text Text;

	public InitialMenu(Image imageTemplate, Text textTemplate) : base(imageTemplate, textTemplate)
    {
		Logo = Instantiate(imageTemplate, imageTemplate.transform.parent);
        Logo.transform.localScale = Vector3.zero;

        Text = Instantiate(textTemplate, textTemplate.transform.parent);
        Text.text = "\n\n\n\n\n\n\nPlease insert your card";
        Text.fontSize = 20;
        Text.transform.localScale = Vector3.zero;
    }

    public override void Activate()
    {
        Logo.transform.localScale = Vector3.one;
        Text.transform.localScale = Vector3.one;
    }

    public override void Destroy()
    {
        Destroy(Logo.gameObject);
        Destroy(Text.gameObject);
    }

    protected override void RegisterCardInsert()
    {
        ChangeMenu(typeof(PINMenu));
    }
}
