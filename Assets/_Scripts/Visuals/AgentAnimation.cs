using System;
using UnityEngine;

public class AgentAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public event Action OnAction;
    public void PlayAnimation(string animationName)
        => animator.Play(animationName);

    public void ActionEvent()
    {
        OnAction?.Invoke();
        OnAction = null;
    }
}
