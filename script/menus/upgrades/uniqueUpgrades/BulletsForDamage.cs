using Godot;
using System;
using System.Collections.Generic;

/// <summary> Adds/Deletes a percentage of the users bullet, add number to damage </summary> 
///<param name="noOfBullets">multiplier of the users bullet (-neg deletes bullets)</param>
///<param name="bulletStrength">UNUSED</param>
public class BulletsForDamage : UpgradeBtn
{
    internal override void _UniqueCalcOnPress(Player player) 
    {
        int playerBulletNo = player.noOfBullets;
        int deltaNumber = playerBulletNo * noOfBullets;
        noOfBullets = deltaNumber;
        bulletStrength = -deltaNumber * 5;
    }
}
