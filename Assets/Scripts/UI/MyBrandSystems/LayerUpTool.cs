using UnityEngine;

public class LayerUpTool : MonoBehaviour
{
    [SerializeField] private Select select;

    public void LayerUp()
    {
        Debug.Log(gameObject.name + " BringToFront");
        LayerUpToolManager.CurrentFrontOrder++;
        sr.sortingOrder = LayerUpToolManager.CurrentFrontOrder;
    }

}