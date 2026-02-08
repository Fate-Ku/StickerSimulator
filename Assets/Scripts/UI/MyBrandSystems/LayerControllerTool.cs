using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LayerControllerTool : MonoBehaviour
{
    public Select select;

    // ---------------------------------------------------------
    // ★ 一鍵で最前面へ（親子対応）
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
            .SelectMany(s => s.GetComponentsInChildren<SpriteRenderer>())
            .Max(r => r.sortingOrder);

        int newOrder = maxOrder + 1;

        select.targetRenderer.sortingOrder = newOrder;

        // ★ 親子の sortingOrder を統一
        NormalizeChildSortingOrderUp(select.targetRenderer.transform.root, newOrder);

        Debug.Log("最前面へ移動 → " + newOrder);
    }

    // ---------------------------------------------------------
    // ★ 一鍵で最背面へ（親子対応）
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
            .SelectMany(s => s.GetComponentsInChildren<SpriteRenderer>())
            .Min(r => r.sortingOrder);

        int newOrder = minOrder - 1;

        select.targetRenderer.sortingOrder = newOrder;

        // ★ 親子の sortingOrder を統一
        NormalizeChildSortingOrderDown(select.targetRenderer.transform.root, newOrder);

        Debug.Log("最背面へ移動 → " + newOrder);
    }

    // ---------------------------------------------------------
    // ★ 重なりグループ内で「1つ前へ」（親子対応）
    // ---------------------------------------------------------
    public void MoveOneLayerUpAmongOverlaps_Collider()
    {
        if (select?.targetRenderer == null) return;

        SpriteRenderer target = select.targetRenderer;

        List<SpriteRenderer> overlaps = GetOverlappingStickersByCollider(target);

        if (overlaps.Count == 0)
        {
            Debug.Log("重なっているステッカーはありません");
            return;
        }

        overlaps.Add(target);
        overlaps = overlaps.OrderBy(r => r.sortingOrder).ToList();

        int index = overlaps.IndexOf(target);

        if (index == overlaps.Count - 1)
        {
            SortingOrderPlus(target);

            // ★ 親子同期
            NormalizeChildSortingOrderUp(target.transform.root, target.sortingOrder);

            Debug.Log("重なりグループ内で既に最前面です");
            return;
        }

        SpriteRenderer above = overlaps[index + 1];

        int temp = target.sortingOrder;
        target.sortingOrder = above.sortingOrder;
        above.sortingOrder = temp;

        //SortingOrderPlus(target);

        // ★ 親子同期
        NormalizeChildSortingOrderUp(target.transform.root, target.sortingOrder);

        Debug.Log("重なりグループ内で1つ前へ移動（Collider 判定）");
    }

    // ---------------------------------------------------------
    // ★ 重なりグループ内で「1つ後ろへ」（親子対応）
    // ---------------------------------------------------------
    public void MoveOneLayerDownAmongOverlaps_Collider()
    {
        if (select?.targetRenderer == null) return;

        SpriteRenderer target = select.targetRenderer;

        List<SpriteRenderer> overlaps = GetOverlappingStickersByCollider(target);

        if (overlaps.Count == 0)
        {
            Debug.Log("重なっているステッカーはありません");
            return;
        }

        overlaps.Add(target);
        overlaps = overlaps.OrderBy(r => r.sortingOrder).ToList();

        int index = overlaps.IndexOf(target);

        if (index == 0)
        {
            //SortingOrderMinus(target);

            // ★ 親子同期
            //NormalizeChildSortingOrderDown(target.transform.root, target.sortingOrder);

            Debug.Log("重なりグループ内で既に最後面です");
            return;
        }

        SpriteRenderer below = overlaps[index - 1];

        int temp = target.sortingOrder;
        target.sortingOrder = below.sortingOrder;
        below.sortingOrder = temp;

        //SortingOrderMinus(target);

        // ★ 親子同期
        NormalizeChildSortingOrderDown(target.transform.root, target.sortingOrder);

        Debug.Log("重なりグループ内で1つ後ろへ移動（Collider 判定）");
    }

    // ---------------------------------------------------------
    // ★ Collider2D を使って重なり取得（親子対応）
    // ---------------------------------------------------------
    public List<SpriteRenderer> GetOverlappingStickersByCollider(SpriteRenderer target)
    {
        List<SpriteRenderer> list = new List<SpriteRenderer>();

        Collider2D col = target.GetComponent<Collider2D>();
        if (col == null) col = target.GetComponentInParent<Collider2D>();
        if (col == null) col = target.GetComponentInChildren<Collider2D>();

        if (col == null) return list;

        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;

        Collider2D[] results = new Collider2D[50];
        int count = Physics2D.OverlapCollider(col, filter, results);

        for (int i = 0; i < count; i++)
        {
            Collider2D hit = results[i];
            if (hit == null) continue;

            if (hit.gameObject == target.gameObject) continue;

            var r = hit.GetComponentInChildren<SpriteRenderer>();
            if (r != null)
                list.Add(r);
        }

        return list;
    }

    // ---------------------------------------------------------
    // ★ sortingOrder が同じなら +1（親子対応）
    // ---------------------------------------------------------
    private void SortingOrderPlus(SpriteRenderer target)
    {
        GameObject[] stickers = GameObject.FindGameObjectsWithTag("Sticker");

        foreach (var s in stickers)
        {
            var renderers = s.GetComponentsInChildren<SpriteRenderer>();

            foreach (var r in renderers)
            {
                if (r == target) continue;

                if (r.sortingOrder == target.sortingOrder)
                    target.sortingOrder += 1;
            }
        }
    }

    // ---------------------------------------------------------
    // ★ sortingOrder が同じなら -1（親子対応）
    // ---------------------------------------------------------
    private void SortingOrderMinus(SpriteRenderer target)
    {
        GameObject[] stickers = GameObject.FindGameObjectsWithTag("Sticker");

        foreach (var s in stickers)
        {
            var renderers = s.GetComponentsInChildren<SpriteRenderer>();

            foreach (var r in renderers)
            {
                if (r == target) continue;

                if (r.sortingOrder == target.sortingOrder)
                    target.sortingOrder -= 1;
            }
        }
    }

    // ---------------------------------------------------------
    // ★ 子オブジェクトの sortingOrder を親基準で統一
    // ---------------------------------------------------------
    public void NormalizeChildSortingOrderUp(Transform stickerRoot, int newBaseOrder)
    {
        SpriteRenderer[] renderers = stickerRoot.GetComponentsInChildren<SpriteRenderer>();

        if (renderers.Length == 0) return;

        int minOrder = renderers.Min(r => r.sortingOrder);

        foreach (var r in renderers)
        {
            int offset = r.sortingOrder - minOrder;
            r.sortingOrder = newBaseOrder + offset;
        }
    }

    public void NormalizeChildSortingOrderDown(Transform stickerRoot, int newBaseOrder)
    {
        SpriteRenderer[] renderers = stickerRoot.GetComponentsInChildren<SpriteRenderer>();

        if (renderers.Length == 0) return;

        int maxOrder = renderers.Max(r => r.sortingOrder);

        foreach (var r in renderers)
        {
            int offset = r.sortingOrder - maxOrder;
            r.sortingOrder = newBaseOrder - offset;
        }
    }

}