using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public abstract class Menu : MonoBehaviour {

	public void RegisterInput(string input)
	{
		if (Regex.IsMatch(input, "^arrow [0-7]$"))
		{
			int value = int.Parse(Regex.Matches(input, "^arrow ([0-7])$")[1].Value);
			RegisterArrowPress(value);
        }
		else if (Regex.IsMatch(input, "^keypad num [0-9]$"))
		{
            int value = int.Parse(Regex.Matches(input, "^keypad num ([0-9])$")[1].Value);
            RegisterNumKeyPress(value);
        }
		else if (Regex.IsMatch(input, "^keypad side [0-2]$"))
		{
            int value = int.Parse(Regex.Matches(input, "^keypad side ([0-2])$")[1].Value);
            RegisterSideKeyPress(value);
        }
		else if (Regex.IsMatch(input, "^card insert$"))
            RegisterCardInsert();
	}

	protected virtual void RegisterArrowPress(int pos) { }

	protected virtual void RegisterNumKeyPress(int pos) { }

	protected virtual void RegisterSideKeyPress(int pos) { }

	protected virtual void RegisterCardInsert() { }

	public virtual void Activate() { }

    public virtual void Deactivate() { }
}
