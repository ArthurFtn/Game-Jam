using UnityEngine;
using UnityEngine.UI;

public class NextWaveButton : MonoBehaviour
{
    public WaveManager waveManager;
    public Button waveButton;

    private void Start()
    {
        if (waveButton == null)
        {
            waveButton = GetComponent<Button>();
        }

        if (waveButton != null)
        {
            waveButton.onClick.AddListener(StartWave);
        }
        else
        {
            Debug.LogError("❌ Aucun bouton trouvé sur l'objet " + gameObject.name);
        }
    }

    public void StartWave()
    {
        waveManager.StartNextWave();
    }
}