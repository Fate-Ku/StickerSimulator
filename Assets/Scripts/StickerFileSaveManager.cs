using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
//using UnityEditor.Overlays;

public class StickerFileSaveManager : MonoBehaviour
{
    [Serializable]
    public class StickerData
    {
        public string prefabName;
        public float x;
        public float y;
        public float rotation;
        public float scaleX;
        public float scaleY;
    }

    [Serializable]
    public class StickerSaveData
    {
        public List<StickerData> stickers = new List<StickerData>();
    }

    // プレハブ登録リスト
    public List<GameObject> stickerPrefabs;

    //保存するファイル名前
    [SerializeField]public string sceneName;

    //保存するところ
    public string SavePath => Path.Combine(Application.persistentDataPath, sceneName + ".json");


    //最初からデータを読み込む
    private void Start()
    {
        // シーン名を自動でセット
        if (string.IsNullOrEmpty(sceneName))
        {
            sceneName = SceneManager.GetActiveScene().name;
        }
        LoadFromFile();
    }


    // ─────────────────────────────
    // シール帳の保存
    // ─────────────────────────────
    public void SaveToFile()
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

            saveData.stickers.Add(data);
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SavePath, json);

        Debug.Log("シール保存完了: " + SavePath);
    }


    // ─────────────────────────────
    // シール帳の読み込み
    // ─────────────────────────────
    public void LoadFromFile()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("保存ファイルがありません: " + SavePath);
            return;
        }

        string json = File.ReadAllText(SavePath);
        StickerSaveData saveData = JsonUtility.FromJson<StickerSaveData>(json);

        Debug.Log($"JSONから{saveData.stickers.Count}個のシールデータを読み込みました。");

        //Debug.Log("--- stickerPrefabs リストの内容 ---");
        //foreach (var prefab in stickerPrefabs)
        //{
        //    if (prefab != null)
        //    {
        //        // リストに登録されているGameObjectのnameを出力
        //        Debug.Log($"登録プレハブ名: {prefab.name}");
        //    }
        //    else
        //    {
        //        Debug.LogWarning("登録プレハブ: (None) または削除済みアセットが含まれています。");
        //    }
        //}
        //Debug.Log("------------------------------------");

        foreach (var data in saveData.stickers)
        {
            // JSONから読み込んだ名前を出力
            Debug.Log($"ロードを試行: JSONのprefabNameは '{data.prefabName}' です。");

            GameObject prefab = stickerPrefabs.Find(p => p.name == data.prefabName);

            // プレハブが見つかったかチェック
            if (prefab == null)
            {
                Debug.LogError($"プレハブが見つかりません: {data.prefabName}");
                continue; // 次のデータへ
            }

            if (prefab != null)
            {
                Debug.Log($"プレハブ発見: {data.prefabName}。位置 ({data.x}, {data.y}) に生成します。");

                GameObject obj = Instantiate(prefab,
                    new Vector2(data.x, data.y),
                    Quaternion.Euler(0, 0, data.rotation));

                obj.transform.localScale = new Vector3(data.scaleX, data.scaleY, 1);
                obj.tag = "Sticker"; // 複製不可タグに統一
            }
        }

        Debug.Log("シールロード完了: " + SavePath);
    }


    // ─────────────────────────────
    // 保存ファイル削除(シール帳)
    // ─────────────────────────────
    public void DeleteSaveFile()
    {
        if (File.Exists(SavePath))
        {
            // 現在設定されている sceneName のファイルを削除しにいく
            File.Delete(SavePath);
            Debug.Log($"{sceneName}.json を削除しました。");

            // （オプション）ロードしているステッカーも同時に全削除
            GameObject[] oldStickers = GameObject.FindGameObjectsWithTag("Sticker");
            foreach (var s in oldStickers) Destroy(s);
        }
        else
        {
            Debug.Log("削除対象の保存ファイルが存在しません: " + SavePath);
        }
    }


}
