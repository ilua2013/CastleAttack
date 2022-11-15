using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public class Entry<T>
{
    public string Key;
    public T Value;

    public Entry(string key, T value)
    {
        Key = key;
        Value = value;
    }
}

[Serializable]
public class Dictionary
{
    [SerializeField] private List<Entry<bool>> _entiesBool = new List<Entry<bool>>();
    [SerializeField] private List<Entry<int>> _entiesInt = new List<Entry<int>>();
    [SerializeField] private List<Entry<float>> _entiesFloat = new List<Entry<float>>();
    [SerializeField] private List<Entry<string>> _entiesString = new List<Entry<string>>();
    [SerializeField] private List<Entry<Card[]>> _entiesCard = new List<Entry<Card[]>>();

    public IEnumerable<Entry<bool>> EntiesBool => _entiesBool;
    public IEnumerable<Entry<int>> EntiesInt => _entiesInt;
    public IEnumerable<Entry<float>> EntiesFloat => _entiesFloat;
    public IEnumerable<Entry<string>> EntiesString => _entiesString;
    public IEnumerable<Entry<Card[]>> EntiesCard => _entiesCard;

    public bool ContainsKey(string key)
    {
        foreach (var entry in _entiesBool)
            if (entry.Key == key)
                return true;

        foreach (var entry in _entiesInt)
            if (entry.Key == key)
                return true;

        foreach (var entry in _entiesFloat)
            if (entry.Key == key)
                return true;

        foreach (var entry in _entiesString)
            if (entry.Key == key)
                return true;

        foreach (var entry in _entiesCard)
            if (entry.Key == key)
                return true;

        return false;
    }

    public void Add(string key, bool value)
    {
        if (ContainsKey(key))
        {
            foreach (var entry in _entiesBool)
                if (entry.Key == key)
                    entry.Value = value;
        }
        else
        {
            _entiesBool.Add(new Entry<bool>(key, value));
        }
    }

    public void Add(string key, int value)
    {
        if (ContainsKey(key))
        {
            foreach (var entry in _entiesInt)
                if (entry.Key == key)
                    entry.Value = value;
        }
        else
        {
            _entiesInt.Add(new Entry<int>(key, value));
        }
    }

    public void Add(string key, float value)
    {
        if (ContainsKey(key))
        {
            foreach (var entry in _entiesFloat)
                if (entry.Key == key)
                    entry.Value = value;
        }
        else
        {
            _entiesFloat.Add(new Entry<float>(key, value));
        }
    }

    public void Add(string key, string value)
    {
        if (ContainsKey(key))
        {
            foreach (var entry in _entiesString)
                if (entry.Key == key)
                    entry.Value = value;
        }
        else
        {
            _entiesString.Add(new Entry<string>(key, value));
        }
    }

    public void Add(string key, Card[] value)
    {
        if (ContainsKey(key))
        {
            foreach (var entry in _entiesCard)
                if (entry.Key == key)
                    entry.Value = value;
        }
        else
        {
            _entiesCard.Add(new Entry<Card[]>(key, value));
        }
    }

    public bool GetBool(string key)
    {
        foreach (var entry in _entiesBool)
            if (entry.Key == key)
                return entry.Value;

        throw new InvalidOperationException($"Key:{key} is not found");
    }

    public string GetString(string key)
    {
        foreach (var entry in _entiesString)
            if (entry.Key == key)
                return entry.Value;

        throw new InvalidOperationException($"Key:{key} is not found");
    }

    public int GetInt(string key)
    {
        foreach (var entry in _entiesInt)
            if (entry.Key == key)
                return entry.Value;

        throw new InvalidOperationException($"Key:{key} is not found");
    }

    public float GetFloat(string key)
    {
        foreach (var entry in _entiesFloat)
            if (entry.Key == key)
                return entry.Value;

        throw new InvalidOperationException($"Key:{key} is not found");
    }

    public Card[] GetDeck(string key)
    {
        foreach (var entry in _entiesCard)
            if (entry.Key == key)
                return entry.Value;

        throw new InvalidOperationException($"Key:{key} is not found");
    }
}


public static class Saves
{
    private static Dictionary _saves = new Dictionary();

    static Saves()
    {
        string json = "";

        if (PlayerPrefs.HasKey("Saving"))
            json = PlayerPrefs.GetString("Saving");

        _saves = GetSaveFromJson(json);
    }

    private static Dictionary GetSaveFromJson(string json)
    {
        if (string.IsNullOrEmpty(json))
            return new Dictionary();

        return JsonUtility.FromJson<Dictionary>(json);
    }

    private static string GetJsonFromSave(Dictionary saving)
    {
        return JsonUtility.ToJson(saving);
    }

    public static void Save()
    {
        string json = GetJsonFromSave(_saves);
        PlayerPrefs.SetString("Saving", json);
    }

    public static bool HasKey(string key)
    {
        return _saves.ContainsKey(key);
    }

    public static void SetBool(string key, bool value)
    {
        _saves.Add(key, value);
    }

    public static void SetInt(string key, int value)
    {
        _saves.Add(key, value);
    }

    public static void SetFloat(string key, float value)
    {
        _saves.Add(key, value);
    }

    public static void SetString(string key, string value)
    {
        _saves.Add(key, value);
    }

    public static void SetDeck(string key, Card[] value)
    {
        _saves.Add(key, value);
    }

    public static bool GetBool(string key)
    {
        return _saves.GetBool(key);
    }

    public static string GetString(string key)
    {
        return _saves.GetString(key);
    }

    public static int GetInt(string key)
    {
        return _saves.GetInt(key);
    }

    public static float GetFloat(string key)
    {
        return _saves.GetFloat(key);
    }

    public static Card[] GetDeck(string key)
    {
        return _saves.GetDeck(key);
    }
}
