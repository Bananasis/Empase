using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistryEntity : MonoBehaviour,IRegistryEntity
{
    public int rId { get; set; }
}

public interface IRegistryEntity {
    public int rId { get; set; }
}
