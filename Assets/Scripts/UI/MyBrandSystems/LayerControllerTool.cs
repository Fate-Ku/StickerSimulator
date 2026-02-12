using UnityEngine;
using UnityEngine.Rendering; // 必須引用，才能控制 SortingGroup
using System.Collections.Generic;

public class LayerControllerTool : MonoBehaviour
{
    // 在 Inspector 面板將掛有 SortingGroup 的父物件拖進來
    public List<SortingGroup> layers = new List<SortingGroup>();
    public int selectedIndex = -1; // 目前選中的圖層索引


    // 在 LayerControllerTool.cs 中加入
    public void RegisterNewLayer(SortingGroup newGroup)
    {
        if (!newGroup) return; 


        if (!layers.Contains(newGroup))
        {
            layers.Add(newGroup);       // 加入清單
            newGroup.sortAtRoot = true; // 確保排序正確
            ApplyOrder();               // 重新整理所有人的 Order

            // 自動選中這個新生成的物件
            selectedIndex = layers.Count - 1;
            Debug.Log($"新圖層 {newGroup.name} 已註冊並選中");
        }
    }

    public void SetSelectedIndexFromSticker(SortingGroup selectedGroup)
    {
        if (layers.Contains(selectedGroup))
        {
            selectedIndex = layers.IndexOf(selectedGroup);
            Debug.Log($"圖層工具已同步：{selectedGroup.name} (Index: {selectedIndex})");
        }
    }

    /// <summary>
    /// 往上移動，直到跳過第一個與自己重疊的圖層
    /// </summary>
    public void MoveUpOverlapping()
    {

        if (selectedIndex < 0 || selectedIndex >= layers.Count - 1) return;

        Collider2D myCollider = layers[selectedIndex].GetComponentInChildren<Collider2D>();

        int targetIndex = -1;

        // 往上搜尋 (index + 1 開始)
        for (int i = selectedIndex + 1; i < layers.Count; i++)
        {
            Collider2D otherCollider = layers[i].GetComponentInChildren<Collider2D>();
            if (otherCollider != null && myCollider.bounds.Intersects(otherCollider.bounds))
            {
                targetIndex = i; // 找到上方第一個重疊的
                break;
            }
        }

        if (targetIndex != -1)
        {
            // 跳過該重疊物件：將自己插入到目標之後
            SortingGroup temp = layers[selectedIndex];
            layers.RemoveAt(selectedIndex);
            layers.Insert(targetIndex, temp);

            selectedIndex = targetIndex; // 更新索引
            ApplyOrder();
        }
        else
        {
            // 如果上方沒有重疊的，就直接移到最頂層，或者維持現狀
            // 這裡我們選擇直接移到最頂
            MoveToTop(selectedIndex);
        }
    }

    /// <summary>
    /// 往下移動，直到跳過第一個與自己重疊的圖層
    /// </summary>
    public void MoveDownOverlapping()
    {

        if (selectedIndex <= 0 || selectedIndex >= layers.Count) return;

        Collider2D myCollider = layers[selectedIndex].GetComponentInChildren<Collider2D>();

        int targetIndex = -1;

        // 往下搜尋 (index - 1 開始)
        for (int i = selectedIndex - 1; i >= 0; i--)
        {
            Collider2D otherCollider = layers[i].GetComponentInChildren<Collider2D>();
            if (otherCollider != null && myCollider.bounds.Intersects(otherCollider.bounds))
            {
                targetIndex = i;
                break;
            }
        }

        if (targetIndex != -1)
        {
            SortingGroup temp = layers[selectedIndex];
            layers.RemoveAt(selectedIndex);
            layers.Insert(targetIndex, temp);

            selectedIndex = targetIndex;
            ApplyOrder();
        }
        else
        {
            MoveToBottom(selectedIndex);
        }
    }

    /// <summary>
    /// 直接移至最頂層（畫面最前方）
    /// </summary>
    public void MoveToTop(int index)
    {
        if (index < 0 || index >= layers.Count - 1) return;

        SortingGroup target = layers[index];

        // 1. 從清單中移除
        layers.RemoveAt(index);

        // 2. 加入到清單最後面（List 最後面代表 SortingOrder 最大）
        layers.Add(target);

        // 更新當前選中索引
        selectedIndex = layers.Count - 1;

        // 刷新所有物件的 SortingOrder
        ApplyOrder();
        Debug.Log($"{target.name} 已移至最頂層");
    }

    /// <summary>
    /// 直接移至最底層（畫面最後方）
    /// </summary>
    public void MoveToBottom(int index)
    {
        if (index <= 0 || index >= layers.Count) return;

        SortingGroup target = layers[index];

        // 1. 從清單中移除
        layers.RemoveAt(index);

        // 2. 插入到清單最前面（Index 0 代表 SortingOrder 為 0）
        layers.Insert(0, target);

        // 更新當前選中索引
        selectedIndex = 0;

        // 刷新所有物件的 SortingOrder
        ApplyOrder();
        Debug.Log($"{target.name} 已移至最底層");
    }

    // 依照 List 順序重新賦值給 Sorting Group
    public void ApplyOrder()
    {
        //Destroy 済みのものを削除
        layers.RemoveAll(layer => layer == null);

        for (int i = 0; i < layers.Count; i++)
        {
            if (!layers[i]) continue; // 念のため保険

            layers[i].sortingOrder = i;
        }
    }

    public void RegisterLoadLayer(SortingGroup newGroup)
    {
        if (!layers.Contains(newGroup))
        {
            layers.Add(newGroup);       // 加入清單
            newGroup.sortAtRoot = true; // 確保排序正確

            Debug.Log("註冊到：" + gameObject.name, gameObject);
            Debug.Log($"[LayerTool] 已加入 {newGroup.name}。當前 List 總數: {layers.Count}");
        }
    }
}