using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

    private string SavePath => Path.Combine(Application.persistentDataPath, "stickers.json");


    //private void Start()
    //{
    //    LoadFromFile();
    //}

    private void Start()
    {
        StartCoroutine(LoadNextFrame());
    }

    private System.Collections.IEnumerator LoadNextFrame()
    {
        yield return null; // 1フレーム待つ
        LoadFromFile();
    }


    // ─────────────────────────────
    // 保存
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

        Debug.Log("ステッカー保存完了: " + SavePath);
    }


    // ─────────────────────────────
    // 読み込み
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

        // 既存のステッカーを全削除
        GameObject[] oldStickers = GameObject.FindGameObjectsWithTag("Sticker");
        foreach (var s in oldStickers) Destroy(s);

        foreach (var data in saveData.stickers)
        {
            GameObject prefab = stickerPrefabs.Find(p => p.name == data.prefabName);
            if (prefab != null)
            {
                GameObject obj = Instantiate(prefab,
                    new Vector2(data.x, data.y),
                    Quaternion.Euler(0, 0, data.rotation));

                obj.transform.localScale = new Vector3(data.scaleX, data.scaleY, 1);
                obj.tag = "Sticker"; // 複製不可タグに統一
            }
        }

        Debug.Log("ステッカーロード完了: " + SavePath);
    }
}
