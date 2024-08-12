public class VulcanTurret : Turret
{
    protected override void Init()
    {
        name = "Vulcan Turret";
        cost = 5;
        range = 4f;
        fireRate = 1f; // Slow speed
        damage = 10;
    }

    public override float GetRange()
    {
        return 4f/2;
    }
}
