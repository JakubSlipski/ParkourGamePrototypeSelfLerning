using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Eqiupment Object", menuName = "Inventory System/Items/Eqiupment")]
public class EqiupmentObject : ItemObject
{
    public float armor;
    public float armorToxic;
    
    public void Awake()
    {
        type = ItemType.Equipment;
    }
}
