using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class StickerBookDetail : MonoBehaviour
{
    [Header("UI")]
    public RawImage previewImage;     // 画像を表示する RawImage
    public TMP_Text pageText;         // P X / Y
    public TMP_Text fileNameText;     // 画像名（拡張子なし）


    private List<string> savedImagePaths = new List<string>();
    private int currentPage = 0;

    private void Start()
    {
        LoadAllSavedStickers();
        ShowPage(0);
    }

    // ----------------------------------------
    // ① 保存された PNG を全部読み込む
    // ----------------------------------------
    private void LoadAllSavedStickers()
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
    private void ShowPage(int page)
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

        // ★ 画像のサイズをそのまま RawImage に反映
        RectTransform rt = previewImage.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(tex.width, tex.height);

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
}