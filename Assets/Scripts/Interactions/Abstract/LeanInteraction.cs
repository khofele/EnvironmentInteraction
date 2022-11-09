using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LeanInteraction : Interaction
{
    protected Collider snapCollider = null;
    protected Collider playerCollider = null;
    protected Leanable currentLeanableObject = null;
    protected float snapDistance = 0.5f;
    protected Vector3 offset;

    protected virtual void LeanOnObject()
    {
        Vector3 playerClosestPoint = playerCollider.ClosestPoint(snapCollider.transform.position);
        Vector3 objectClosestPoint = snapCollider.ClosestPoint(playerClosestPoint);
        offset = objectClosestPoint - playerClosestPoint;

        if (offset.magnitude < snapDistance)
        {
            interactionManager.IsLeaningSnapping = true;
            charController.transform.position += offset;
            finalIKController.IsIkActive = true;
            SetLeanBool(true);
            ExecuteAnimation();
        }
    }

    protected virtual void DetectObject()
    {
        Ray rayFront = new Ray(charController.transform.position, charController.transform.forward);
        Ray rayBack = new Ray(charController.transform.position, -charController.transform.forward);
        Ray rayRight = new Ray(charController.transform.position, charController.transform.right);
        Ray rayLeft = new Ray(charController.transform.position, -charController.transform.right);

        RaycastHit hit;

        if (((Physics.Raycast(rayFront, out hit, 1f)) || (Physics.Raycast(rayBack, out hit, 1f)) || (Physics.Raycast(rayRight, out hit, 1f)) || (Physics.Raycast(rayLeft, out hit, 1f)))
            && CheckLeaningBool())
        {
            if (hit.transform.gameObject.GetComponent<Interactable>() != null)
            {
                charController.transform.rotation = Quaternion.LookRotation(hit.normal);
            }
        }
        else
        {
            charController.transform.rotation = Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized, Camera.main.transform.up);
            isInteractionRunning = false;
            StopAnimation();
        }
    }

    protected override void Start()
    {
        base.Start();
        playerCollider = charController.GetComponent<CharacterController>();
    }

    protected override void Update()
    {
        if (CheckTrigger() == true)
        {
            if (CheckMatchingInteractable() == true)
            {
                if (CheckConditions() == true)
                {
                    ExecuteInteraction();
                }
            }
        }

        ExecuteLeanInteraction();
    }

    protected virtual void ExecuteLeanInteraction()
    {
        if (isInteractionRunning == true)
        {
            if(Vector3.Distance(charController.transform.position, snapCollider.ClosestPoint(charController.transform.position)) > 0.3f)
            {
                LeanOnObject();
            }
            else
            {
                interactionManager.IsLeaningSnapping = false;
            }

            DetectObject();

            if (CheckTerminationCondition())
            {
                ResetInteraction();
            }
        }
    }

    protected virtual void ResetValues()
    {
        isInteractionRunning = false;
        finalIKController.IsIkActive = false;
    }

    protected override void ExecuteInteraction()
    {
        if (isInteractionRunning == false)
        {
            interactionManager.IsLeaningSnapping = true;
        }

        base.ExecuteInteraction();
        currentLeanableObject = (Leanable)interactableManager.CurrentInteractable;
        snapCollider = currentLeanableObject.SnapCollider;
        LeanOnObject();
    }

    protected override void ResetInteraction()
    {
        base.ResetInteraction();
        ResetCharacter();
        currentLeanableObject = null;
        ResetValues();
        StopAnimation();
        interactionManager.IsLeaningSnapping = false;
    }

    protected abstract void ExecuteAnimation();
    protected abstract void StopAnimation();
    protected abstract bool CheckTerminationCondition();
    protected abstract void ResetCharacter();
    protected abstract void SetLeanBool(bool value);
    protected abstract bool CheckLeaningBool();
}
