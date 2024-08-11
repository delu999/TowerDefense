public class VulcanTurret : Turret
{
    void Start()
    {
        name = "Vulcan Turret";
        cost = 5;
        range = 4f;
        fireRate = 1f; // Slow speed
        damage = 10;
    }
}
