using UnityEngine;
using UnityEngine.UI;

public class NextWaveButton : MonoBehaviour
{
    public WaveManager waveManager;
    public Button waveButton;

    private void Start()
    {
        waveButton.onClick.AddListener(StartWave);
    }

    public void StartWave()
    {
        waveManager.StartNextWave();
    }
}