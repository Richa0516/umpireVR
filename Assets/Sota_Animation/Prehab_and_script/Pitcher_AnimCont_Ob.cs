using UnityEngine;

public class Pitcher_AnimCont : MonoBehaviour
{
    private Animator animator;
    private float PitcherThrow_AnimStartTime;//PitcherがPitcher_Throwを始めてから計測
    private float Throw_AnimTime;
    public bool BallMove_Flag;//ボールがピッチャーから離れていいとき、trueになる



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
            animator.Play("Pitcher_Throw"); // "wait"というアニメーションがAnimator Controllerに設定されている前提
        }
    }

    void Update()
    {
        //Throw_AnimTime = Time.realtimeSinceStartup - PitcherThrow_AnimStartTime;
        //if (Throw_AnimTime >= 5.04f){
        //    BallMove_Flag = true;
        //}

        // Animator Controllerが正しく再生されている場合、状態を取得
        if (animator != null && animator.isActiveAndEnabled)
        {
            // 現在のアニメーションの状態を取得
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // "wait"アニメーションが終了したら次のアニメーションを再生
            if (stateInfo.IsName("Pitcher_Throw") && stateInfo.normalizedTime >= 1.0f)
            {
                animator.Play("Disapointment"); // "First_MID"が設定されていると仮定
            }
        }
    }
}
