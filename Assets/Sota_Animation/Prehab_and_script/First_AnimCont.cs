using UnityEngine;

public class First_AnimCont : MonoBehaviour
{
    private Animator animator;
    private int currentAnimationIndex = 0; // 現在のアニメーションインデックス
    private string[] animations = { "wait1", "wait", "First_MID" }; // 再生するアニメーションの順序

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
            // 最初のアニメーションを再生
            PlayNextAnimation();
        }
    }

    void Update()
    {
        // Animator Controllerが正しく再生されている場合、状態を取得
        if (animator != null && animator.isActiveAndEnabled)
        {
            // 現在のアニメーションの状態を取得
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // 現在のアニメーションが終了したら次のアニメーションを再生
            if (stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0))
            {
                PlayNextAnimation();
            }
        }
    }

    // 次のアニメーションを再生するメソッド
    private void PlayNextAnimation()
    {
        if (currentAnimationIndex < animations.Length)
        {
            string animationName = animations[currentAnimationIndex];
            animator.Play(animationName);
            Debug.Log($"Playing animation: {animationName}");
            currentAnimationIndex++;
        }
        else
        {
            Debug.Log("すべてのアニメーションが再生されました。");
        }
    }
}