using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventInvoker : MonoBehaviour
{
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void InvokeAttackAnimationEvent()
    {
        playerController.AttackAnimationEvent();
    }
}

