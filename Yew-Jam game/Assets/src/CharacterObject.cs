using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterObject
{
	public static readonly Rect dimensions = new Rect( 0, 0, 32, 32 );
	string _name;
	Image defaultImage;
	Sprite defaultSprite;
	GameObject imgParent;
	public string name {  get { return _name; } set { _name = value;  } }

	public CharacterObject(string name)
	{
		this._name = name;
		defaultSprite = Resources.Load<Sprite>("characters/" + _name);
	}

	public void show()
	{
		foreach (var obj in SceneScript.characterSpriteParents)
		{
			if (obj.GetComponent<Image>() == null)
			{
				defaultImage = obj.AddComponent<Image>();
				defaultImage.sprite = defaultSprite;
				imgParent = obj;
				break;
			}
		}
		imgParent?.SetActive(true);
	}
	public void showName()
	{
		SceneScript.characterNaamBox.text = name;
	}
	public void hide()
	{
		if (imgParent?.GetComponent<Image>() != null)
		{
			imgParent?.SetActive(false);
			SceneScript.Destroy(imgParent?.GetComponent<Image>());
		}
	}	
}
