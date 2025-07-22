using UnityEngine;

public static  class DamageMultiplierManager
{
    public static bool IsDoubleDamageActive = false;

    public static int Apply(int baseDamage)
    {
        return IsDoubleDamageActive ? baseDamage * 2 : baseDamage;
    }
}
