/*
using UnityEngine;
using UnityEditor;

public class LayerUpTool : MonoBehaviour
{
    static void LayerUp()
    {
        // 選択中のすべてのGameObjectを対象にする
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0) return;

        foreach (GameObject go in selectedObjects)
        {
            // Rendererコンポーネント（SpriteRenderer, TilemapRendererなど）を取得
            Renderer renderer = go.GetComponent<Renderer>();

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

}
*/
