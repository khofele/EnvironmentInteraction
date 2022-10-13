using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchingCondition : Condition
{
    private CharController charController = null;
    private bool isCrouching = false;

    private void Start()
    {
        charController = GetComponentInParent<CharController>();
    }

    private void Update()
    {
        if(charController.IsCrouching == true)
        {
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }
    }

    public override bool CheckCondition()
    {
        return isCrouching; 
    }
}
