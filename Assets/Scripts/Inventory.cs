using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private InventorySlot[] items = new InventorySlot[3];

    public bool Store(float healValue, Sprite sprite)
    {
        foreach (var slot in items)
        {
            if (slot.IsEmpty)
            {
                slot.Fill(healValue, sprite);
                return true;
            }
        }

        return false;
    }
    
}
