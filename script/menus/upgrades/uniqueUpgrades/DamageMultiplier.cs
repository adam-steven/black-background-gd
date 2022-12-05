using Godot;
using System;
using System.Collections.Generic;

/// <summary> Multiplies bullet strength </summary> 
///<param name="bulletStrength">multiplier of the users damage</param>
public class DamageMultiplier : UpgradeBtn
{
    internal override void _UniqueCalcOnPress(Player player) 
    {
        int playerBulletStrength = player.bulletStrength;
        int deltaNumber = playerBulletStrength * bulletStrength;
        bulletStrength = deltaNumber;
    }
}
