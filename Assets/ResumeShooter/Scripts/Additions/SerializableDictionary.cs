using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
	#region SERIALIZE FIELDS
	[SerializeField] public TKey[] keys;
	[SerializeField] private TValue[] values;
	#endregion

	#region PROPERTIES
	public Dictionary<TKey, TValue> Dictionary { get { return dictionary; } }

	public int Count { get { return dictionary.Count; } }
	public TValue this[TKey key] { get { return dictionary[key]; } set { dictionary[key] = value; } }
	#endregion

	#region FIELDS
	private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
	#endregion

	public void OnBeforeSerialize()
	{

	}

	public void OnAfterDeserialize()
	{
		if (keys.Length == 0 || values.Length == 0) { return; }

		int minLength = Mathf.Min(keys.Length, values.Length);
		dictionary.Clear();

		for(int i = 0; i < minLength; i++)
		{
			if (keys[i] == null || values[i] == null)
				continue;

		dictionary.TryAdd(keys[i], values[i]);
		}
	}

	public void Add(TKey key, TValue value)
	{
		dictionary.Add(key, value);
	}

	public bool ContainsKey(TKey key)
	{
		return dictionary.ContainsKey(key);
	}

	public bool Remove(TKey key)
	{
		return dictionary.Remove(key);
	}

	public bool TryGetValue(TKey key, out TValue value)
	{
		return dictionary.TryGetValue(key, out value);
	}
	
	public void Clear()
	{
		dictionary.Clear();
	}
}