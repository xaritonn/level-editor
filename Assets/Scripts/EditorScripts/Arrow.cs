using UnityEngine;

public class Arrow : MonoBehaviour
{
    public delegate void UseAction(Axis axis, float value);
    public static event UseAction OnUse;

    public delegate void StopUseAction();
    public static event StopUseAction OnStopUse;

    [SerializeField] Axis axis;

    Vector3 offset;
    float zCoord;
    float arrowOffset;

    void Awake()
    {
        arrowOffset = Vector3.Distance(transform.parent.position, transform.position);
    }

    void OnMouseDown()
    {
        zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        offset = gameObject.transform.position - GetMouseAsWorldPoint();
    }

    Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        float newPosValue = 0;

        if (axis == Axis.X)
            newPosValue = (GetMouseAsWorldPoint() + offset).x - arrowOffset;

        if (axis == Axis.Y)
            newPosValue = (GetMouseAsWorldPoint() + offset).y - arrowOffset;

        if (axis == Axis.Z)
            newPosValue = (GetMouseAsWorldPoint() + offset).z - arrowOffset;

        if (OnUse != null)
            OnUse(axis, newPosValue);
    }

    void OnMouseUp()
    {
        if (OnStopUse != null)
            OnStopUse();
    }
}
