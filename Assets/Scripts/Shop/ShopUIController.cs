using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIController : MonoBehaviour
{
    [SerializeField] GameObject button;
    [SerializeField] Transform buttonPanel;
    [SerializeField] Text totalPriceText;

    List<ShopButton> buttons = new List<ShopButton>();
    Inventory inventory;
    CartController cartController;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        cartController = FindObjectOfType<CartController>();
        AddButtons();
    }

    void OnEnable()
    {
        CartController.OnAddInCart += OnAddInCart;
        CartController.OnRemoveFromCart += OnRemoveFromCart;
        CartController.OnBuy += OnBuy;
    }

    void OnAddInCart(Item item, int totalPrice)
    {
        UpdatePriceInfo(totalPrice);
        CheckButtonsState(totalPrice);
    }

    void OnRemoveFromCart(Item item, int totalPrice)
    {
        UpdatePriceInfo(totalPrice);
        CheckButtonsState(totalPrice);
    }

    void AddButtons()
    {
        for (int i = 0; i < inventory.items.Length; i++)
        {
            GameObject newButton = Instantiate(button, buttonPanel);
            buttons.Add(newButton.GetComponent<ShopButton>());
            Item item = inventory.items[i];
            buttons[i].Init(item);
        }

        CheckButtonsState(0);
    }

    void CheckButtonsState(int totalPrice)
    {
        foreach (ShopButton button in buttons)
        {
            button.button.interactable = inventory.money - totalPrice - button.shopItem.price >= 0;
        }
    }

    void UpdatePriceInfo(int totalPrice)
    {
        totalPriceText.text = inventory.money + "$ \n" + totalPrice + " $";
    }

    void OnBuy(Dictionary<Item, int> itemsInCart, int totalPrice)
    {
        UpdatePriceInfo(0);
        CheckButtonsState(totalPrice);
    }

    void OnDisable()
    {
        CartController.OnAddInCart -= OnAddInCart;
        CartController.OnRemoveFromCart -= OnRemoveFromCart;
        CartController.OnBuy -= OnBuy;
    }
}
