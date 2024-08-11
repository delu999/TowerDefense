public class PlasmaTurret : Turret
{
    protected override void Init()
    {
        name = "Plasma Turret";
        cost = 15;
        range = 5f;
        fireRate = 0.5f; // Fast speed
        damage = 5;
    }
}
