using UnityEngine;

public class TransformVariantButton : MonoBehaviour
{
    public delegate void ClickAction(TransformVariant transformVariant);
    public static event ClickAction OnClicked;

    public TransformVariant transformVariant;

    public void Click()
    {
        if (OnClicked != null)
            OnClicked(transformVariant);
    }
}
