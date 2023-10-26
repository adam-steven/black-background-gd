/// <summary> Multiplies bullet strength </summary> 
///<param name="bulletStrength">multiplier of the users damage</param>
public partial class DamageMultiplier : UpgradeBtn
{
    internal override void _UniqueCalcOnPress(Player player) 
    {
        int playerBulletStrength = player.BulletStrength;
        int deltaNumber = playerBulletStrength * BulletStrength;
        BulletStrength = deltaNumber;
    }
}
