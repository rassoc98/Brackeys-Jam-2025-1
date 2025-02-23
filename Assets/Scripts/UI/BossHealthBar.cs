using Game.Entity;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
[RequireComponent(typeof(Slider))]
public class BossHealthBar : MonoBehaviour
{
    private Boss _boss;
    private Slider _slider;

    private void Awake()
    {
        _boss = FindFirstObjectByType<Boss>();
        _slider = GetComponent<Slider>();
    }

    private void Update()
    {
        _slider.value = _boss.Health;
    }
}
}
