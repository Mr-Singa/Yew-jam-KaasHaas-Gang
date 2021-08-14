using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCreator : MonoBehaviour
{
	DecisionsObject dobject;
	string var;
	List<GameObject> buttons = new List<GameObject>();
	public void make(string[] options, string var, DecisionsObject dobject)
	{
		SceneScript.isMakingDecision = true;
		this.dobject = dobject;
		this.var = var;
		for (int i = 0; i < options.Length; i++)
		{
			GameObject button = (GameObject)Instantiate(SceneScript.buttonPrefab);
			button.transform.SetParent(SceneScript.buttonParentObject.transform);
			button.transform.position = new Vector3(0, 1.6f - (i * 0.6f));

			// omdat c# fucking retarted is met lambda captures blijkbaar...
			int capturedindex = i;

			button.GetComponent<Button>().onClick.AddListener(() =>
			{
				dobject.set(var, (capturedindex + 1) + "");
				onClick();
			});
			button.transform.GetChild(0).GetComponent<Text>().text = options[i];
			button.SetActive(true);
			buttons.Add(button);
		}
	}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	void onClick()
	{
		foreach (GameObject b in buttons)
		{
			Destroy(b);
		}
		SceneScript.isMakingDecision = false;
	}
}
