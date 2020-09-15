using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public delegate void UseAction(Item item);
    public static event UseAction OnUsed;

    public Button button;

    [SerializeField] Image itemIcon;
    [SerializeField] Text countText;

    Item item;
    Vector3 startPosition;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Init(Item newItem)
    {
        itemIcon.sprite = newItem.icon;
        item = newItem;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (OnUsed != null && Vector2.Distance(startPosition, transform.position) > 10)
            OnUsed(item);

        transform.position = startPosition;
    }

    public void UpdateCount(int count)
    {
        countText.text = count.ToString();
    }
}
