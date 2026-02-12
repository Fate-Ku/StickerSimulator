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
        if (previewImage.texture == null)
        {
            Debug.Log("表示中の画像がありません");
            return;
        }

        // ① Texture2D を取得
        Texture2D tex = previewImage.texture as Texture2D;

        // ② Sprite に変換
        Sprite sprite = Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f),
            100f
        );

        // ★ ③ RawImage のファイル名をそのまま GameObject 名にする
        string stickerName = fileNameText.text;
        GameObject obj = new GameObject(stickerName);
        obj.tag = "Sticker";
        obj.layer = LayerMask.NameToLayer("Sticker");

        // ④ SpriteRenderer を追加
        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.material = new Material(Shader.Find("Sprites/Default"));

        // ★ 本体 sortingOrder（必要なら調整）
        //sr.sortingOrder = 25;

        // ★ マテリアル設定（必要なら）
        Material mat = Resources.Load<Material>("Materials/ChangeColor_Shape");
        if (mat != null)
            sr.material = mat;

        // ⑤ 画像サイズに応じてスケール調整
        //float baseSize = 300f; // ここは好みで調整してOK
        //float scaleFactor = baseSize / Mathf.Max(tex.width, tex.height);
        //obj.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);

        // ⑥ 画面中央に配置
        obj.transform.position = Vector3.zero;

        // ⑦ selectSticker（選択枠）を追加
        GameObject select = Instantiate(selectStickerPrefab, obj.transform);
        select.name = "SelectSticker";
        select.transform.localPosition = Vector3.zero;

        // ★ 初期非表示
        select.SetActive(false);

        // ★ Prefab の SpriteRenderer を取得
        SpriteRenderer prefabSR = selectStickerPrefab.GetComponent<SpriteRenderer>();
        SpriteRenderer selectSR = select.GetComponent<SpriteRenderer>();

        // ★ Prefab の Sprite と Material をそのままコピー
        if (prefabSR != null)
        {
            selectSR.sprite = prefabSR.sprite;
            selectSR.sharedMaterial = prefabSR.sharedMaterial;
        }

        // ★ order in layer = 50
        selectSR.sortingOrder = 50;

        // ★ SelectSticker のサイズをステッカーに合わせる
        if (selectSR.sprite != null)
        {
            float padding = 1.1f;

            // ステッカー本体の SpriteRenderer のワールドサイズ
            Bounds b = sr.bounds;

            float stickerWidth = b.size.x;
            float stickerHeight = b.size.y;

            // 外框 Sprite の元サイズ（ワールド座標）
            float frameWidth = selectSR.sprite.bounds.size.x;
            float frameHeight = selectSR.sprite.bounds.size.y;

            // 外框のスケールを計算
            float scaleX = (stickerWidth / frameWidth) * padding;
            float scaleY = (stickerHeight / frameHeight) * padding;

            select.transform.localScale = new Vector3(scaleX, scaleY, 1);
        }


        // ⑧ 增加 Sorting Group 並設定屬性
        SortingGroup sg = obj.AddComponent<SortingGroup>();
        sg.sortingOrder = 100;    // 設定為你要求的 100
        sg.sortAtRoot = true;      // 勾選 Sort At Root

        // ⑨ 必要なコンポーネント追加
        obj.AddComponent<BoxCollider2D>();
        obj.AddComponent<RotateTool>();
        obj.AddComponent<Sticker_Manager>();

        if (globalLayerManager != null)
        {
            globalLayerManager.RegisterNewLayer(sg);
        }
        else
        {
            Debug.LogError("No Global Layer Manager！請在 Inspector 拖入物件。");
        }


        // ⑨ popup を閉じる
        popupPanel.SetActive(false);

        Debug.Log("ロード画像をステッカーとして生成しました: " + stickerName);
        Debug.Log($"生成成功: {stickerName}, SortingOrder: {sg.sortingOrder}, SortAtRoot: {sg.sortAtRoot}");
    }
}