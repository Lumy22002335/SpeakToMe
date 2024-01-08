using TMPro;
using UnityEngine;

public class FrequencyUpdate : MonoBehaviour
{
    private readonly int minFrequency = 880;
    private readonly int maxFrequency = 1080;

    private TMP_Text frequencyText;

    private int currentFrequency = 880;
    private float timer = 0.0f;

    private void Start()
    {
        frequencyText = GetComponent<TMP_Text>();
        currentFrequency = Random.Range(minFrequency, maxFrequency);
        frequencyText.text = currentFrequency.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 0.2f)
        {
            timer = 0.0f;
            currentFrequency++;
            if (currentFrequency > maxFrequency)
            {
                currentFrequency = minFrequency;
            }

            frequencyText.text = currentFrequency.ToString();
        }
    }
}
