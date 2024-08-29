public class PlasmaTurret : Turret
{
    protected override void Init()
    {
        name = "Plasma Turret";
        cost = 15;
        range = 5f;
        fireRate = 4f; // Fast speed
        damage = 5;
    }
    
    public override float GetRange()
    {
        return 5f/2;
    }
}
