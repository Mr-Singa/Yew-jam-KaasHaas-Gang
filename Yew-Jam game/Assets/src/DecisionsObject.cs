using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionsObject 
{
	private Dictionary<string, string> variables = new Dictionary<string, string>();
	public static readonly string UNSET_VALUE = "-1";
	public static readonly string TRUE_VALUE = "1";
	public static readonly string FALSE_VALUE = "0";
	public DecisionsObject()
	{}

	public void create(string name)
	{
		variables.Add(name, UNSET_VALUE);
	}
	public string get(string name)
	{
		return variables[name];
	}
	public void set(string name, string value)
	{
		variables[name] = value;
	}
	public bool contains(string name)
	{
		return variables.ContainsKey(name);
	}
}
