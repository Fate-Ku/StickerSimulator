using UnityEngine;

public class LayerControllerTool : MonoBehaviour
{
    // SelectTool スクリプトを参照する
    public Select select;

    // order in layer を 1 増やす
    public void LayerUp()
    {
        if (select == null || select.targetRenderer == null)
        {
            Debug.Log("選択中のステッカーがありません");
            return;
        }

        select.targetRenderer.sortingOrder += 1;
        Debug.Log("Layer Up → " + select.targetRenderer.sortingOrder);
    }

    // order in layer を 1 減らす
    public void LayerDown()
    {
        if (select == null || select.targetRenderer == null)
        {
            Debug.Log("選択中のステッカーがありません");
            return;
        }

        int resultLayer = select.targetRenderer.sortingOrder - 1;

        if (resultLayer < 0)
        {
            select.targetRenderer.sortingOrder = 0;
        }
        else
        {
            select.targetRenderer.sortingOrder -= 1;
        }
        Debug.Log("Layer Down → " + select.targetRenderer.sortingOrder);
    }

}
