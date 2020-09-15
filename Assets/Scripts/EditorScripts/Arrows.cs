using UnityEngine;

public class Arrows : MonoBehaviour
{
    public delegate void MoveAction(Vector3 position);
    public static event MoveAction OnMove;

    public delegate void RotateAction(Vector3 rotation);
    public static event RotateAction OnRotate;

    public delegate void ScaleAction(Vector3 scale);
    public static event ScaleAction OnScale;

    [SerializeField] GameObject arrowsVisual;
    [SerializeField] GameObject arrows;
    [SerializeField] GameObject rotators;
    [SerializeField] GameObject scalers;

    bool used = false;

    TransformVariant transformVariant = TransformVariant.position;

    float prevValue;

    Transform targetTransform;

    void OnEnable()
    {
        Arrow.OnUse += Use;
        Arrow.OnStopUse += StopUse;
        TransformVariantButton.OnClicked += ChangeTransformVariant;
        EditorController.OnSelectedObject += ShowVisuals;
        EditorController.OnDeleteObject += HideVisual;
    }

    void StopUse()
    {
        used = false;
    }

    void ShowVisuals(Item item)
    {
        transform.position = item.transform.position;
        transform.rotation = item.transform.rotation;

        arrowsVisual.SetActive(true);

        targetTransform = item.transform;
    }

    void HideVisual()
    {
        targetTransform = null;
        arrowsVisual.SetActive(false);
    }

    void ChangeTransformVariant(TransformVariant newTransformVariant)
    {
        transformVariant = newTransformVariant;

        arrows.SetActive(transformVariant == TransformVariant.position);
        rotators.SetActive(transformVariant == TransformVariant.rotation);
        scalers.SetActive(transformVariant == TransformVariant.scale);

        used = false;
    }

    void Use(Axis axis, float value)
    {
        if (transformVariant == TransformVariant.position)
            Move(axis, value);

        if (transformVariant == TransformVariant.rotation)
            Rotate(axis, value);

        if (transformVariant == TransformVariant.scale)
            Scale(axis, value);

        prevValue = value;

        used = true;
    }

    void Move(Axis axis, float value)
    {
        Vector3 pos = Vector3.zero;

        if (axis == Axis.X)
            pos = new Vector3(value, transform.position.y, transform.position.z);

        if (axis == Axis.Y)
            pos = new Vector3(transform.position.x, value, transform.position.z);

        if (axis == Axis.Z)
            pos = new Vector3(transform.position.x, transform.position.y, value);

        transform.position = pos;

        if (OnMove != null)
            OnMove(pos);
    }

    void Rotate(Axis axis, float value)
    {
        if (used)
        {
            Vector3 rotate = Vector3.zero;

            if (axis == Axis.X)
                rotate = new Vector3(rotate.x - (prevValue - value) * 1000, rotate.y, rotate.z);

            if (axis == Axis.Y)
                rotate = new Vector3(rotate.x, rotate.y - (prevValue - value) * 1000, rotate.z);

            if (axis == Axis.Z)
                rotate = new Vector3(rotate.x, rotate.y, rotate.z - (prevValue - value) * 1000);

            if (OnRotate != null)
                OnRotate(rotate);
        }
    }

    void Scale(Axis axis, float value)
    {
        if (used)
        {
            Vector3 scale = Vector3.zero;

            if (axis == Axis.X)
                scale = new Vector3(scale.x - (prevValue - value), scale.y, scale.z);

            if (axis == Axis.Y)
                scale = new Vector3(scale.x, scale.y - (prevValue - value), scale.z);

            if (axis == Axis.Z)
                scale = new Vector3(scale.x, scale.y, scale.z - (prevValue - value));

            if (OnScale != null)
                OnScale(scale);
        }
    }

    void OnDisable()
    {
        Arrow.OnUse -= Move;
        Arrow.OnStopUse -= StopUse;
        TransformVariantButton.OnClicked -= ChangeTransformVariant;
        EditorController.OnSelectedObject -= ShowVisuals;
        EditorController.OnDeleteObject -= HideVisual;
    }
}
