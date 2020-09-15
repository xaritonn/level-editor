using System;
using UnityEngine;

public class EditorController : MonoBehaviour
{
    public delegate void UpdateTransformAction(Vector3 newVector);
    public static event UpdateTransformAction OnUpdateTransform;

    public delegate void DeleteObjectAction();
    public static event DeleteObjectAction OnDeleteObject;

    public delegate void SelectedAction(Item item);
    public static event SelectedAction OnSelectedObject;

    Camera editorCamera;

    GameObject selectedItem;

    TransformVariant currentTransformVariant = TransformVariant.position;

    void OnEnable()
    {
        TransformVariantButton.OnClicked += SwitchTransormVariant;
        AxisInput.OnChange += SetTransform;
        Arrows.OnMove += MoveObject;
        Arrows.OnRotate += RotateObject;
        Arrows.OnScale += ScaleObject;
    }

    void MoveObject(Vector3 position)
    {
        selectedItem.transform.position = position;

        if (OnUpdateTransform != null)
            OnUpdateTransform(selectedItem.transform.position);
    }

    void RotateObject(Vector3 rotationInc)
    {
        selectedItem.transform.Rotate(rotationInc, Space.World);

        if (OnUpdateTransform != null)
            OnUpdateTransform(selectedItem.transform.eulerAngles);
    }

    void ScaleObject(Vector3 scaleInc)
    {
        Vector3 scale = selectedItem.transform.localScale + scaleInc;

        selectedItem.transform.localScale = scale;

        if (OnUpdateTransform != null)
            OnUpdateTransform(selectedItem.transform.localScale);
    }

    void Start()
    {
        editorCamera = FindObjectOfType<Camera>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = editorCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Item item = hit.collider.GetComponent<Item>();

                if (item)
                {
                    Select(item);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            Delete();
        }
    }

    void Select(Item item)
    {
        if (OnSelectedObject != null)
            OnSelectedObject(item);

        if(selectedItem)
            selectedItem.GetComponent<Collider>().enabled = true;

        selectedItem = item.gameObject;
        selectedItem.GetComponent<Collider>().enabled = false;

        UpdateTransformValues();
    }

    void Delete()
    {
        if (selectedItem)
        {
            Destroy(selectedItem.gameObject);
            selectedItem = null;
            UpdateTransformValues();

            if (OnDeleteObject != null)
                OnDeleteObject();
        }
    }

    void SetTransform(Axis axis, float value)
    {
        if (currentTransformVariant == TransformVariant.position)
            SetPosition(axis, value);

        if (currentTransformVariant == TransformVariant.rotation)
            SetRotation(axis, value);

        if (currentTransformVariant == TransformVariant.scale)
            SetScale(axis, value);
    }

    void SetPosition(Axis axis, float newPos)
    {
        Vector3 oldPos = selectedItem.transform.position;

        if (axis == Axis.X)
            selectedItem.transform.position = new Vector3 (newPos, oldPos.y, oldPos.z);

        if (axis == Axis.Y)
            selectedItem.transform.position = new Vector3(oldPos.x, newPos, oldPos.z);

        if (axis == Axis.Z)
            selectedItem.transform.position = new Vector3(oldPos.x, oldPos.y, newPos);
    }

    void SetRotation(Axis axis, float newRot)
    {
        Vector3 oldRot = selectedItem.transform.eulerAngles;

        if (axis == Axis.X)
            selectedItem.transform.eulerAngles = new Vector3(newRot, oldRot.y, oldRot.z);

        if (axis == Axis.Y)
            selectedItem.transform.eulerAngles = new Vector3(oldRot.x, newRot, oldRot.z);

        if (axis == Axis.Z)
            selectedItem.transform.eulerAngles = new Vector3(oldRot.x, oldRot.y, newRot);
    }

    void SetScale(Axis axis, float newScale)
    {
        Vector3 oldScale = selectedItem.transform.localScale;

        if (axis == Axis.X)
            selectedItem.transform.localScale = new Vector3(newScale, oldScale.y, oldScale.z);

        if (axis == Axis.Y)
            selectedItem.transform.localScale = new Vector3(oldScale.x, newScale, oldScale.z);

        if (axis == Axis.Z)
            selectedItem.transform.localScale = new Vector3(oldScale.x, oldScale.y, newScale);
    }

    public void SwitchTransormVariant(TransformVariant newTransformVariant)
    {
        currentTransformVariant = newTransformVariant;
        UpdateTransformValues();
    }

    void UpdateTransformValues()
    {
        Vector3 currentTransformVector = Vector3.zero;

        if (selectedItem)
        {
            if (currentTransformVariant == TransformVariant.position)
            {
                currentTransformVector = selectedItem.transform.position;
            }

            if (currentTransformVariant == TransformVariant.rotation)
            {
                currentTransformVector = selectedItem.transform.eulerAngles;
            }

            if (currentTransformVariant == TransformVariant.scale)
            {
                currentTransformVector = selectedItem.transform.localScale;
            }
        }

        if (OnUpdateTransform != null)
            OnUpdateTransform(currentTransformVector);
    }

    void OnDisable()
    {
        TransformVariantButton.OnClicked -= SwitchTransormVariant;
        AxisInput.OnChange -= SetTransform;
        Arrows.OnMove -= MoveObject;
        Arrows.OnRotate -= RotateObject;
        Arrows.OnScale -= ScaleObject;
    }
}
