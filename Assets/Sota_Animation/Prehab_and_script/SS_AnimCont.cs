using UnityEngine;

public class SS_AnimCont : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Animatorコンポーネントを取得
        animator = GetComponent<Animator>();

        // Animatorが正しく設定されているか確認
        if (animator == null)
        {
            Debug.LogError("Animatorコンポーネントが見つかりません。");
        }
        else
        {
            // 初期アニメーションの再生
            animator.Play("wait2(1)"); // "wait"というアニメーションがAnimator Controllerに設定されている前提
        }
    }

    void Update()
    {
        // Animator Controllerが正しく再生されている場合、状態を取得
        if (animator != null && animator.isActiveAndEnabled)
        {
            // 現在のアニメーションの状態を取得
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // "wait"アニメーションが終了したら次のアニメーションを再生
            if (stateInfo.IsName("wait2(1)") && stateInfo.normalizedTime >= 1.0f)
            {
                animator.Play("ss_Throw(1)"); // "First_MID"が設定されていると仮定
            }
        }
    }
}
