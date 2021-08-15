using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SceneObject
{
	public static readonly Rect backgroundDimensions = new Rect(0, 0, 64, 64);
	string _name;
	CharacterObject[] characters;
	static Image _background = null;

	bool isDecision = false;
	string varName = "";
	string expectedValue = "";
	Sprite s;
	DecisionsObject decisionsObject;
	List<System.Tuple<CharacterObject, DiologueObject>> dialogue = new List<System.Tuple<CharacterObject, DiologueObject>>();
	int currentDialogueIndex = 0;

	public SceneObject(DecisionsObject decisionsObject, string name)
	{
		_name = name;
		this.decisionsObject = decisionsObject;

		loadInData(name);
	}
	public SceneObject(DecisionsObject decisionsObject, string name, string varName, string expectedValue)
	{
		_name = name;
		isDecision = true;
		this.varName = varName;
		this.expectedValue = expectedValue;
		this.decisionsObject = decisionsObject;
		loadInData(name);
	}
	public void destroy()
	{
		//SceneScript.Destroy(_background);
	}

	/// <summary>
	/// starts the scene, if it has a decision added checks first
	/// </summary>
	/// <returns>whether the scene successfully started</returns>
	public bool start()
	{
		//if (_background != null)
		//	destroy();
		_background.sprite = s;
		SceneScript.backgroundImageParent.SetActive(true);
		if (isDecision)
		{
			if (decisionsObject.get(varName) != expectedValue)
			{
				return false;
			}
		}
		advance();
		return true;
	}
	/// <summary>
	/// advances the script
	/// </summary>
	/// <returns>whether the scene is done or not</returns>
	public bool advance()
	{
		if (currentDialogueIndex >= dialogue.Count)
			return true;

		bool hasShownNewText = false;

		while (!hasShownNewText)
		{
			try
			{
				var d = dialogue[currentDialogueIndex];
				var character = d.Item1;
				var text = d.Item2;
				currentDialogueIndex++;

				if (text.isShowDialogue())
				{
					if (text.shouldShow())
						character.show();
					else
						character.hide();
				}
				else
				{
					character?.showName();
					text.show();
					hasShownNewText = true;
				}
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}



		}


		return false;
	}
	private void loadInData(string name)
	{
		if (_background == null) _background = SceneScript.backgroundImageParent.AddComponent<Image>();

		s = Resources.Load<Sprite>("backgrounds/" + _name);

		using (StreamReader sr = new StreamReader(SceneScript.SCENEORDER_FOLDER + name + ".txt"))
		{
			string line;
			while ((line = sr.ReadLine()) != null)
			{
				parseLine(line);
			}
		}
	}
	private void parseLine(string line)
	{
		int dubblepointpos = line.IndexOf(':');
		if (dubblepointpos < 0)
		{
			int i = 0;
			i += 1;
		}
		string option = line.Substring(0, dubblepointpos);
		string data = line.Substring(dubblepointpos + 1);
		if (option.Equals("chars"))
		{
			string[] chars = data.Split(',');
			characters = new CharacterObject[chars.Length];
			for (int i = 0; i < chars.Length; i++)
			{
				string c = chars[i];
				characters[i] = new CharacterObject(c);
			}
		}
		else if (decisionsObject.contains(option))
		{
			string[] text = data.Split('/');

			DiologueObject d = new DiologueObject(decisionsObject, option, text);
			dialogue.Add(new System.Tuple<CharacterObject, DiologueObject>(null, d));
		}
		else if (option.Contains("+"))
		{
			foreach (CharacterObject c in characters)
			{
				if (c.name.Equals(data))
				{
					// command to show a character
					DiologueObject d = new DiologueObject(true);
					dialogue.Add(new System.Tuple<CharacterObject, DiologueObject>(c, d));
					break;
				}
			}
		}
		else if (option.Contains("-"))
		{
			foreach (CharacterObject c in characters)
			{
				if (c.name.Equals(data))
				{
					// command to show a character
					DiologueObject d = new DiologueObject(false);
					dialogue.Add(new System.Tuple<CharacterObject, DiologueObject>(c, d));
					break;
				}
			}
		}
		else
		{
			try
			{
				foreach (CharacterObject c in characters)
				{
					if (c.name.Equals(option))
					{
						DiologueObject d = new DiologueObject(data);
						dialogue.Add(new System.Tuple<CharacterObject, DiologueObject>(c, d));
						break;
					}
				}
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
		}
	}
}