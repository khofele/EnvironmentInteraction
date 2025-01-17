using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistFightInteraction : MultipleOutcomesInteraction
{ 
    private Hands lastHand = Hands.NULL;
    private Hands currentHand = Hands.NULL;

    public Hands CurrentHand { get => currentHand; }

    private void SetLastHand()
    {
        lastHand = currentHand;
    }

    private void Punch()
    {
        interactionManager.IsFighting = true;
        ChoosePunchHand();
        ExecutePunchAnimation();
        animationManager.EnableHeadLayer();
    }

    private IEnumerator WaitAndReset()
    {
        yield return new WaitForSeconds(0.05f);

        interactionManager.SetLastInteraction();
        interactionManager.IsFighting = false;
        isInteractionRunning = false;
        finalIKController.IsIkActive = false;
        isTriggeredByInterruptibleInteraction = false;
        outcomeManager.ResetOutcomes();
    }

    protected override void SetMatchingInteractable()
    {
        matchingInteractable = typeof(Enemy);
    }

    public override void ResetInteraction()
    {
        StartCoroutine(WaitAndReset());
    }

    public override void ExecuteInteraction()
    {
        if (isInteractionRunning == false)
        {
            Punch();
        }

        base.ExecuteInteraction();
        ResetInteraction();
    }

    public void ChoosePunchHand()
    {
        if (lastHand == Hands.NULL)
        {
            int random = Random.Range(1, 10);
            if (random % 2 == 0)
            {
                currentHand = Hands.LEFT;
            }
            else
            {
                currentHand = Hands.RIGHT;
            }
        }
        else if (lastHand == Hands.LEFT)
        {
            currentHand = Hands.RIGHT;
        }
        else if (lastHand == Hands.RIGHT)
        {
            currentHand = Hands.LEFT;
        }

        SetLastHand();
    }

    public void ExecutePunchAnimation()
    {
        int random = Random.Range(1, 4);

        if (currentHand == Hands.LEFT)
        {
            switch (random)
            {
                case 1:
                    animationManager.ExecuteBasicHipPunchLeft();
                    break;

                case 2:
                    animationManager.ExecuteBasicPunchLeft();
                    break;

                case 3:
                    animationManager.ExecuteCrossPunchLeft();
                    break;
            }
        }
        else if (currentHand == Hands.RIGHT)
        {
            switch (random)
            {
                case 1:
                    animationManager.ExecuteBasicHipPunchRight();
                    break;

                case 2:
                    animationManager.ExecuteBasicPunchRight();
                    break;

                case 3:
                    animationManager.ExecuteCrossPunchRight();
                    break;
            }
        }
    }
}
