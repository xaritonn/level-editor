using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    public delegate void ClickAction(Item item);
    public static event ClickAction OnClicked;

    public Item shopItem;
    public Button button;

    [SerializeField] Image itemIcon;
    [SerializeField] Text priceText;

    public void Init(Item item)
    {
        itemIcon.sprite = item.icon;
        priceText.text = item.price + " $";
        shopItem = item;

        button = GetComponent<Button>();
    }

    public void AddInCart()
    {
        if (OnClicked != null)
            OnClicked(shopItem);
    }
}
