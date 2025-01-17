using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpOverObstacleInteraction : Interaction
{
    private void Jump()
    {
        animationManager.ExecuteJumpAnimation();
        animationManager.EnableArmsLayer();
        interactionManager.IsJumping = true;
    }

    private IEnumerator WaitAndReset()
    {
        yield return new WaitForSeconds(1f);
        base.ResetInteraction();
        interactionManager.IsJumping = false;
        animationManager.StopJumpAnimation();
        animationManager.DisableArmsLayer();
    }

    private void Reset()
    {
        StartCoroutine(WaitAndReset());
    }

    protected override void SetMatchingInteractable()
    {
        matchingInteractable = typeof(Jumpable);
    }

    protected override void Update()
    {
        if (CheckTrigger() == true)
        {
            if (CheckMatchingInteractable() == true)
            {
                if (CheckOtherInteractionsRunning() == true)
                {
                    if (CheckConditions() == true)
                    {
                        ExecuteInteraction();
                        Reset();
                    }
                    else if (isTriggeredByInterruptibleInteraction == true)
                    {
                        ExecuteInteraction();
                        Reset();
                    }
                }
            }
        }

        if (interactionManager.IsJumping == true)
        {
            finalIKController.IsIkActive = true;
            charController.transform.position += new Vector3(charController.MoveDirection.x, 0, charController.MoveDirection.z).normalized * Time.deltaTime * 2;
            charController.transform.position += new Vector3(Camera.main.transform.forward.x * 2, 0.2f, Camera.main.transform.forward.z * 2).normalized * Time.deltaTime * 2;
        }
    }

    public override void ExecuteInteraction()
    {
        base.ExecuteInteraction();
        Jump();
    }
}
