using System.Collections.Generic;

/// <summary>
/// This is a nested generic list
/// We will use it to hold the geometric and visual data of our Hexes
/// While allowing for more than one mesh (multiple leves of objects in the map)
/// </summary>
public static class ListPool<T>
{
    static Stack<List<T>> stack = new Stack<List<T>>();

    /// <summary>
    /// Get List from pool
    /// </summary>
    /// <returns></returns>
    public static List<T> Get()
    {
        if (stack.Count > 0)
        {
            return stack.Pop();
        }
        return new List<T>();
    }


    /// <summary>
    /// Return list to pool
    /// </summary>
    /// <param name="list"></param>
    public static void Add(List<T> list)
    {
        list.Clear();
        stack.Push(list);
    }

}