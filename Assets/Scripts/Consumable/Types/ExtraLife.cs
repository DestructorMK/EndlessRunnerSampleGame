using UnityEngine;
using System;
using System.Collections;

public class ExtraLife : Consumable
{
    protected const int k_CoinValue = 10;

    public override string GetConsumableName()
    {
        return "Life";
    }

    public override ConsumableType GetConsumableType()
    {
        return ConsumableType.EXTRALIFE;
    }

    public override int GetPrice()
    {
        return 2000;
    }

	public override int GetPremiumCost()
	{
		return 5;
	}

    public override bool CanBeUsed(CharacterInputController c)
    {
        if (c.copDistance >= c.maxCopDistance)
            return false;

        return true;
    }

    public override IEnumerator Started(CharacterInputController c)
    {
        yield return base.Started(c);
        if (c.copDistance < c.maxCopDistance)
            c.copDistance += c.obstacleHitPenalty;
		else
            c.coins += k_CoinValue;
    }
}
