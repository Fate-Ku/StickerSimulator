using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LayerUpTool : MonoBehaviour
{
    // 最前面に移動させたいGameObjectをインスペクターから設定するための変数 
    public GameObject objectToBringToFront;
    SpriteRenderer sprRenderer;

    // このメソッドをボタンのOnClickイベントに紐づける
    public void MoveObjectToFront()
    {
        //新しいレイヤー名(事前に設定)
        sprRenderer.sortingLayerName = "ForeGround";
        //レイヤー内順序（数字が大きいほど前面）
        sprRenderer.sortingOrder = 2;
    }

}
