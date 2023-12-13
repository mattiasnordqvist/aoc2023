namespace AdventOfCode;

public class CountDictionary<TKey> : Dictionary<TKey, long>
{
    public void AddCount(TKey key, long count)
    {
        if (TryGetValue(key, out var val))
        {
            this[key] = val + count;
        }
        else
        {
            Add(key, count);
        }
    }
}

public class ListDictionary<TKey, TValue> : Dictionary<TKey, List<TValue>>
{
    public void Add(TKey key, TValue value)
    {
        if (TryGetValue(key, out var list))
        {
            list.Add(value);
        }
        else
        {
            Add(key, [value]);
        }
    }
}
