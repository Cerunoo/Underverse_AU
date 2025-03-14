using UnityEngine;
using UnityEngine.UI;

public class TransparentNoise : MonoBehaviour
{
    [SerializeField] private float waveSpeed;
    [SerializeField] private float waveFrequency;

    private Image[] noises;
    [SerializeField, Header("Debug")] private float[] transparent;

    private void Start()
    {
        noises = GetComponentsInChildren<Image>();
        transparent = new float[noises.Length];
    }

    private void Update()
    {
        for (int i = 0; i < noises.Length; i++)
        {
            float waveValue = Mathf.Sin((Time.time * waveSpeed + i) * waveFrequency);

            Color color = noises[i].color;
            color.a = Mathf.Clamp01((waveValue + 1f) / 2f);
            noises[i].color = color;

            transparent[i] = color.a;
        }
    }
}