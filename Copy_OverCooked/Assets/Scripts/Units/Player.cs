using UnityEngine;
using System.Collections;

public enum EAnimationType
{
    Dash,
    Chop,
    

} 

public class Player : MonoBehaviour
{
    [Header("Information")]
    [SerializeField]
    private int Id;

    private Rigidbody rigid;
    private Vector3 moveDirection;

    [Header("Stat")]
    [SerializeField]
    private float Speed;

    [SerializeField]
    private float dashSpeed;
    private float applyDashSpeed;

    // 몇초 동안 대쉬를 할 것인가
    [SerializeField]
    private float dashTime;
    private WaitForSeconds dashTimeWaitForsecond;

    // 대쉬 쿨타임
    [SerializeField]
    private float dashCoolDownTime;
    private WaitForSeconds dashDelayWaitForSecond;

    private bool isDashable;


    private Hand hand;
    private Animator animator;    


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        applyDashSpeed = 1f;

        dashTimeWaitForsecond = new WaitForSeconds(dashTime);
        dashDelayWaitForSecond = new WaitForSeconds(dashCoolDownTime);

        isDashable = true;

        hand = GetComponentInChildren<Hand>();
        hand.SetPlayer(this);

        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + moveDirection * Speed * applyDashSpeed * Time.deltaTime);
    }

    public Interactor GetInteractor()
    {
        return hand.interactor;
    }

    public void SetBoolAnimation(EAnimationType animationType, bool condition)
    {
        switch (animationType)
        {
            case EAnimationType.Dash:
                break;
            case EAnimationType.Chop:
                transform.Find("ChoppingArm").gameObject.SetActive(condition);
                break;
        }

        animator.SetBool($"Is{animationType}", condition);
    }


    public void SetMoveDirection(Vector3 moveDirection)
    {
        this.moveDirection = moveDirection;
        transform.LookAt(transform.position + moveDirection);
    }


    public void GrabAndPut()
    {
        hand.GrabAndPut();
    }

    public void OnInteractAndThrow()
    {
        hand.InteractAndThorw();
    }

    public void Dash()
    {
        if (isDashable)
        {
            StartCoroutine(DashCoroutine());
            StartCoroutine(CoolDownDash());
        }
    }

    private IEnumerator DashCoroutine()
    {
        isDashable = false;
        //animator.SetBool("IsDash", true);
        applyDashSpeed = dashSpeed;
        yield return dashTimeWaitForsecond;

        applyDashSpeed = 1f;
        //animator.SetBool("IsDash", false);
    }

    private IEnumerator CoolDownDash()
    {
        yield return dashDelayWaitForSecond;

        isDashable = true;
    }
}
