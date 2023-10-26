/// <summary> Adds/Deletes a percentage of the users bullet, add number to damage </summary> 
///<param name="noOfBullets">multiplier of the users bullet (-neg deletes bullets)</param>
///<param name="bulletStrength">UNUSED</param>
public partial class BulletsForDamage : UpgradeBtn
{
    internal override void _UniqueCalcOnPress(Player player) 
    {
        int playerBulletNo = player.NoOfBullets;
        int deltaNumber = playerBulletNo * this.NoOfBullets;
        this.NoOfBullets = deltaNumber;
        this.BulletStrength = -deltaNumber * 5;
    }
}