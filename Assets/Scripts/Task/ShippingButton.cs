using System;
using UnityEngine;

public class ShippingButton:MonoBehaviour
{

    //依頼の数を取得
    [SerializeField] private RequestTask requestTask;

    //残りの依頼の数を取得
    [SerializeField] private RemainingTask remainingTask;

    //タスク成功数
    private int SuccessTask = 0;

    //タスク失敗数
    private int FailureTask = 0;

    //シール編集エリア
    [SerializeField] private Collider2D StickerArea;

    [SerializeField] private StarGauge starGauge;

    //発送ボタンが押された
    public void OnShippingButton()
    {
        //シール編集エリアにあるシール情報を保存する

        //シール編集エリアにあるシールを全て削除する
        DeleteSticker(StickerArea);

        //依頼の内容を変更する
        requestTask.CompleteTask();

        //残りの依頼の数を減らす
        remainingTask.RemainingTaskCount();

        //成功か失敗か判断して対応するカウントを増やす（未実装）
        SuccessTask++;
        FailureTask++;

        //星のゲージを増やす
        starGauge.OnTaskSuccess();
    }

    //シール編集エリアにあるシールを全て削除する
    private void DeleteSticker(Collider2D StickerArea)
    {
        //シール編集エリア内のオブジェクトを取得
        //引数→center=編集エリア中心座標
        //        size=編集エリアの幅と高さ
        //        　0f=回転角
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            StickerArea.bounds.center,
             StickerArea.bounds.size,0f);

        //範囲内にあるStickerタグを持つオブジェクトを削除する
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Sticker"))
            {
                Destroy(hit.gameObject);
            }
        }

    }
}
