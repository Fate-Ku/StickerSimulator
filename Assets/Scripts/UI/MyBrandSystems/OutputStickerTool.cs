using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class OutputStickerTool : MonoBehaviour
{
    [Header("Popup UI")]
    public GameObject popupPanel;
    public TMP_InputField nameInput;
    public TextMeshProUGUI errorText;

    [Header("Success Popup")]
    public GameObject successPanel;
    public TextMeshProUGUI successMessage;

    [Header("Save System")]
    public Camera pinkFrameCamera; // 粉色框専用カメラ

    private void Start()
    {
        popupPanel.SetActive(false);
        successPanel.SetActive(false);
        errorText.gameObject.SetActive(false);
    }

    // 保存ポップアップを開く
    public void OpenPopup()
    {
        popupPanel.SetActive(true);
        nameInput.text = "";
        errorText.gameObject.SetActive(false);
    }

    // キャンセル
    public void Cancel()
    {
        popupPanel.SetActive(false);
    }

    // 保存確認ボタン
    public void Confirm()
    {
        string fileName = nameInput.text.Trim();

        // 未入力チェック
        if (string.IsNullOrEmpty(fileName))
        {
            errorText.text = "画像名を入力してください";
            errorText.gameObject.SetActive(true);
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
        if (File.Exists(imagePath) && File.Exists(jsonPath))
        {
            errorText.text = "ファイル名は既に存在します。\n名前を変更してください。";
            errorText.gameObject.SetActive(true);
            return;
        }

        // 保存処理
        SaveImage(imagePath);
        SaveStickerData(jsonPath);

        popupPanel.SetActive(false);
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


    // -----------------------------
    // PNG 保存（SpriteRenderer 合成）
    // -----------------------------
    private void SaveImage(string savePath)
    {
        Camera cam = pinkFrameCamera;

        float heightWorldUnits = cam.orthographicSize * 2f;
        float widthWorldUnits = heightWorldUnits * cam.aspect;

        int pixelsPerUnit = 100;
        int rtWidth = Mathf.RoundToInt(widthWorldUnits * pixelsPerUnit);
        int rtHeight = Mathf.RoundToInt(heightWorldUnits * pixelsPerUnit);

        // --- 修改開始 ---
        // 讓 Unity 自動根據專案設置處理色彩空間 (使用預設 constructor)
        RenderTexture rt = new RenderTexture(rtWidth, rtHeight, 24, RenderTextureFormat.ARGB32);

        // 如果專案是 Linear 模式，這會確保顏色正確轉換到 sRGB
        rt.Create();
        // --- 修改結束 ---

        cam.targetTexture = rt;
        cam.Render();

        RenderTexture.active = rt;

        // 這裡最後一個參數 linear 設為 false (即使用 sRGB)，這是 PNG 圖片的標準格式
        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false, false);

        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(savePath, bytes);

        cam.targetTexture = null;
        RenderTexture.active = null;

        Destroy(rt);
        Destroy(tex);

        Debug.Log("PNG 保存完成: " + savePath);
    }

    // -----------------------------
    // JSON 保存（SpriteRenderer 情報）
    // -----------------------------
    [System.Serializable]
    public class StickerInfo
    {
        public string name;
        public Vector2 position;
        public float rotation;
        public Vector3 scale;
        public Color color;
    }

    [System.Serializable]
    public class StickerInfoList
    {
        public List<StickerInfo> stickers = new List<StickerInfo>();
    }

    private void SaveStickerData(string jsonPath)
    {
        StickerInfoList infoList;

        // ① 既存ファイルがあるなら読み込む
        if (File.Exists(jsonPath))
        {
            string oldJson = File.ReadAllText(jsonPath);
            infoList = JsonUtility.FromJson<StickerInfoList>(oldJson);

            if (infoList == null)
                infoList = new StickerInfoList();
        }
        else
        {
            // 既存ファイルがない場合は新規作成
            infoList = new StickerInfoList();
        }

        // ② 今のシーンの Sticker を追加
        GameObject[] stickers = GameObject.FindGameObjectsWithTag("Sticker");

        foreach (var s in stickers)
        {
            var renderer = s.GetComponent<SpriteRenderer>();
            if (renderer == null) continue;

            StickerInfo info = new StickerInfo();
            info.name = s.name.Replace("(Clone)", "");
            info.position = s.transform.position;
            info.rotation = s.transform.eulerAngles.z;
            info.scale = s.transform.localScale;
            info.color = renderer.color;

            infoList.stickers.Add(info);
        }

        // ③ JSON に書き戻す（上書きだが内容は追加済み）
        string json = JsonUtility.ToJson(infoList, true);
        File.WriteAllText(jsonPath, json);

        Debug.Log("JSON 保存完成: " + jsonPath);
    }
}