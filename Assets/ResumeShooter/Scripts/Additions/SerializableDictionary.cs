using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TK, TV> : ISerializationCallbackReceiver
{
	[SerializeField] public List<TK> keys;
	[SerializeField] private List<TV> values;

	public ref Dictionary<TK, TV> GetDictionary { get { return ref dictionary; } }

	private Dictionary<TK, TV> dictionary = new Dictionary<TK, TV>();


	public void OnBeforeSerialize()
	{
		
	}

	public void OnAfterDeserialize()
	{
		if (keys.Count == 0 || values.Count == 0) { return; }

		int minLength = Mathf.Min(keys.Count, values.Count);
		dictionary.Clear();

		for(int i = 0; i < minLength; i++)
		{
			if (keys[i] == null || values[i] == null)
				continue;

		dictionary.Add(keys[i], values[i]);
		}
	}
}