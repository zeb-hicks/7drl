using System;
using System.Collections.Generic;

namespace Hiveware {
  public class UniqueDictionary<TKey, TValue>: Dictionary<TKey, TValue> {
    private object syn = new object();

    // private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
    private Dictionary<TValue, TKey> inverse = new Dictionary<TValue, TKey>();

    private bool _readonly;
    public bool IsReadOnly {
      get { return _readonly; }
      set { _readonly = value; }
    }
    
    public new void Add(TKey key, TValue value) {
      lock(syn) {
        if (ContainsKey(key)) {
          throw new Exception("Duplicate key.");
        }
        if (inverse.ContainsKey(value)) {
          throw new Exception("Duplicate value.");
        }
        base.Add(key, value);
        inverse.Add(value, key);
      }
    }

    public new bool TryAdd(TKey key, TValue value) {
      lock(syn) {
        if (ContainsKey(key)) return false;
        if (inverse.ContainsKey(value)) return false;
        base.Add(key, value);
        inverse.Add(value, key);
        return true;
      }
    }

    public TValue GetFromKey(TKey key) {
      if (!base.ContainsKey(key)) throw new Exception("Key does not exist.");
      return base[key];
    }

    public TKey GetFromValue(TValue value) {
      if (!inverse.ContainsKey(value)) throw new Exception("Value does not exist.");
      return inverse[value];
    }
    
  }
}
