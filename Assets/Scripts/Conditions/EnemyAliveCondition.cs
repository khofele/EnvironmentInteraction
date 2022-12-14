using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAliveCondition : Condition
{
    private Enemy enemy = null;
    private bool isEnemyAlive = true;

    public override bool CheckCondition()
    {
        return isEnemyAlive;
    }

    private void Update()
    {
        if(interactableManager.CurrentInteractableParent != null)
        {
            if(interactableManager.CurrentInteractableParent.GetComponentInChildren<Enemy>() != null)
            {
                enemy = (Enemy)interactableManager.CurrentInteractableParent.GetComponentInChildren<Enemy>();

                if (enemy.IsDead == false)
                {
                    isEnemyAlive = true;
                }
                else
                {
                    isEnemyAlive = false;
                }
            }
            else
            {
                isEnemyAlive = false;
            }
        }
    }
}
