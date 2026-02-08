using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;

public class StickerDownloadManager : MonoBehaviour
{
    [Header("Save System")]
    public Camera pinkFrameCamera;

    [Header("Success Popup")]
    public GameObject successPanel;
    public TextMeshProUGUI successMessage;

    private void Start()
    {
        successPanel.SetActive(false);
    }

    public void DownloadToGraph()
    {
        // ★ ① 日付＋時間でファイル名生成
        string timeStamp = System.DateTime.Now.ToString("yyyyMMdd-HHmmss");
        string fileName = "ScreenShoot-" + timeStamp + ".png";

        // ★ ② Download フォルダ取得
        string downloadPath = GetDownloadFolder();

        // フォルダが無ければ作成
        if (!Directory.Exists(downloadPath))
            Directory.CreateDirectory(downloadPath);

        // ★ ③ 保存パス
        string savePath = Path.Combine(downloadPath, fileName);

        // ★ ④ 保存実行
        SaveImage(savePath);

        // ★ ⑤ 成功ポップアップ
        ShowSuccess();
    }

    // -----------------------------
    // OS別 Download フォルダ取得
    // -----------------------------
    private string GetDownloadFolder()
    {
#if UNITY_ANDROID
    // Android の Download フォルダ
    return Path.Combine(Application.persistentDataPath, "Download");

#elif UNITY_IOS
    // iOS は専用 Download が無いのでアプリ内に作成
    return Path.Combine(Application.persistentDataPath, "Download");

#else
        // Windows / Mac / Unity Editor
        return Path.Combine(
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile),
            "Downloads"
        );
#endif
    }

    private void ShowSuccess()
    {
        successPanel.SetActive(true);
        successMessage.text = $"ダウンロードフォルダに保存しました！";

        StartCoroutine(AutoCloseSuccess());
    }

    private IEnumerator AutoCloseSuccess()
    {
        yield return new WaitForSeconds(1f);
        successPanel.SetActive(false);
    }

    // -----------------------------
    // PNG 保存（カメラキャプチャ）
    // -----------------------------
    public void SaveImage(string savePath)
    {
        Camera cam = pinkFrameCamera;

        float heightWorldUnits = cam.orthographicSize * 2f;
        float widthWorldUnits = heightWorldUnits * cam.aspect;

        int pixelsPerUnit = 100;
        int rtWidth = Mathf.RoundToInt(widthWorldUnits * pixelsPerUnit);
        int rtHeight = Mathf.RoundToInt(heightWorldUnits * pixelsPerUnit);

        RenderTexture rt = new RenderTexture(rtWidth, rtHeight, 24, RenderTextureFormat.ARGB32);
        rt.Create();

        cam.targetTexture = rt;
        cam.Render();

        RenderTexture.active = rt;

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
}