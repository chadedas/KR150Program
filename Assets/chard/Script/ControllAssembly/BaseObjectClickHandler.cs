using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObjectClickHandler : MonoBehaviour
{
    protected Animator animator;
    protected bool isActive = false;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }
    }

    public void Activate()
    {
        isActive = true;
        Debug.Log("Activated: " + gameObject.name);
    }

    private void OnMouseDown()
    {
        Debug.Log("Click detected");
        if (isActive && animator != null)
        {
            Debug.Log("Animator is not null");
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            Debug.Log("Current state: " + stateInfo.shortNameHash);
            if (stateInfo.IsName("Appear"))
            {
                Debug.Log("Animator state is Appear");

                animator.SetTrigger("TriggerDisappear");
                OnDisappear();
            }
            else
            {
                Debug.Log("Animator state is not Appear");
                animator.SetTrigger("TriggerAppear");
                OnAppear();
            }
        }
        else
        {
            Debug.LogWarning("Either isActive is false or Animator is null");
        }
    }

    protected virtual void OnAppear() { }
    protected virtual void OnDisappear() { }
}
