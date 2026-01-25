using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class StickerFileSave : MonoBehaviour
{
    //シールデータ
    public class StickerData
    {
        public string prefabName;
        public float x;
        public float y;
        public float rotation;
        public float scaleX;
        public float scaleY;
        public float color;
        public float layer;
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
}
