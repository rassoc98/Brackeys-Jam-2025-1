using System.Collections;
using Audio;
using Game.Entity;

namespace Game
{
public class BossButton : Trigger
{
    protected override IEnumerator HandleTrigger()
    {
        FindFirstObjectByType<Boss>().TakeDamage(1);
        AudioManager.Instance.PlaySound("Explosion");
        Destroy(gameObject);
        yield return null;
    }
}
}
