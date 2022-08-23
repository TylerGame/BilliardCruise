using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Collections;

[Serializable]
public class HashTable<K, T>: Dictionary<K, T> where T : new() 
{
    public new T this[K key]
    {
        get => GetValue(key);
        set => SetValue(key, value);
    }

    public override string ToString()
    {
        string result = "";
        foreach(KeyValuePair<K,T> pair in this) {
            result += pair.Key + "=>" + pair.Value+", ";
        }
        return result;
    }

    public HashTable(): base() {
    }

    
    public HashTable(HashTable<K,T> toClone): base(toClone)
    {
    }

    public int CountWithout(T value)
    {
        int result = 0;
        foreach (K key in Keys)
        {
            if (!this[key].Equals(value)) result++;
        }
        return result;
    }

    public List<K> KeysWithout(T value)
    {
        List<K> result = new List<K>();
        foreach (K key in Keys)
        {
            if (!this[key].Equals(value)) result.Add(key);
        }
        return result;
    }
    /*
    public static HashTable<K, T> operator+ (HashTable<K, T> a, HashTable<K, T> b)
    {
        HashTable<K, T> result = new HashTable<K, T>(a);
        foreach (K key in b.Keys)
        {
            result[key] = (dynamic)result[key] + (dynamic)b[key];
        }
        return result;
    }
    */
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is HashTable<K,T> && Equals((HashTable<K, T>)obj);
    }

    public override int GetHashCode()
    {
        return 0;
    }

    public static bool operator== (HashTable<K, T> a, HashTable<K, T> b)
    {
        var aKeys = a.Keys;
        var bKeys = b.Keys;

        if (aKeys.Count != bKeys.Count) return false;

        var aExcept = aKeys.Except(bKeys);
        var bExcept = bKeys.Except(aKeys);

        if (aExcept.Any() || bExcept.Any()) return false;

        foreach(K key in aKeys)
        {
            if (!EqualityComparer<T>.Default.Equals(a[key], b[key])) return false;
        }

        return true;
    }
    public static bool operator!= (HashTable<K, T> a, HashTable<K, T> b)
    {
        return !(a == b);
    }
    
    /*
    public static HashTable<K, T> operator* (HashTable<K, T> a, float b)
    {
        HashTable<K, T> result = new HashTable<K, T>(a);
        foreach (K key in a.Keys)
        {
            result[key] = (dynamic)result[key] * b;
        }
        return result;
    }
    */
    void SetValue(K key, T value)
    {
        if (ContainsKey(key))
        {
            ((IDictionary)this)[key] = value;
        }
        else
        {
            Add(key, value);
        }
    }

    [JsonIgnore]
    public new List<K> Keys
    {
        get
        {
            return new List<K>(base.Keys);
        }
    }

    T GetValue(K key)
    {
        T value = default(T);
        if(!TryGetValue(key, out value)) {
            ((IDictionary)this)[key] = new T();
            return base[key];
        } 
        return value;
    }
}