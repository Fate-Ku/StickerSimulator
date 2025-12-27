using UnityEngine;

public class LayerDownTool : MonoBehaviour
{
    private SpriteRenderer myRenderer;

     void Awake()
    {
        myRenderer = GetComponent<SpriteRenderer>();
    }
    public void LayerDown()
    {
        // 同じPrefab（同じスクリプトが付いているもの）を取得
        LayerDownTool[] group =
            FindObjectsOfType<LayerDownTool>();

        int minOrder = myRenderer.sortingOrder;

        foreach (var obj in group)
        {
            minOrder = Mathf.Min(minOrder, obj.myRenderer.sortingOrder);
        }

        // 一番小さい値よりさらに背面へ
        myRenderer.sortingOrder = minOrder - 1;
    }
    
}
