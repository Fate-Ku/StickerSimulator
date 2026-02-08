using UnityEngine;

public class ScaleTool : MonoBehaviour
{
    [SerializeField] private Select select;
    [SerializeField] private float changeScale = 0.2f;

    public void ScaleUp()
    {
        //1.2倍
        Scale(1f + changeScale);
    }

    public void ScaleDown()
    {
        //0.83倍(1f + changeScale)は元の大きさに戻せるようにするため
        Scale(1f / (1f + changeScale));
    }

    //magnification→倍率
    private void Scale(float magnification)
    {
        if (select == null) return;
        if (select.targetObject == null) return;

        Transform target = select.targetObject;

        //シールの大小の上限を取得
        ScaleLimit limit = target.GetComponent<ScaleLimit>();

        //現在の大きさから倍率をかけて大きさを変化する
        Vector3 nextScale = target.localScale * magnification;

        //Mathf.Clamp(値, 最小値, 最大値)
        //値を設定した大小の上限より大きく（小さく）ならないようにする
        target.localScale = new Vector3(
            Mathf.Clamp(nextScale.x, limit.minScale.x, limit.maxScale.x),
            Mathf.Clamp(nextScale.y, limit.minScale.y, limit.maxScale.y),
            Mathf.Clamp(nextScale.z, limit.minScale.z, limit.maxScale.z)
        );
    }
}