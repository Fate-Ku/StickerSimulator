using System;
using System.Collections.Generic;
using UnityEngine;

public class StickersSaveManager : MonoBehaviour
{
    [Serializable]
    public class StickerData
    {
        public string prefabName; // どのステッカーかを特定
        public float x;
        public float y;
    }

    [Serializable]
    public class StickerSaveData
    {
        public List<StickerData> stickers = new List<StickerData>();
    }

    // Sticker プレハブを指定しておく（複製で使ったもの）
    public List<GameObject> stickerPrefabs;


    // 起動時に自動的にロードする
    private void Start()
    {
        LoadStickers();
    }

    public void SaveStickers()
    {
        StickerSaveData saveData = new StickerSaveData();

        // シーン内の "Sticker" タグを持つ複製されたオブジェクトをすべて探す
        GameObject[] stickers = GameObject.FindGameObjectsWithTag("Sticker");

        foreach (var s in stickers)
        {
            StickerData data = new StickerData();
            data.prefabName = s.name.Replace("(Clone)", ""); // 元プレハブ名に戻す
            data.x = s.transform.position.x;
            data.y = s.transform.position.y;

            saveData.stickers.Add(data);
        }

        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString("StickerSave", json);
        PlayerPrefs.Save();

        Debug.Log("ステッカー保存完了: " + json);
    }

    public void LoadStickers()
    {
        string json = PlayerPrefs.GetString("StickerSave", "");

        if (string.IsNullOrEmpty(json))
        {
            Debug.Log("保存データなし");
            return;
        }

        StickerSaveData saveData =
            JsonUtility.FromJson<StickerSaveData>(json);

        foreach (var data in saveData.stickers)
        {
            // 名前から対応するプレハブを探す
            GameObject prefab =
                stickerPrefabs.Find(p => p.name == data.prefabName);

            if (prefab != null)
            {
                Instantiate(prefab,
                    new Vector2(data.x, data.y),
                    Quaternion.identity).tag = "Sticker";
            }
        }

        Debug.Log("ステッカーロード完了");
    }
}
