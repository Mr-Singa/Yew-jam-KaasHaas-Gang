using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SceneScript : MonoBehaviour
{
	public static readonly string SCENEORDER_FOLDER = "Assets/gamescenes/";
	public static readonly string SCENEORDER_FILE = "sceneorder.txt";
	public Text textbox;
	public Text characternaambox;
	public GameObject buttonparent;
	public  GameObject buttonprefab;
	public GameObject backgroundimageparent;

	public static Text tBox;
	public static Text characterNaamBox;
	public static GameObject buttonParentObject;
	public static GameObject buttonPrefab;
	public static bool isMakingDecision = false;
	public static GameObject backgroundImageParent;

	// i know... .. .
	public GameObject characterspritesparent1;
	public GameObject characterspritesparent2;
	public GameObject characterspritesparent3;
	public GameObject characterspritesparent4;
	public static GameObject[] characterSpriteParents;

	private List<SceneObject> scenes = new List<SceneObject>();
	private DecisionsObject gameVariables;
	private int currentSceneIndex = 0;
	private bool gameIsDone = false;
	

    // Start is called before the first frame update
    void Start()
    {
		tBox = textbox;
		characterNaamBox = characternaambox;
		buttonParentObject = buttonparent;
		buttonPrefab = buttonprefab;
		backgroundImageParent = backgroundimageparent;

		characterSpriteParents = new GameObject[4];
		characterSpriteParents[0] = characterspritesparent1;
		characterSpriteParents[1] = characterspritesparent2;
		characterSpriteParents[2] = characterspritesparent3;
		characterSpriteParents[3] = characterspritesparent4;

		gameVariables = new DecisionsObject();

		try
		{
			using (StreamReader sr = new StreamReader(SCENEORDER_FOLDER + SCENEORDER_FILE))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					parseLine(line);
				}
			}
			scenes[currentSceneIndex].start();
		}
		catch (Exception e)
		{
			Debug.Log(e);
		}

    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonDown(0) && gameIsDone == false && isMakingDecision == false)
		{
			bool sceneisdone = scenes[currentSceneIndex].advance();
			// if the scene is now over
			if (sceneisdone)
			{
				scenes[currentSceneIndex].destroy();
				currentSceneIndex++;
				// check if there is another one available
				if (currentSceneIndex != scenes.Count)
				{
					// attempt to start a new scene
					while (scenes[currentSceneIndex].start() != true)
					{
						// check if the next scene exists
						if (currentSceneIndex >= scenes.Count - 1)
						{
							gameIsDone = true;
							break;
						}
						currentSceneIndex++;
					}
					
				}
				else
				{
					gameIsDone = true;
				}
			}
		}

	}

	void parseLine(string line)
	{
		int dubblepointpos = line.IndexOf(':');
		string option = line.Substring(0, dubblepointpos);
		// declaring the variables
		if (option.Equals("vars"))
		{
			string v = line.Substring(dubblepointpos + 1);
			string[] values = v.Split(',');
			foreach (string value in values)
			{
				gameVariables.create(value);
			}
		}

		// a scene
		if (option.Equals("s"))
		{
			string sceneName = line.Substring(dubblepointpos + 1);
			scenes.Add(new SceneObject(gameVariables, sceneName));
		}
		// a choice
		if (option.Contains("="))
		{
			string varname = line.Substring(0, line.IndexOf('='));
			string varvalue = line.Substring(line.IndexOf('=') + 1, dubblepointpos - line.IndexOf('=') - 1);
			string sceneName = line.Substring(dubblepointpos + 1);
			scenes.Add(new SceneObject(gameVariables, sceneName, varname, varvalue));
		}
	}

}
