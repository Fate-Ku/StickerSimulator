using UnityEngine;

public class LayerUpTool : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void LayerUp()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder += 1;
            Debug.Log("Layer Up! Current Order: " + spriteRenderer.sortingOrder);
        }
        else
        {
            Debug.LogWarning("SpriteRenderer not found");
        }
    }

}