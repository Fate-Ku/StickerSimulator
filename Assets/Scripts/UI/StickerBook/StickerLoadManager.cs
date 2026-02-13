using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class StickerLoadManager : MonoBehaviour
{
    [Header("Popup UI")]
    public GameObject popupPanel;

    [Header("UI")]
    public RawImage previewImage;     // 画像を表示する RawImage
    public TMP_Text pageText;         // P X / Y
    public TMP_Text fileNameText;     // 画像名（拡張子なし）

    [Range(0.001f, 2f)]
    public float displayScale = 0.03f;   // 30% 表示

    private List<string> savedImagePaths = new List<string>();
    private int currentPage = 0;

    public GameObject selectStickerPrefab;   // ← SelectSticker の Prefab をアサイン

    public LayerControllerTool globalLayerManager;

    private void Start()
    {
        popupPanel.SetActive(false);
    }

    public void LoadMyBrandStickers()
    {
        LoadAllSavedStickers();
        ShowPage(0);
    }

    // ----------------------------------------
    // ① 保存された PNG を全部読み込む
    // ----------------------------------------
    public void LoadAllSavedStickers()
    {
        string folder = Path.Combine(Application.persistentDataPath, "MyBrandStickersPhoto");

        if (!Directory.Exists(folder))
        {
            Debug.Log("画像フォルダがありません");
            return;
        }

        string[] files = Directory.GetFiles(folder, "*.png");

        savedImagePaths.Clear();
        savedImagePaths.AddRange(files);
        // ★ 名前順に並べる
        savedImagePaths.Sort();
    }

    // ----------------------------------------
    // ② 指定ページの画像を表示
    // ----------------------------------------
    public void ShowPage(int page)
    {
        if (savedImagePaths.Count == 0)
        {
            pageText.text = "P 0 / 0";
            previewImage.texture = null;
            fileNameText.text = "";
            return;
        }

        currentPage = Mathf.Clamp(page, 0, savedImagePaths.Count - 1);

        string path = savedImagePaths[currentPage];

        // PNG 読み込み
        byte[] bytes = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        tex.LoadImage(bytes);

        // RawImage に表示
        previewImage.texture = tex;
        previewImage.color = Color.white;

        // ★ 縮小倍率を適用
        RectTransform rt = previewImage.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(tex.width * displayScale, tex.height * displayScale);

        // ★ ファイル名（拡張子なし）を表示
        string fileName = Path.GetFileNameWithoutExtension(path);
        fileNameText.text = fileName;

        // ページ表示更新
        pageText.text = $"P {currentPage + 1} / {savedImagePaths.Count}";

        Debug.Log($"ShowPage: {currentPage} / {savedImagePaths.Count}");
        Debug.Log("Path: " + savedImagePaths[currentPage]);
    }

    // ----------------------------------------
    // ③ 次のページ
    // ----------------------------------------
    public void NextPage()
    {
        if (currentPage < savedImagePaths.Count - 1)
            ShowPage(currentPage + 1);
    }

    // ----------------------------------------
    // ④ 前のページ
    // ----------------------------------------
    public void PrevPage()
    {
        if (currentPage > 0)
            ShowPage(currentPage - 1);
    }

    // ----------------------------------------
    // ⑤ 最初のページへ
    // ----------------------------------------
    public void FirstPage()
    {
        ShowPage(0);
    }

    // ----------------------------------------
    // ⑥ 最後のページへ
    // ----------------------------------------
    public void LastPage()
    {
        if (savedImagePaths.Count > 0)
            ShowPage(savedImagePaths.Count - 1);
    }

    // Loadポップアップを開く
    public void OpenPopup()
    {
        popupPanel.SetActive(true);
        LoadMyBrandStickers();
    }

    // キャンセル
    public void Cancel()
    {
        popupPanel.SetActive(false);
    }

    public void CreateStickerFromCurrentImage()
    {
        if (previewImage.texture == null) return;

        Texture2D tex = previewImage.texture as Texture2D;
        // 建立 Sprite
        Sprite sprite = Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f),
            100f
        );

        string stickerName = fileNameText.text;
        GameObject obj = new GameObject(stickerName);
        obj.tag = "Sticker";
        obj.layer = LayerMask.NameToLayer("Sticker");

        // 1. 先處理 SpriteRenderer
        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;

        // 2. 處理材質 (這是最容易出錯的地方)
        Material mat = Resources.Load<Material>("Materials/ChangeColor_Shape");
        if (mat != null)
        {
            // 使用實例化的材質，避免多個貼紙共用同一個實體導致渲染異常
            sr.material = new Material(mat);
        }
        else
        {
            sr.material = new Material(Shader.Find("Sprites/Default"));
        }

        obj.transform.position = Vector3.zero;

        // 3. 處理 Collider (確保點擊範圍正確)
        BoxCollider2D col = obj.AddComponent<BoxCollider2D>();
        col.size = sr.sprite.bounds.size; // 強制校正 Collider 大小

        // 4. 處理 Sorting Group (確保整體排序)
        SortingGroup sg = obj.AddComponent<SortingGroup>();
        sg.sortingOrder = 100;
        sg.sortAtRoot = true;

        // 5. 處理外框 SelectSticker
        if (selectStickerPrefab != null)
        {
            GameObject select = Instantiate(selectStickerPrefab, obj.transform);
            select.name = "SelectSticker";
            select.transform.localPosition = Vector3.zero;
            select.SetActive(false);

            SpriteRenderer selectSR = select.GetComponent<SpriteRenderer>();
            if (selectSR != null)
            {
                // 外框在 Sorting Group 內部必須比本體高
                selectSR.sortingOrder = 1;
                sr.sortingOrder = 0; // 本體設為 0

                // 修正縮放邏輯：使用本地尺寸而非世界邊界
                float padding = 1.1f;
                float sWidth = sprite.rect.width / sprite.pixelsPerUnit;
                float sHeight = sprite.rect.height / sprite.pixelsPerUnit;
                float fWidth = selectSR.sprite.rect.width / selectSR.sprite.pixelsPerUnit;
                float fHeight = selectSR.sprite.rect.height / selectSR.sprite.pixelsPerUnit;

                select.transform.localScale = new Vector3(
                    (sWidth / fWidth) * padding,
                    (sHeight / fHeight) * padding,
                    1f
                );
            }
        }

        // 6. 註冊到 LayerManager
        if (globalLayerManager != null)
        {
            globalLayerManager.RegisterNewLayer(sg);
        }

        popupPanel.SetActive(false);
    }
}