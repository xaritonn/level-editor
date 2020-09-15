using UnityEngine;
using UnityEngine.UI;

public class CartButton : MonoBehaviour
{
    public delegate void ClickAction(Item item);
    public static event ClickAction OnClicked;

    [SerializeField] Image itemIcon;
    [SerializeField] Text priceText;
    [SerializeField] Text countText;

    Item shopItem;

    public void Init(Item item)
    {
        itemIcon.sprite = item.icon;
        shopItem = item;
    }

    public void UpdateInfo(Item item, int count)
    {
        priceText.text = item.price * count + "$";
        countText.text = count.ToString();
    }

    public void Return()
    {
        if (OnClicked != null)
            OnClicked(shopItem);
    }
}
