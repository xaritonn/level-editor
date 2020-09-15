using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public delegate void ChangeAction(Dictionary<Item, int> inventory);
    public static event ChangeAction OnChange;

    public Dictionary<Item, int> inventory = new Dictionary<Item, int>();
    public Item[] items;

    public int money = 500;

    Transform cameraTransform;
    float distancePlace = 2.0f;

    GameObject parentObject;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        parentObject = new GameObject("Objects");
    }

    void OnEnable()
    {
        ItemButton.OnUsed += PickItem;
        CartController.OnBuy += AddItems;
    }

    void AddItems(Dictionary<Item, int> itemsInCart, int totalPrice)
    {
        money -= totalPrice;

        foreach (var item in itemsInCart)
        {
            AddItem(item.Key, item.Value);
        }
    }

    void PickItem(Item item)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] -= 1;
            InstanceItem(item);
        }

        if (OnChange != null)
            OnChange(inventory);
    }

    void AddItem(Item item, int count)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] += count;
        }
        else
        {
            inventory.Add(item, count);
        }

        if (OnChange != null)
            OnChange(inventory);
    }

    public void InstanceItem(Item item)
    {
        Vector3 instancePos = cameraTransform.position + cameraTransform.forward * distancePlace;
        Instantiate(item, instancePos, Quaternion.identity, parentObject.transform);
    }

    void OnDisable()
    {
        ItemButton.OnUsed -= PickItem;
        CartController.OnBuy -= AddItems;
    }
}
