using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Registry<T> : MonoBehaviour,IRegistry<T> where T : IRegistryEntity
{
    public List<T> reg { get; } = new List<T>();

    public void Add(T re)
    {
        reg.Add(re);
        re.rId = reg.Count - 1;
    }

    public void Remove(T re)
    {
        reg[re.rId] = reg[reg.Count - 1];
        reg[re.rId].rId = re.rId;
        reg.RemoveAt(reg.Count - 1);
    }
}

public interface IRegistry<T>
{
     void Add(T re);
     void Remove(T re);
     public List<T> reg { get; }

}
