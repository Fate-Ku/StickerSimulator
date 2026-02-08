//using UnityEngine;
//using TMPro;
//using System.Collections.Generic;

//public class StickerBookDetailBackup : MonoBehaviour
//{
//    [Header("Data")]
//    public StickerData[] allStickers;     // all stickers
//    [Header("UI")]
//    public UI_Sticker stickerPrefab;       // StickerCard prefab
//    public Transform gridParent;          // Content (GridLayout) transform
//    public TMP_Text page;             // Page X/Y
//    public TMP_Text categoryPercent;  // category percent
//    public TMP_Text totalPercent;     // total percent

//    private List<StickerData> currentCategoryList = new List<StickerData>();
//    private int currentPage = 0;
//    private const int stickersPerPage = 9; // 1 page have 9 stickers

//    private void Start()
//    {
//        UpdateTotalPercent();
//        ShowCategory("Shape");
//    }

//    // call this when a category button clicked
//    public void ShowCategory(string category)
//    {
//        currentPage = 0;
//        currentCategoryList.Clear();

//        foreach (var s in allStickers)
//        {
//            if (s.category == category)
//                currentCategoryList.Add(s);
//        }

//        RefreshPage();
//        UpdateCategoryPercent();
//    }

//    public void NextPage()
//    {
//        int maxPage = Mathf.CeilToInt(currentCategoryList.Count / (float)stickersPerPage) - 1;
//        if (currentPage < maxPage)
//        {
//            currentPage++;
//            RefreshPage();
//        }
//    }

//    public void PrevPage()
//    {
//        if (currentPage > 0)
//        {
//            currentPage--;
//            RefreshPage();
//        }
//    }

//    private void RefreshPage()
//    {
//        // clean all UI
//        for (int i = gridParent.childCount - 1; i >= 0; i--)
//            Destroy(gridParent.GetChild(i).gameObject);

//        // start from which sticker
//        int start = currentPage * stickersPerPage;
//        // create sticker
//        for (int i = start; i < start + stickersPerPage; i++)
//        {
//            if (i >= currentCategoryList.Count) break;
//            var ui = Instantiate(stickerPrefab, gridParent);
//            ui.Setup(currentCategoryList[i]);
//        }

//        UpdatePageText();
//    }

//    private void UpdatePageText()
//    {
//        int maxPage = Mathf.CeilToInt(currentCategoryList.Count / (float)stickersPerPage);
//        if (maxPage == 0) maxPage = 1;
//        page.text = $"P {currentPage + 1} / {maxPage}";
//    }

//    private void UpdateCategoryPercent()
//    {
//        if (currentCategoryList.Count == 0)
//        {
//            categoryPercent.text = "種類完成度:0%";
//            return;
//        }

//        int unlocked = 0;
//        foreach (var s in currentCategoryList)
//            if (s.isUnlocked) unlocked++;

//        float percent = unlocked / (float)currentCategoryList.Count * 100f;
//        categoryPercent.text = $"種類完成度:{percent:F1}%";
//    }

//    private void UpdateTotalPercent()
//    {
//        if (allStickers == null || allStickers.Length == 0)
//        {
//            totalPercent.text = "完成度:0%";
//            return;
//        }

//        int unlocked = 0;
//        foreach (var s in allStickers)
//            if (s != null && s.isUnlocked) unlocked++;

//        float percent = unlocked / (float)allStickers.Length * 100f;
//        totalPercent.text = $"完成度:{percent:F1}%";
//    }

//    public void RefreshAllUI()
//    {
//        UpdateTotalPercent();
//        UpdateCategoryPercent();
//        RefreshPage();
//    }
//}
