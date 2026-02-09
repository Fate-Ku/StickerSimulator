using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;

public class ZunkanDownloadManager : MonoBehaviour
{
    public TextMeshProUGUI fileNameText;

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
        SaveAllStickersAsPNG(savePath);

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

    public void SaveAllStickersAsPNG(string savePath)
{
    GameObject[] stickers = GameObject.FindGameObjectsWithTag("Sticker");
    if (stickers.Length == 0)
    {
        Debug.LogError("ステッカーがありません");
        return;
    }

    // ★ MyBrandStickersPhoto のフォルダ
    string photoFolder = Path.Combine(Application.persistentDataPath, "MyBrandStickersPhoto");

    // ★ Download フォルダ
    string downloadFolder = Path.GetDirectoryName(savePath);

    if (!Directory.Exists(downloadFolder))
        Directory.CreateDirectory(downloadFolder);

    foreach (var s in stickers)
    {
        string stickerName = fileNameText.text;

        // ★ 対応 PNG のパス
        string pngPath = Path.Combine(photoFolder, stickerName + ".png");

        if (File.Exists(pngPath))
        {
            // ★ 保存先ファイル名
            string saveFile = Path.Combine(downloadFolder, stickerName + ".png");

            // ★ PNG をコピー保存
            File.Copy(pngPath, saveFile, overwrite: true);

            Debug.Log($"保存成功: {saveFile}");
        }
        else
        {
            Debug.LogWarning($"PNG が見つかりません: {pngPath}");
        }
    }

    Debug.Log("すべてのステッカー PNG 保存完了");
}



}
