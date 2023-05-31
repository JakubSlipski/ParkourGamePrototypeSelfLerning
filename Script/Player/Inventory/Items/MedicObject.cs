using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Medic Object", menuName = "Inventory System/Items/Medic")]
public class MedicObject : ItemObject
{
    //public int healingValue;
    //public float castTime;

    public void Awake()
    {
        type = ItemType.Medic;
    }
}
