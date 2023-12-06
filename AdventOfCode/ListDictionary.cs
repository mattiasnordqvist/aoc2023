namespace AdventOfCode;

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
