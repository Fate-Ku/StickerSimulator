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
        public Color color;
        public int layer;
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


    // 作成したシールの保存
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
            
            // 自身のSpriteRendererを取得
            SpriteRenderer spriteRenderer = s.GetComponent<SpriteRenderer>();
            //色情報
            //Color objectColor = spriteRenderer.color;
            data.color = spriteRenderer.color;

            //レイヤー情報
            //Order in Layerを取得
            //int currentOrder = spriteRenderer.sortingOrder;
            data.layer = spriteRenderer.sortingOrder;

            saveData.stickers.Add(data);
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SavePath, json);

        Debug.Log("シール保存完了: " + SavePath);

        //シール保存完了後、オブジェクトを全削除
        DestroyObjectsInArea();
    }


    // シールの読み込み
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

    //指定範囲(マイブランド画面の作業範囲)のオブジェクトを全て削除
    [SerializeField] Vector2 boxSize = new Vector2(3f, 2f); // 四角形のサイズ
    [SerializeField] float rotation = 0f; // 回転角度
    [SerializeField] string targetTag = "Sticker"; // 削除対象のタグ
    public void DestroyObjectsInArea()
    {
        // 指定範囲内の全Collider2Dを取得
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxSize, rotation);

        foreach (Collider2D hit in hits)
        {
            // nullチェック + タグ判定
            if (hit != null && hit.CompareTag(targetTag))
            {
                Destroy(hit.gameObject);
            }
        }
    }
    
}
