using UnityEngine;

public class ScaleTool : MonoBehaviour
{
    [SerializeField] private Select select;
    [SerializeField] private float changeScale = 0.2f;

    public void ScaleUp()
    {
        Scale(1f + changeScale);
    }

    public void ScaleDown()
    {
        Scale(1f / (1f + changeScale));
    }

    private void Scale(float factor)
    {
        if (select == null) return;
        if (select.targetObject == null) return;

        Transform target = select.targetObject;

        // Åö Ç±Ç±Ç≈ñàâÒéÊìæÇ∑ÇÈ
        ScaleLimit limit = target.GetComponent<ScaleLimit>();
        if (limit == null)
        {
            Debug.LogWarning($"{target.name} Ç… ScaleLimit Ç™ïtÇ¢ÇƒÇ¢Ç‹ÇπÇÒ", target);
            return;
        }

        Vector3 nextScale = target.localScale * factor;

        target.localScale = new Vector3(
            Mathf.Clamp(nextScale.x, limit.minScale.x, limit.maxScale.x),
            Mathf.Clamp(nextScale.y, limit.minScale.y, limit.maxScale.y),
            Mathf.Clamp(nextScale.z, limit.minScale.z, limit.maxScale.z)
        );
    }
}
