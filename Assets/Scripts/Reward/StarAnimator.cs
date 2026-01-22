using UnityEngine;

public class StarAnimator : MonoBehaviour
{
    private Animator anim;
    private int currentStar = -1;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetStarCount(int count)
    {
        count = Mathf.Clamp(count, 0, 3);

        if (count == currentStar) return; 

        currentStar = count;
        anim.SetInteger("StarCount", count);
    }
}
