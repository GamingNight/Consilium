using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[Serializable]
public class SerializableDictionary<K, V> : ISerializationCallbackReceiver {
    private System.Object mutexForDictionary = new System.Object();
    private Dictionary<K, V> dictionary = new Dictionary<K, V>();

    [SerializeField] private List<K> listOfKeys = new List<K>();
    [SerializeField] private List<V> listOfValues = new List<V>();

    public SerializableDictionary() {

        lock (mutexForDictionary) {
            dictionary = new Dictionary<K, V>();
        }
    }

    #region Serialization Related
    public void OnBeforeSerialize() {
        lock (mutexForDictionary) {
            listOfKeys.Clear();
            listOfValues.Clear();

            foreach (KeyValuePair<K, V> eachPair in dictionary) {
                listOfKeys.Add(eachPair.Key);
                listOfValues.Add(eachPair.Value);
            }
        }
    }

    public void OnAfterDeserialize() {
        lock (mutexForDictionary) {

            dictionary.Clear();
            checkIfKeyAndValueValid();

            for (int i = 0; i < listOfKeys.Count; ++i) {
                dictionary.Add(listOfKeys[i], listOfValues[i]);
            }
        }
    }
    #endregion

    #region Dictionary Interface
    public void Add(K _key, V _value) {
        lock (mutexForDictionary) {
            dictionary.Add(_key, _value);
        }
    }
    public V this[K _key]
    {
        get
        {
            lock (mutexForDictionary) {
                return dictionary[_key];
            }
        }
        set
        {
            lock (mutexForDictionary) {
                dictionary[_key] = value;
            }
        }
    }
    public void Remove(K _key) {
        lock (mutexForDictionary) {
            dictionary.Remove(_key);
        }
    }

    public Dictionary<K, V>.KeyCollection Keys
    {
        get
        {
            lock (mutexForDictionary) {
                return dictionary.Keys;
            }
        }
    }

    public Dictionary<K, V>.ValueCollection Values
    {
        get
        {
            lock (mutexForDictionary) {
                return dictionary.Values;
            }
        }
    }

    public int Count
    {
        get
        {
            lock (mutexForDictionary) {
                return dictionary.Count;
            }
        }
    }


    #endregion

    private void checkIfKeyAndValueValid() {
        int numberOfKeys = listOfKeys.Count;

        int numberOfValues = listOfValues.Count;

        if (numberOfKeys != numberOfValues) {
            throw new System.ArgumentException("(nKey, nValue) = ("
                                        + numberOfKeys.ToString() + ", "
                                        + numberOfValues.ToString() + ") are NOT Equal!");
        }
    }
}//End of class