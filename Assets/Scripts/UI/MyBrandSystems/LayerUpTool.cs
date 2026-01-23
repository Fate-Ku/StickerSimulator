using UnityEngine;
using UnityEngine.UIElements;

public class LayerUpTool : MonoBehaviour
{
    [SerializeField] private Select select;

    static void LayerUp()
    {
        

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
