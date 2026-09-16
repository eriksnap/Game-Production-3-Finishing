using UnityEngine;
using UnityEngine.UI;

public class BoatWorldUI : MonoBehaviour
{
    [Header("World HP Bar")]
    public Slider worldHPSlider;
    public Image worldFillImage;

    [Header("Damage Numbers")]
    public Transform damageSpawnPoint;
    public GameObject damageNumberPrefab;

    [Header("Damage Colour Thresholds")]
    public Color lightHitColour = Color.white;
    public Color mediumHitColour = new Color(1f, 0.65f, 0f); // orange
    public Color heavyHitColour = Color.red;
    public float mediumThreshold = 20f;
    public float heavyThreshold = 40f;

    public void SetHP(float currentHP, float maxHP)
    {
        if (worldHPSlider == null) return;

        worldHPSlider.maxValue = maxHP;
        worldHPSlider.value = currentHP;

        if (worldFillImage != null)
        {
            float fraction = currentHP / maxHP;
            worldFillImage.color = Color.Lerp(Color.red, Color.green, fraction);
        }
    }

    public void ShowDamageNumber(float amount)
    {
        if (damageNumberPrefab == null || damageSpawnPoint == null) return;

        Color colour = lightHitColour;
        if (amount >= heavyThreshold) colour = heavyHitColour;
        else if (amount >= mediumThreshold) colour = mediumHitColour;

        // Instantiate in world space (not parented to the boat) so the
        // number floats independently even as the boat keeps moving.
        GameObject numberObj = Instantiate(damageNumberPrefab, damageSpawnPoint.position, damageSpawnPoint.rotation);
        numberObj.GetComponent<DamageNumber>()?.Initialise(amount, colour);
    }

    public void SetEliminatedVisual()
    {
        if (worldFillImage != null)
            worldFillImage.color = Color.grey;
    }
}