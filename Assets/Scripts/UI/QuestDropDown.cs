using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDropDown : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private bool active;

    public void ToggleStatus()
    {
        active = !active;
        PlayAnimation(active);
    }


    public void PlayAnimation(bool active)
    {
        if (animator == null) return;

        animator.SetBool("status", active);

    }

}
