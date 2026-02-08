using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StickerFileSaveManager : MonoBehaviour
{
    [Header("Success Popup")]
    public GameObject successPanel;
    public TextMeshProUGUI successMessage;

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

    // ─────────────────────────────
    // 設定
    // ─────────────────────────────

    //public List<GameObject> stickerPrefabs;

    [SerializeField] public string sceneName;

    public string SavePath => Path.Combine(Application.persistentDataPath, sceneName + ".json");

    private void Start()
    {
        if (string.IsNullOrEmpty(sceneName))
            sceneName = SceneManager.GetActiveScene().name;

        LoadFromFile();
    }

    // ─────────────────────────────
    // 保存処理
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

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SavePath, json);

        ShowSuccess();
        Debug.Log("シール保存完了: " + SavePath);
    }

    // ─────────────────────────────
    // 読み込み処理
    // ─────────────────────────────
    public void LoadFromFile()
    {
        // 既存削除
        GameObject[] oldStickers = GameObject.FindGameObjectsWithTag("Sticker");
        foreach (var s in oldStickers) Destroy(s);

        if (!File.Exists(SavePath))
        {
            Debug.Log("保存ファイルがありません: " + SavePath);
            return;
        }

        string json = File.ReadAllText(SavePath);
        StickerSaveData saveData = JsonUtility.FromJson<StickerSaveData>(json);

        Debug.Log($"JSONから{saveData.stickers.Count}個のシールデータを読み込みました。");

        string photoFolder = Path.Combine(Application.persistentDataPath, "MyBrandStickersPhoto");

        foreach (var data in saveData.stickers)
        {
            // ① まず Resources からプレハブを探す
            GameObject prefab = Resources.Load<GameObject>("Stickers/" + data.prefabName);

            GameObject obj = null;

            if (prefab != null)
            {
                // ★ プレハブが見つかった → 通常ロード
                obj = Instantiate(
                    prefab,
                    new Vector2(data.x, data.y),
                    Quaternion.Euler(0, 0, data.rotation)
                );

                obj.transform.localScale = new Vector3(data.scaleX, data.scaleY, 1);
                obj.tag = "Sticker";
            }
            else
            {
                // ★ プレハブが見つからない → MyBrandStickersPhoto から PNG を探す
                string pngPath = Path.Combine(photoFolder, data.prefabName + ".png");

                if (File.Exists(pngPath))
                {
                    Debug.Log($"PNG を使用してステッカー生成: {pngPath}");

                    // ① Texture2D を読み込む
                    byte[] bytes = File.ReadAllBytes(pngPath);
                    Texture2D tex = new Texture2D(2, 2);
                    tex.LoadImage(bytes);

                    // ② Sprite に変換
                    Sprite sprite = Sprite.Create(
                        tex,
                        new Rect(0, 0, tex.width, tex.height),
                        new Vector2(0.5f, 0.5f),
                        100f
                    );

                    // ③ GameObject 名は JSON の prefabName を使う
                    obj = new GameObject(data.prefabName);
                    obj.tag = "Sticker";
                    obj.layer = LayerMask.NameToLayer("Sticker");


                    // ④ SpriteRenderer を追加
                    SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
                    sr.sprite = sprite;
                    sr.material = new Material(Shader.Find("Sprites/Default"));


                    // ★ マテリアル設定
                    Material mat = Resources.Load<Material>("Materials/ChangeColor_Shape");
                    if (mat != null)
                        sr.material = mat;

                    // 位置・回転・スケール
                    obj.transform.position = new Vector2(data.x, data.y);
                    obj.transform.rotation = Quaternion.Euler(0, 0, data.rotation);
                    obj.transform.localScale = new Vector3(data.scaleX, data.scaleY, 1);

                    // ★ 必要なコンポーネント追加
                    obj.AddComponent<BoxCollider2D>();
                    obj.AddComponent<LayerControllerTool>();
                    obj.AddComponent<RotateTool>();
                    obj.AddComponent<Sticker_Manager>();
                }
                else
                {
                    Debug.LogError($"Prefab も PNG も見つかりません: {data.prefabName}");
                    continue;
                }
            }

            // -----------------------------
            // ★ 親の material.color を復元
            // -----------------------------
            SpriteRenderer parentSR = obj.GetComponentInChildren<SpriteRenderer>();
            if (parentSR != null)
            {
                parentSR.material.color = new Color32(data.r, data.g, data.b, data.a);
                parentSR.sortingOrder = data.sortingOrder;
            }

            // -----------------------------
            // ★ 子の SpriteRenderer を復元
            // -----------------------------
            SpriteRenderer[] children = obj.GetComponentsInChildren<SpriteRenderer>();

            foreach (var child in children)
            {
                foreach (var saved in data.childLayers)
                {
                    if (child.gameObject.name == saved.childName)
                    {
                        child.sortingOrder = saved.sortingOrder;
                        child.material.color = new Color32(saved.r, saved.g, saved.b, saved.a);
                    }
                }
            }
        }

        Debug.Log("シールロード完了: " + SavePath);
    }

    // ─────────────────────────────
    // 削除
    // ─────────────────────────────
    public void DeleteSaveFile()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log($"{sceneName}.json を削除しました。");
        }
        else
        {
            Debug.Log("削除対象の保存ファイルが存在しません: " + SavePath);
        }
    }

    public void DeleteAllSticker()
    {
        GameObject[] oldStickers = GameObject.FindGameObjectsWithTag("Sticker");
        foreach (var s in oldStickers) Destroy(s);
    }

    // 保存成功ポップアップ
    private void ShowSuccess()
    {
        successPanel.SetActive(true);
        successMessage.text = $"保存しました！";

        // 自動的にクローズされる
        StartCoroutine(AutoCloseSuccess());
    }

    private IEnumerator AutoCloseSuccess()
    {
        yield return new WaitForSeconds(1f);
        successPanel.SetActive(false);
    }
}