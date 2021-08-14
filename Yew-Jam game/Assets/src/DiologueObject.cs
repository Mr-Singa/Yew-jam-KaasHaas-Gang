using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiologueObject
{
	bool isShowCommand = false;
	bool shouldshow;
	bool isDesicion;
	string[] text;
	DecisionsObject dobject;
	string variable;
	public Text textbox;
	public DiologueObject(string text)
	{
		this.textbox = SceneScript.tBox;
		this.text = new string[1];
		this.text[0] = text;
	}
	public DiologueObject(DecisionsObject dobject, string var, string[] text)
	{
		this.textbox = SceneScript.tBox;
		this.dobject = dobject;
		this.variable = var;
		this.text = text;
		this.isDesicion = true;
	}
	public DiologueObject(bool shouldshow)
	{
		isShowCommand = true;
		this.shouldshow = shouldshow;
		this.text = new string[1];
		this.text[0] = "";
	}
	public bool isShowDialogue() { return isShowCommand; }
	public bool shouldShow() { return shouldshow; }

	public void show()
	{
		if (isDesicion)
		{
			ButtonCreator bc = SceneScript.buttonParentObject.AddComponent<ButtonCreator>();
			bc.make(text, variable, dobject);
		}
		else
		{
			textbox.text = text[0];
		}
	}
	public string get()
	{
		return text[0];
	}
}
