using UnityEngine;
using UnityEngine.UIElements;

public class LayerUpTool : MonoBehaviour
{
    [SerializeField] private Select select;

    public void LayerUp()
    {
        if (select.targetObject == null) { return; }

        Renderer renderer = select.targetObject.GetComponent<SpriteRenderer>();
        if (renderer == null) return;

        SpriteRenderer[] allSprites = FindObjectsOfType<SpriteRenderer>();

        int maxOrder = renderer.sortingOrder;

        foreach (var sr in allSprites)
        {
            //同じSorting Layerの中だけで、
            if (sr.sortingLayerID == renderer.sortingLayerID)
            {
                if (sr.sortingOrder > maxOrder)
                maxOrder = sr.sortingOrder;
            }
        }

        renderer.sortingOrder = maxOrder + 1;
        // Rendererコンポーネント（SpriteRenderer, TilemapRendererなど）を取得
        //Renderer renderer = GetComponent<Renderer>();

        //if (GetComponent<Renderer>() != null)
        //{
            // Order in Layer の値を1増やす
            //GetComponent<Renderer>().sortingOrder += 1;

            //Debug.Log("Layer Up");

        //}

        //else
        //{
            //Debug.LogWarning("SpriteRenderer not found");
        //}

    }

}
