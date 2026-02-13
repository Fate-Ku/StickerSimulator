using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OutputStickerTool : MonoBehaviour
{
    [Header("Popup UI")]
    public GameObject popupPanel;
    public TMP_InputField nameInput;
    public TextMeshProUGUI errorText;

    [Header("Success Popup")]
    public GameObject successPanel;
    public TextMeshProUGUI successMessage;

    [Header("Question Popup")]
    public GameObject QuestionPanel;
    public TextMeshProUGUI QuestionMessage;

    [Header("Save System")]
    public Camera pinkFrameCamera; // 粉色框専用カメラ

    // ─────────────────────────────
    // 保存データ構造
    // ─────────────────────────────

    [Serializable]
    public class ChildLayerInfo
    {
        public string childName;
        public int sortingOrder;

        // ★ 子の material.color（0〜255）
        public byte r, g, b, a;
    }

    [Serializable]
    public class StickerData
    {
        public string prefabName;

        public float x;
        public float y;
        public float rotation;

        public float scaleX;
        public float scaleY;

        // ★ 親の material.color（0〜255）
        public byte r, g, b, a;

        public int sortingOrder;

        public List<ChildLayerInfo> childLayers = new List<ChildLayerInfo>();
    }

    [Serializable]
    public class StickerSaveData
    {
        public List<StickerData> stickers = new List<StickerData>();
    }


    private void Start()
    {
        popupPanel.SetActive(false);
        successPanel.SetActive(false);
        QuestionPanel.SetActive(false);
        errorText.gameObject.SetActive(false);
    }

    private void Update()
    {
        //if (popupPanel.activeSelf && nameInput.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                StartCoroutine(HandleEnterKey());
            }
        }
    }

    private IEnumerator HandleEnterKey()
    {
        // ★ IME を確定
        nameInput.DeactivateInputField();

        // ★ 1フレーム待つ（IME が text に反映される）
        yield return null;

        // ★ フォーカスを戻す
        nameInput.Select();
        nameInput.ActivateInputField();
    }

    // 保存ポップアップを開く
    public void OpenPopup()
    {
        GameObject[] stickers = GameObject.FindGameObjectsWithTag("Sticker");
        if (stickers.Length == 0)
        {
            WrongMsg();
            return;
        }


        popupPanel.SetActive(true);
        nameInput.text = "";

        nameInput.Select();
        nameInput.ActivateInputField();

        errorText.gameObject.SetActive(false);
    }

    // キャンセル
    public void Cancel()
    {
        popupPanel.SetActive(false);
    }

    public void qCancel()
    {
        QuestionPanel.SetActive(false);
    }

    // 保存確認ボタン
    public void Confirm()
    {
        // ★ 日本語入力を確定させる（IME の未確定文字を確定）
        nameInput.DeactivateInputField();

        string fileName = nameInput.text.Trim();

        // 未入力チェック
        if (string.IsNullOrEmpty(fileName))
        {
            errorText.text = "画像名を入力してください";
            errorText.gameObject.SetActive(true);

            // ★ エラー時はフォーカスを戻す
            nameInput.Select();
            nameInput.ActivateInputField();

            return;
        }

        // 追加したいフォルダ名前
        string imageFolderName = "MyBrandStickersPhoto";
        string imageFolderPath = Path.Combine(Application.persistentDataPath, imageFolderName);
        string jsonFolderName = "MyBrandStickersInfo";
        string jsonFolderPath = Path.Combine(Application.persistentDataPath, jsonFolderName);

        // if folder is not exists → auto create
        if (!Directory.Exists(imageFolderPath)) { Directory.CreateDirectory(imageFolderPath); }
        if (!Directory.Exists(jsonFolderPath)) { Directory.CreateDirectory(jsonFolderPath); }

        // PNG Path
        string imagePath = Path.Combine(imageFolderPath, fileName + ".png");
        // JSON Path
        string jsonPath = Path.Combine(jsonFolderPath, fileName + ".json");

        // 重複チェック
        if (File.Exists(imagePath) || File.Exists(jsonPath))
        {
            QuestionPanel.SetActive(true);
            QuestionMessage.text = "ファイル名は既に存在します\n上書きしますか？";
            QuestionMessage.gameObject.SetActive(true);
            return;
        }

        // 保存処理
        SaveAllStickersAsPNG(imagePath);
        //SaveImage(imagePath);
        SaveStickerData(jsonPath);

        popupPanel.SetActive(false);
        ShowSuccess(fileName);
    }

    public void Confirm2()
    {

        string fileName = nameInput.text.Trim();

        // 追加したいフォルダ名前
        string imageFolderName = "MyBrandStickersPhoto";
        string imageFolderPath = Path.Combine(Application.persistentDataPath, imageFolderName);
        string jsonFolderName = "MyBrandStickersInfo";
        string jsonFolderPath = Path.Combine(Application.persistentDataPath, jsonFolderName);

        // PNG Path
        string imagePath = Path.Combine(imageFolderPath, fileName + ".png");
        // JSON Path
        string jsonPath = Path.Combine(jsonFolderPath, fileName + ".json");

        // 保存処理
        SaveAllStickersAsPNG(imagePath);
        //SaveImage(imagePath);
        SaveStickerData(jsonPath);

        popupPanel.SetActive(false);
        QuestionPanel.SetActive(false);
        ShowSuccess(fileName);
    }

    // 保存成功ポップアップ
    private void ShowSuccess(string fileName)
    {
        successPanel.SetActive(true);
        successMessage.text = $"「{fileName}」保存成功！";

        // 自動的にクローズされる
        StartCoroutine(AutoCloseSuccess());
    }

    private IEnumerator AutoCloseSuccess()
    {
        yield return new WaitForSeconds(1f);
        successPanel.SetActive(false);
    }

    private void WrongMsg()
    {
        successPanel.SetActive(true);
        successMessage.text = $"シールを作りませんか？";

        StartCoroutine(AutoCloseSuccess());

    }


    // -----------------------------
    // PNG 保存（SpriteRenderer 合成）
    // -----------------------------
    //private void SaveImage(string savePath)
    //{
    //    Camera cam = pinkFrameCamera;

    //    float heightWorldUnits = cam.orthographicSize * 2f;
    //    float widthWorldUnits = heightWorldUnits * cam.aspect;

    //    int pixelsPerUnit = 100;
    //    int rtWidth = Mathf.RoundToInt(widthWorldUnits * pixelsPerUnit);
    //    int rtHeight = Mathf.RoundToInt(heightWorldUnits * pixelsPerUnit);

    //    // --- 修改開始 ---
    //    // 讓 Unity 自動根據專案設置處理色彩空間 (使用預設 constructor)
    //    RenderTexture rt = new RenderTexture(rtWidth, rtHeight, 24, RenderTextureFormat.ARGB32);

    //    // 如果專案是 Linear 模式，這會確保顏色正確轉換到 sRGB
    //    rt.Create();
    //    // --- 修改結束 ---

    //    cam.targetTexture = rt;
    //    cam.Render();

    //    RenderTexture.active = rt;

    //    // 這裡最後一個參數 linear 設為 false (即使用 sRGB)，這是 PNG 圖片的標準格式
    //    Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false, false);

    //    tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
    //    tex.Apply();

    //    byte[] bytes = tex.EncodeToPNG();
    //    File.WriteAllBytes(savePath, bytes);

    //    cam.targetTexture = null;
    //    RenderTexture.active = null;

    //    Destroy(rt);
    //    Destroy(tex);

    //    Debug.Log("PNG 保存完成: " + savePath);
    //}

    private void SaveStickerData(string jsonPath)
    {
        StickerSaveData saveData = new StickerSaveData();

        GameObject[] stickers = GameObject.FindGameObjectsWithTag("Sticker");

        foreach (var s in stickers)
        {
            StickerData data = new StickerData();
            data.prefabName = s.name.Replace("(Clone)", "");

            data.x = s.transform.position.x;
            data.y = s.transform.position.y;
            data.rotation = s.transform.eulerAngles.z;

            data.scaleX = s.transform.localScale.x;
            data.scaleY = s.transform.localScale.y;

            // 親の SpriteRenderer
            SpriteRenderer parentSR = s.GetComponentInChildren<SpriteRenderer>();
            if (parentSR != null)
            {
                Color32 c = parentSR.material.color;   // ★ material.color を保存

                data.r = c.r;
                data.g = c.g;
                data.b = c.b;
                data.a = c.a;

                data.sortingOrder = parentSR.sortingOrder;
            }

            // ★ 子の SpriteRenderer を保存
            SpriteRenderer[] children = s.GetComponentsInChildren<SpriteRenderer>();

            foreach (var child in children)
            {
                ChildLayerInfo info = new ChildLayerInfo();
                info.childName = child.gameObject.name;
                info.sortingOrder = child.sortingOrder;

                Color32 cc = child.material.color;     // ★ material.color を保存
                info.r = cc.r;
                info.g = cc.g;
                info.b = cc.b;
                info.a = cc.a;

                data.childLayers.Add(info);
            }

            saveData.stickers.Add(data);
        }

        // ③ JSON に書き戻す（上書きだが内容は追加済み）
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(jsonPath, json);

        Debug.Log("JSON 保存完成: " + jsonPath);
    }

    public void SaveAllStickersAsPNG(string savePath)
    {
        GameObject[] stickers = GameObject.FindGameObjectsWithTag("Sticker");
        if (stickers.Length == 0)
        {
            Debug.LogError("ステッカーがありません");
            return;
        }

        // ① 全ステッカーの Bounds を取得
        bool first = true;
        Bounds totalBounds = new Bounds();

        foreach (var s in stickers)
        {
            SpriteRenderer sr = s.GetComponentInChildren<SpriteRenderer>();
            if (sr == null) continue;

            if (first)
            {
                totalBounds = sr.bounds;
                first = false;
            }
            else
            {
                totalBounds.Encapsulate(sr.bounds);
            }
        }

        // ② Bounds のサイズを取得
        float widthWorld = totalBounds.size.x;
        float heightWorld = totalBounds.size.y;

        // ③ ピクセルサイズに変換
        int pixelsPerUnit = 100; 
        int texWidth = Mathf.RoundToInt(widthWorld * pixelsPerUnit);
        int texHeight = Mathf.RoundToInt(heightWorld * pixelsPerUnit);

        // ④ RenderTexture を作成
        RenderTexture rt = new RenderTexture(texWidth, texHeight, 24, RenderTextureFormat.ARGB32);
        rt.Create();

        // ⑤ カメラをステッカー全体に合わせる
        Camera cam = pinkFrameCamera;
        cam.targetTexture = rt;

        cam.orthographic = true;
        cam.orthographicSize = heightWorld / 2f;

        // カメラ位置を中央に
        cam.transform.position = new Vector3(
            totalBounds.center.x,
            totalBounds.center.y,
            cam.transform.position.z
        );

        // ⑥ 撮影
        cam.Render();

        RenderTexture.active = rt;

        // ⑦ Texture2D に書き出し
        Texture2D tex = new Texture2D(texWidth, texHeight, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, texWidth, texHeight), 0, 0);
        tex.Apply();

        // ⑧ PNG 保存
        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(savePath, bytes);

        // ⑨ 後処理
        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        Destroy(tex);

        Debug.Log("ステッカー全体をまとめて PNG 保存完了: " + savePath);
    }
}