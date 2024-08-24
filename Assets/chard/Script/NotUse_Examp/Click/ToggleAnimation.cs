using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClickHandler : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    public GameObject objectB;
    private Animator animator;
    

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            Debug.LogError("MeshRenderer not found on the object.");
        }

        if (objectB == null)
        {
            Debug.LogError("ObjectB not assigned in the Inspector.");
            return;
        }

        animator = objectB.GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found on ObjectB.");
        }
    }

    private void OnMouseDown()
    {

        if (animator != null)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Appear"))
            {
                animator.SetTrigger("TriggerAppear");
            }
            else
            {
                animator.SetTrigger("TriggerDisappear");
            }
        }
    }
}