using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Medic,
    Equipment,
    Weapon,
    Default
}
public abstract class ItemObject : ScriptableObject
{
    public GameObject prefab;
    public ItemType type;
    public Sprite icon;
    [TextArea(15,20)]
    public string description;
    //if need castting time
    public float castTime;
    //if item healing
    public int healing;

    public int stamina;

    public int secondForActiveEff;
}
