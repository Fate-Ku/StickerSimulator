using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LayerUpTool : MonoBehaviour
{
    public string targetSortingLayer = "Default"; // 手前にしたいSorting Layer名
    public int orderInLayerOffset = 1; // 現在のOrder In Layerに加算する値

    public void LayerUp()
    {
        // 自身のRectTransformを取得し、描画順を最後に設定（一番手前）
        GetComponent<RectTransform>().SetAsLastSibling();


        // スプライトレンダラーを取得
        //SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        //if (spriteRenderer != null)
        //{
            // 現在のSorting LayerとOrder In Layerを取得し、手前にする設定を適用
            // （もしレイヤーが違うなら、レイヤー名も設定する必要がある）
            //spriteRenderer.sortingLayerName = targetSortingLayer;
            //spriteRenderer.sortingOrder = orderInLayerOffset;
            // または、すでに設定されているOrder in Layerに加算する場合:
            //spriteRenderer.sortingOrder += orderInLayerOffset;
        //}
    }

}
