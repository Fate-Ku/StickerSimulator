using UnityEngine;

public class LayerUpTool : MonoBehaviour
{
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void LayerUp()
    {
        Debug.Log(gameObject.name + " BringToFront");
        LayerUpToolManager.CurrentFrontOrder++;
        sr.sortingOrder = LayerUpToolManager.CurrentFrontOrder;
    }

}