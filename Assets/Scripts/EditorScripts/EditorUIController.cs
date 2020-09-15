using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorUIController : MonoBehaviour
{
    [SerializeField] GameObject button;
    [SerializeField] Transform buttonPanel;

    Dictionary<Item, ItemButton> buttons = new Dictionary<Item, ItemButton>();
    Inventory inventory;

    [SerializeField] InputField xInputField;
    [SerializeField] InputField yInputField;
    [SerializeField] InputField zInputField;

    void OnEnable()
    {
        Inventory.OnChange += UpdateButtons;
        EditorController.OnSelectedObject += ActivateTransformFields;
        EditorController.OnUpdateTransform += UpdateTransformFields;
        EditorController.OnDeleteObject += DeactivateTransformFields;
    }

    void ActivateTransformFields(Item item)
    {
        xInputField.interactable = true;
        yInputField.interactable = true;
        zInputField.interactable = true;
    }

    void UpdateTransformFields(Vector3 newVector)
    {
        xInputField.text = newVector.x.ToString();
        yInputField.text = newVector.y.ToString();
        zInputField.text = newVector.z.ToString();
    }

    void DeactivateTransformFields()
    {
        xInputField.interactable = false;
        yInputField.interactable = false;
        zInputField.interactable = false;

        xInputField.text = "0";
        yInputField.text = "0";
        zInputField.text = "0";
    }

    void UpdateButtons(Dictionary<Item, int> inventory)
    {
        foreach (KeyValuePair<Item, int> item in inventory)
        {
            buttons[item.Key].UpdateCount(item.Value);
        }
    }

    void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        AddButtons();
    }

    void AddButtons()
    {
        foreach (Item item in inventory.items)
        {
            GameObject newButton = Instantiate(button, buttonPanel);

            buttons.Add(item, newButton.GetComponent<ItemButton>());
            buttons[item].Init(item);
        }
    }

    void OnDisable()
    {
        Inventory.OnChange -= UpdateButtons;
        EditorController.OnSelectedObject -= ActivateTransformFields;
        EditorController.OnUpdateTransform -= UpdateTransformFields;
        EditorController.OnDeleteObject -= DeactivateTransformFields;
    }
}
