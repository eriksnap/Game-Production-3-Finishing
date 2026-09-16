using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public float floatSpeed = 1.5f;
    public float lifetime = 0.8f;
    public AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    private TextMeshPro tmp;
    private float timer;
    private Color startColor;

    private void Awake()
    {
        tmp = GetComponent<TextMeshPro>();
    }

    public void Initialise(float damageAmount, Color colour)
    {
        tmp.text = Mathf.RoundToInt(damageAmount).ToString();
        tmp.color = colour;
        startColor = colour;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        Color c = startColor;
        c.a = fadeCurve.Evaluate(timer / lifetime);
        tmp.color = c;

        if (timer >= lifetime)
            Destroy(gameObject);
    }
}