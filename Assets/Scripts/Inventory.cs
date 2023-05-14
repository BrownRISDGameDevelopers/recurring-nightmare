using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject inventory;
    private InventorySlot[] _items;

    private void Start()
    {
        _items = inventory.GetComponentsInChildren<InventorySlot>();
    }

    public bool Store(float healValue, Sprite sprite)
    {
        foreach (var slot in _items)
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
