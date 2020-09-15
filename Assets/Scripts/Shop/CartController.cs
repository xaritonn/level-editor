using System.Collections.Generic;
using UnityEngine;

public class CartController : MonoBehaviour
{
    public delegate void AddInCartAction(Item item, int totalPrice);
    public static event AddInCartAction OnAddInCart;

    public delegate void RemoveFromCartAction(Item item, int totalPrice);
    public static event RemoveFromCartAction OnRemoveFromCart;

    public delegate void BuyAction(Dictionary<Item, int> itemsInCart, int totalPrice);
    public static event BuyAction OnBuy;

    public Dictionary<Item, int> itemsInCart = new Dictionary<Item, int>();
    public Dictionary<Item, CartButton> cartButtons = new Dictionary<Item, CartButton>();

    int totalPrice;

    [SerializeField] CartButton cartButton;
    [SerializeField] Transform cartPanel;

    void OnEnable()
    {
        ShopButton.OnClicked += AddItem;
        CartButton.OnClicked += RemoveItem;
    }

    void AddItem(Item item)
    {
        if(itemsInCart.ContainsKey(item))
        {
            itemsInCart[item] += 1;
        }
        else
        {
            itemsInCart.Add(item, 1);
            CartButton newCartButton = Instantiate(cartButton, cartPanel).GetComponent<CartButton>();
            cartButtons.Add(item, newCartButton);
            cartButtons[item].Init(item);
        }

        cartButtons[item].UpdateInfo(item, itemsInCart[item]);

        totalPrice += item.price;

        if (OnAddInCart != null)
            OnAddInCart(item, totalPrice);
    }

    void RemoveItem(Item item)
    {
        itemsInCart[item] -= 1;

        cartButtons[item].UpdateInfo(item, itemsInCart[item]);

        if (itemsInCart[item] <= 0)
        {
            GameObject removedButton = cartButtons[item].gameObject;
            cartButtons.Remove(item);
            itemsInCart.Remove(item);
            Destroy(removedButton);
        }

        totalPrice -= item.price;

        if (OnRemoveFromCart != null)
            OnRemoveFromCart(item, totalPrice);
    }

    public void Buy()
    {
        if (OnBuy != null)
            OnBuy(itemsInCart, totalPrice);

        totalPrice = 0;
        CleanCart();
    }

    void CleanCart()
    {
        foreach (Item cartItem in itemsInCart.Keys)
        {
            Destroy(cartButtons[cartItem].gameObject);
        }

        cartButtons.Clear();
        itemsInCart.Clear();
    }

    void OnDisable()
    {
        ShopButton.OnClicked -= AddItem;
        CartButton.OnClicked -= RemoveItem;
    }
}
