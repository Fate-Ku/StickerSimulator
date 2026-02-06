using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LayerControllerTool : MonoBehaviour
{
    public Select select;

    // ---------------------------------------------------------
    // ★ 一鍵で最前面へ
    // ---------------------------------------------------------
    public void LayerUpToFront()
    {
        if (select?.targetRenderer == null)
        {
            Debug.Log("選択中のステッカーがありません");
            return;
        }

        GameObject[] stickers = GameObject.FindGameObjectsWithTag("Sticker");

        int maxOrder = stickers
            .Select(s => s.GetComponent<SpriteRenderer>())
            .Where(r => r != null)
            .Max(r => r.sortingOrder);

        select.targetRenderer.sortingOrder = maxOrder + 1;

        Debug.Log("最前面へ移動 → " + select.targetRenderer.sortingOrder);
    }

    // ---------------------------------------------------------
    // ★ 一鍵で最背面へ
    // ---------------------------------------------------------
    public void LayerDownToBack()
    {
        if (select?.targetRenderer == null)
        {
            Debug.Log("選択中のステッカーがありません");
            return;
        }

        GameObject[] stickers = GameObject.FindGameObjectsWithTag("Sticker");

        int minOrder = stickers
            .Select(s => s.GetComponent<SpriteRenderer>())
            .Where(r => r != null)
            .Min(r => r.sortingOrder);

        select.targetRenderer.sortingOrder = minOrder - 1;

        Debug.Log("最背面へ移動 → " + select.targetRenderer.sortingOrder);
    }

    //重なっているステッカーの中で「1つ前へ」移動
    public void MoveOneLayerUpAmongOverlaps_Collider()
    {
        if (select?.targetRenderer == null) return;

        SpriteRenderer target = select.targetRenderer;

        // Collider2D を使って重なり取得
        List<SpriteRenderer> overlaps = GetOverlappingStickersByCollider(target);

        if (overlaps.Count == 0)
        {
            Debug.Log("重なっているステッカーはありません");
            return;
        }

        // 自分も含める
        overlaps.Add(target);

        // sortingOrder 昇順に並べ替え
        overlaps = overlaps.OrderBy(r => r.sortingOrder).ToList();

        int index = overlaps.IndexOf(target);

        // すでに最前面
        if (index == overlaps.Count - 1)
        {
            // 同じ order があれば調整
            SortingOrderPlus(target);
            Debug.Log("重なりグループ内で既に最前面です");
            return;
        }

        // 1つ上と入れ替え
        SpriteRenderer above = overlaps[index + 1];

        int temp = target.sortingOrder;
        target.sortingOrder = above.sortingOrder;
        above.sortingOrder = temp;

        // 同じ order があれば調整
        SortingOrderPlus(target);

        Debug.Log("重なりグループ内で1つ前へ移動（Collider 判定）");
    }

    //重なりグループ内で 1 つ後ろへ移動
    public void MoveOneLayerDownAmongOverlaps_Collider()
    {
        if (select?.targetRenderer == null) return;

        SpriteRenderer target = select.targetRenderer;

        // Collider2D を使って重なり取得
        List<SpriteRenderer> overlaps = GetOverlappingStickersByCollider(target);

        if (overlaps.Count == 0)
        {
            Debug.Log("重なっているステッカーはありません");
            return;
        }

        // 自分も含める
        overlaps.Add(target);

        // sortingOrder 昇順に並べ替え
        overlaps = overlaps.OrderBy(r => r.sortingOrder).ToList();

        int index = overlaps.IndexOf(target);

        // すでに最後面
        if (index == 0)
        {
            // 同じ order があれば調整
            SortingOrderMinus(target);
            Debug.Log("重なりグループ内で既に最後面です");
            return;
        }

        // 1つ下と入れ替え
        SpriteRenderer below = overlaps[index - 1];

        int temp = target.sortingOrder;
        target.sortingOrder = below.sortingOrder;
        below.sortingOrder = temp;

        // 同じ order があれば調整
        SortingOrderMinus(target);

        Debug.Log("重なりグループ内で1つ後ろへ移動（Collider 判定）");
    }

    //重なっているステッカーを取得する関数
    public List<SpriteRenderer> GetOverlappingStickersByCollider(SpriteRenderer target)
    {
        List<SpriteRenderer> list = new List<SpriteRenderer>();

        Collider2D col = target.GetComponent<Collider2D>();
        if (col == null) return list;

        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;

        Collider2D[] results = new Collider2D[20];
        int count = Physics2D.OverlapCollider(col, filter, results);

        for (int i = 0; i < count; i++)
        {
            Collider2D hit = results[i];
            if (hit == null) continue;

            // 自分自身は除外
            if (hit.gameObject == target.gameObject) continue;

            var r = hit.GetComponent<SpriteRenderer>();
            if (r != null)
                list.Add(r);
        }

        return list;
    }

    // sortingOrder が同じなら +1
    private void SortingOrderPlus(SpriteRenderer target)
    {
        GameObject[] stickers = GameObject.FindGameObjectsWithTag("Sticker");

        foreach (var s in stickers)
        {
            var r = s.GetComponent<SpriteRenderer>();
            if (r == null || r == target) continue;

            // ★ 同じ sortingOrder を持つステッカーがいたら
            if (r.sortingOrder == target.sortingOrder)
            {
                target.sortingOrder += 1; // ★ 選択中だけ +1
            }
        }
    }

    // sortingOrder が同じなら -1
    private void SortingOrderMinus(SpriteRenderer target)
    {
        GameObject[] stickers = GameObject.FindGameObjectsWithTag("Sticker");

        foreach (var s in stickers)
        {
            var r = s.GetComponent<SpriteRenderer>();
            if (r == null || r == target) continue;

            // ★ 同じ sortingOrder を持つステッカーがいたら
            if (r.sortingOrder == target.sortingOrder)
            {
                target.sortingOrder -= 1; // ★ 選択中だけ +1
            }
        }
    }
}