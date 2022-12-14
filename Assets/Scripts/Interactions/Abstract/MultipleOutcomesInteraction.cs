using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OutcomeManager))]
public abstract class MultipleOutcomesInteraction : Interaction
{
    protected OutcomeManager outcomeManager = null;

    public OutcomeManager OutcomeManager { get => outcomeManager; }

    protected override void Start()
    {
        base.Start();

        outcomeManager = GetComponent<OutcomeManager>();
        outcomeManager.FillOutcomesList();
    }

    protected override void Update()
    {
        if(outcomeManager.CurrentOutcome == null)
        {
            if (CheckTrigger() == true)
            {
                if (CheckMatchingInteractable() == true)
                {
                    if (CheckOtherInteractionsRunning() == true)
                    {
                        if (CheckConditions() == true)
                        {
                            interactionManager.CurrentInteraction = this;
                            SetCurrentInteractable();
                            interactionManager.SetLastInteraction();

                            if (CheckAndExecuteOutcomes() == false)
                            {
                                ExecuteInteraction();
                                ResetInteraction();
                            }
                            else
                            {
                                interactionManager.CurrentInteraction = this;
                                ResetInteraction();
                            }
                        }
                        else if (isTriggeredByInterruptibleInteraction == true)
                        {
                            ExecuteInteraction();
                        }
                    }
                }
            }
        }
    }

    private bool CheckAndExecuteOutcomes()
    {
        foreach (Outcome outcome in outcomeManager.Outcomes)
        {
            if (outcome.CheckConditions() == true)
            {
                outcome.ExecuteOutcome();
                return true;
            }
        }

        return false;
    }
}
