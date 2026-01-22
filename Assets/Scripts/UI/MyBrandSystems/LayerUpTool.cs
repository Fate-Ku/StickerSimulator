using UnityEngine;
using UnityEngine.UIElements;

public class LayerUpTool : MonoBehaviour
{
    [SerializeField] private Select select;

    static void LayerUp()
    {
        if (select == null) return;
        if (select.targetObject == null) return;

        Transform target = select.targetObject;

        // Rendererコンポーネント（SpriteRenderer, TilemapRendererなど）を取得
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            // Order in Layer の値を1増やす
            renderer.sortingOrder += 1;

            Debug.Log("Layer Up");

        }

        else
        {
            Debug.LogWarning("SpriteRenderer not found");
        }

    }

}
