using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeScene(string sceneName)
    {
        Time.timeScale = 1;
        StartCoroutine(ChangeSceneCo(sceneName));
    }

    // change scene
    private IEnumerator ChangeSceneCo(string sceneName)
    {

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);

        yield return new WaitForSeconds(.02f);

    }

    private void Update()
    {
        // F1 → Menu に戻る
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene("MainMenu");
        }

        // ESC → ゲーム終了
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }

        // F2 → MyBrandStickersInfo / Photo / StickerBook.json 削除
        if (Input.GetKeyDown(KeyCode.F2))
        {
            DeleteStickerData();
        }

        // F3 → Download 資料夾の ScreenShoot PNG を全部削除
        if (Input.GetKeyDown(KeyCode.F3))
        {
            DeleteAllScreenShoots();
        }

        // F4 → MyBrandStickersInfo ひとつ上のFileを開く
        if (Input.GetKeyDown(KeyCode.F4))
        {
            OpenParentOfMyBrandStickersInfo();
        }

        // F5 → Download Fileを開く
        if (Input.GetKeyDown(KeyCode.F5))
        {
            OpenDownloadFolder();
        }


    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        // Unity Editor の場合は再生停止
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 実機ビルドの場合はアプリ終了
        Application.Quit();
#endif
    }

    private void DeleteAllScreenShoots()
    {
        string downloadFolder = GetDownloadFolder();

        if (!Directory.Exists(downloadFolder))
            return;

        // ScreenShoot-xxxx.png を全部取得
        string[] files = Directory.GetFiles(downloadFolder, "ScreenShoot-*.png");

        foreach (string file in files)
        {
            File.Delete(file);
            UnityEngine.Debug.Log("削除: " + file);
        }

        UnityEngine.Debug.Log("Download 資料夾の ScreenShoot PNG を全部削除しました");
    }

    private string GetDownloadFolder()
    {
#if UNITY_ANDROID
        return Path.Combine(Application.persistentDataPath, "Download");
#elif UNITY_IOS
        return Path.Combine(Application.persistentDataPath, "Download");
#else
        return Path.Combine(
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile),
            "Downloads"
        );
#endif
    }

    // ★★★ F3 の削除処理 ★★★
    private void DeleteStickerData()
    {
        string root = Application.persistentDataPath;

        string infoFolder = Path.Combine(root, "MyBrandStickersInfo");
        string photoFolder = Path.Combine(root, "MyBrandStickersPhoto");
        string jsonFile = Path.Combine(root, "StickerBook.json");

        // フォルダ削除
        if (Directory.Exists(infoFolder))
        {
            Directory.Delete(infoFolder, true);
            UnityEngine.Debug.Log("削除: " + infoFolder);
        }

        if (Directory.Exists(photoFolder))
        {
            Directory.Delete(photoFolder, true);
            UnityEngine.Debug.Log("削除: " + photoFolder);
        }

        // JSON 削除
        if (File.Exists(jsonFile))
        {
            File.Delete(jsonFile);
            UnityEngine.Debug.Log("削除: " + jsonFile);
        }

        UnityEngine.Debug.Log("F3: MyBrandStickersInfo / Photo / StickerBook.json を削除しました");
    }

    // F4
    private void OpenParentOfMyBrandStickersInfo()
    {
        // MyBrandStickersInfo のパス
        string infoFolder = Path.Combine(Application.persistentDataPath, "MyBrandStickersInfo");

        // 親フォルダを取得
        string parentFolder = Directory.GetParent(infoFolder).FullName;

        // 親フォルダが無ければ作成
        if (!Directory.Exists(parentFolder))
            Directory.CreateDirectory(parentFolder);

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        System.Diagnostics.Process.Start("explorer.exe", parentFolder);
#elif UNITY_STANDALONE_OSX
    System.Diagnostics.Process.Start("open", parentFolder);
#else
    Debug.Log("この OS ではフォルダを直接開けません: " + parentFolder);
#endif

        UnityEngine.Debug.Log("F4: MyBrandStickersInfo の親フォルダを開きました → " + parentFolder);
    }

    // ★★★ F5：Download 資料夾を開く ★★★
    private void OpenDownloadFolder()
    {
        string folder = GetDownloadFolder();

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        Process.Start("explorer.exe", folder);
#elif UNITY_STANDALONE_OSX
        Process.Start("open", folder);
#elif UNITY_ANDROID
        // Android はフォルダを直接開けないのでログ表示のみ
        UnityEngine.Debug.Log("Android ではフォルダを直接開けません: " + folder);
#elif UNITY_IOS
        UnityEngine.Debug.Log("iOS ではフォルダを直接開けません: " + folder);
#endif

        UnityEngine.Debug.Log("Download 資料夾を開きました: " + folder);
    }


}
