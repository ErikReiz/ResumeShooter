using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public abstract class SerializableDictionaryBase
{
	public abstract class Storage { }

	protected class Dictionary<TKey, TValue> : System.Collections.Generic.Dictionary<TKey, TValue>
	{
		public Dictionary() { }
		public Dictionary(IDictionary<TKey, TValue> dict) : base(dict) { }
		public Dictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}

[Serializable]
public sealed class SerializableDictionary<TKey, TValue> : SerializableDictionaryBase, IDictionary<TKey, TValue>, ISerializationCallbackReceiver
{
	#region SERIALIZE FIELDS
	[SerializeField] public TKey[] keys;
	[SerializeField] private TValue[] values;
	#endregion

	#region PROPERTIES
	public ICollection<TKey> Keys { get { return dictionary.Keys; } }
	public ICollection<TValue> Values { get { return dictionary.Values; } }
	public TKey[] KeysArray { get { return keys; } }
	public TValue[] ValuesArray { get { return values; } }
	public int Count { get { return dictionary.Count; } }
	public bool IsReadOnly { get { return ((IDictionary<TKey, TValue>)dictionary).IsReadOnly; } }

	public TValue this[TKey key] { get { return dictionary[key]; } set { dictionary[key] = value; } }
	#endregion

	#region FIELDS
	Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
	#endregion

	public void OnBeforeSerialize() { }

	public void OnAfterDeserialize()
	{
		if (keys.Length == 0 || values.Length == 0) { return; }

		int minLength = Mathf.Min(keys.Length, values.Length);
		dictionary.Clear();

		for (int i = 0; i < minLength; i++)
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

	public void Add(KeyValuePair<TKey, TValue> item)
	{
		((IDictionary<TKey, TValue>)dictionary).Add(item);
	}

	public void Clear()
	{
		((IDictionary<TKey, TValue>)dictionary).Clear();
	}

	public bool Contains(KeyValuePair<TKey, TValue> item)
	{
		return ((IDictionary<TKey, TValue>)dictionary).Contains(item);
	}

	public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
	{
		((IDictionary<TKey, TValue>)dictionary).CopyTo(array, arrayIndex);
	}

	public bool Remove(KeyValuePair<TKey, TValue> item)
	{
		return ((IDictionary<TKey, TValue>)dictionary).Remove(item);
	}

	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		return dictionary.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return dictionary.GetEnumerator();
	}
}