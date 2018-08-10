using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Moong.FluentScheduler.Util
{
  public sealed class ConcurrentSet<T> : ICollection<T>
  {
    private readonly ConcurrentDictionary<T, byte> _dictionary;

    public ConcurrentSet()
    {
      _dictionary = new ConcurrentDictionary<T, byte>();
    }

    public int Count => _dictionary.Count;

    public bool IsReadOnly => false;

    public IEnumerator<T> GetEnumerator()
    {
      foreach (var kvp in _dictionary)
      {
        yield return kvp.Key;
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    public bool Add(T value)
    {
      return _dictionary.TryAdd(value, 0);
    }

    public void Clear()
    {
      _dictionary.Clear();
    }

    public bool Contains(T value)
    {
      return _dictionary.ContainsKey(value);
    }

    public bool Remove(T value)
    {
      return _dictionary.TryRemove(value, out var b);
    }

    void ICollection<T>.Add(T item)
    {
      this.Add(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      throw new System.NotImplementedException();
    }
  }
}