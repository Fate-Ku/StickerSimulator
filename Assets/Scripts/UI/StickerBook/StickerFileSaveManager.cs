using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class StickerFileSaveManager : MonoBehaviour
{
    [Header("Success Popup")]
    public GameObject successPanel;
    public TextMeshProUGUI successMessage;

    public GameObject selectStickerPrefab;   // ← SelectSticker の Prefab をアサイン


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

        public int sortingOrder; // childen

        public int groupOrder;
        public bool sortAtRoot;

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

        // ★ 追加：groupOrder（SortingGroup.sortingOrder）昇順に並び替え
        //List<GameObject> sorted = new List<GameObject>(stickers);
        //sorted.Sort((a, b) =>
        //{
        //    var sga = a.GetComponent<UnityEngine.Rendering.SortingGroup>();
        //    var sgb = b.GetComponent<UnityEngine.Rendering.SortingGroup>();

        //    // SortingGroup が無い場合は 0 として扱う
        //    int ga = sga != null ? sga.sortingOrder : 0;
        //    int gb = sgb != null ? sgb.sortingOrder : 0;

        //    return ga.CompareTo(gb); // 小さい → 大きい
        //});


        foreach (var s in stickers)
        {
            // ★ 核心修正：檢查父物件
            // 如果我的父物件不為 null，且父物件的標籤也是 "Sticker"，代表我是別人的子零件
            // 那我就不應該作為一個獨立的「貼紙(prefabName)」被存入。
            if (s.transform.parent != null && s.transform.parent.CompareTag("Sticker"))
            {
                continue;
            }

            StickerData data = new StickerData();
            data.prefabName = s.name.Replace("(Clone)", "");

            data.x = s.transform.position.x;
            data.y = s.transform.position.y;
            data.rotation = s.transform.eulerAngles.z;

            data.scaleX = s.transform.localScale.x;
            data.scaleY = s.transform.localScale.y;

            // Sorting Group 設定
            UnityEngine.Rendering.SortingGroup sg = s.GetComponent<UnityEngine.Rendering.SortingGroup>();
            if (sg != null)
            {
                data.groupOrder = sg.sortingOrder;
                data.sortAtRoot = sg.sortAtRoot;
            }

            // 親の SpriteRenderer
            SpriteRenderer parentSR = s.GetComponent<SpriteRenderer>();
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
            // 2. 處理真正的子物件 (避開主本體與選取框)
            SpriteRenderer[] allRenderers = s.GetComponentsInChildren<SpriteRenderer>(true);

            foreach (var child in allRenderers)
            {
                // ★ 過濾條件：
                // 1. 不能是主本體自己 (parentSR)
                // 2. 不能是選取框 (SelectSticker)
                if (child == parentSR || child.gameObject.name == "SelectSticker") continue;

                ChildLayerInfo info = new ChildLayerInfo();
                info.childName = child.gameObject.name;
                info.sortingOrder = child.sortingOrder;

                Color32 cc = child.material.color;
                info.r = cc.r; info.g = cc.g; info.b = cc.b; info.a = cc.a;

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

        LayerControllerTool manager = FindObjectOfType<LayerControllerTool>();
        if (manager != null) manager.layers.Clear();

        if (!File.Exists(SavePath))
        {
            Debug.Log("保存ファイルがありません: " + SavePath);
            return;
        }

        string json = File.ReadAllText(SavePath);
        StickerSaveData saveData = JsonUtility.FromJson<StickerSaveData>(json);

        Debug.Log($"JSONから{saveData.stickers.Count}個のシールデータを読み込みました。");

        string photoFolder = Path.Combine(Application.persistentDataPath, "MyBrandStickersPhoto");

        // ★ groupOrder 昇順に並び替え（小さい → 大きい）
        saveData.stickers.Sort((a, b) => a.groupOrder.CompareTo(b.groupOrder));

        foreach (var data in saveData.stickers)
        {
            GameObject obj = null;

            string pngPath = Path.Combine(photoFolder, data.prefabName + ".png");

            // ① ★ 先に PNG を探す
            if (File.Exists(pngPath))
            {
                Debug.Log($"PNG を使用してステッカー生成: {pngPath}");

                // Texture 読み込み
                byte[] bytes = File.ReadAllBytes(pngPath);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(bytes);

                // Sprite 化
                Sprite sprite = Sprite.Create(
                    tex,
                    new Rect(0, 0, tex.width, tex.height),
                    new Vector2(0.5f, 0.5f),
                    100f
                );

                // GameObject 作成
                obj = new GameObject(data.prefabName);
                obj.tag = "Sticker";
                obj.layer = LayerMask.NameToLayer("Sticker");

                // SpriteRenderer
                SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
                sr.sprite = sprite;

                // マテリアル
                Material mat = Resources.Load<Material>("Materials/ChangeColor_Shape");
                if (mat != null)
                    sr.material = mat;

                // Transform 復元
                obj.transform.position = new Vector2(data.x, data.y);
                obj.transform.rotation = Quaternion.Euler(0, 0, data.rotation);
                obj.transform.localScale = new Vector3(data.scaleX, data.scaleY, 1);

                // ⑤ ★ 外框（SelectSticker）を追加
                GameObject select = Instantiate(selectStickerPrefab, obj.transform);
                select.name = "SelectSticker";
                select.transform.localPosition = Vector3.zero;

                // 初期非表示
                select.SetActive(false);

                // Prefab の SpriteRenderer をコピー
                SpriteRenderer prefabSR = selectStickerPrefab.GetComponent<SpriteRenderer>();
                SpriteRenderer selectSR = select.GetComponent<SpriteRenderer>();

                if (prefabSR != null)
                {
                    selectSR.sprite = prefabSR.sprite;
                    selectSR.sharedMaterial = prefabSR.sharedMaterial;
                }

                // order in layer
                selectSR.sortingOrder = 50;

                // ★ 外框サイズをステッカーに完全フィットさせる
                {
                    float padding = 1.1f;

                    // ステッカー本体のワールドサイズ
                    Bounds b = sr.bounds;
                    float stickerWidth = b.size.x;
                    float stickerHeight = b.size.y;

                    // 外框 Sprite の元サイズ
                    float frameWidth = selectSR.sprite.bounds.size.x;
                    float frameHeight = selectSR.sprite.bounds.size.y;

                    // スケール計算
                    float scaleX = (stickerWidth / frameWidth) * padding;
                    float scaleY = (stickerHeight / frameHeight) * padding;

                    select.transform.localScale = new Vector3(scaleX, scaleY, 1);
                }


                // 必要なコンポーネント
                obj.AddComponent<BoxCollider2D>();
                obj.AddComponent<RotateTool>();
                obj.AddComponent<Sticker_Manager>();
            }
            else
            {
                // ② PNG が無い → prefab を探す
                GameObject prefab = Resources.Load<GameObject>("Stickers/" + data.prefabName);

                if (prefab != null)
                {
                    Debug.Log($"Prefab を使用してステッカー生成: {data.prefabName}");

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
                    Debug.LogError($"PNG も Prefab も見つかりません: {data.prefabName}");
                    continue;
                }
            }

            // ★ 復元 Sorting Group
            UnityEngine.Rendering.SortingGroup sg = obj.GetComponent<UnityEngine.Rendering.SortingGroup>();
            if (sg == null) sg = obj.AddComponent<UnityEngine.Rendering.SortingGroup>();

            sg.sortingOrder = data.groupOrder;
            sg.sortAtRoot = data.sortAtRoot;

            if (manager != null)
            {
                manager.RegisterLoadLayer(sg);
            }

            // -----------------------------
            // 親の material.color を復元
            // -----------------------------
            SpriteRenderer parentSR = obj.GetComponentInChildren<SpriteRenderer>();
            if (parentSR != null)
            {
                parentSR.material.color = new Color32(data.r, data.g, data.b, data.a);
                parentSR.sortingOrder = data.sortingOrder;
            }

            // -----------------------------
            // 子の SpriteRenderer を復元
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

            // ★ 修正所有子項目的 Tag (排除 SelectSticker)
            foreach (Transform child in obj.transform)
            {
                if (child.name != "SelectSticker")
                {
                    child.tag = "Sticker";
                }
            }

        }
            if (manager != null)
            {
                manager.ApplyOrder();
                Debug.Log("所有圖層已重新排序並恢復控制功能");
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