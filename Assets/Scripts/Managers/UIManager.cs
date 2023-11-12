using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject finishPanel;


    private void Awake() 
    {
        CouplingManager.Instance.AssemblyCompleted+=OnMontageCompleted;
    }

    void Start()
    {
        finishPanel.SetActive(false);
    }

    public void RestartGame()
    {
        CouplingManager.Instance.UpdateMontageStatus(false);
        CouplingManager.Instance.PlacedPartCount=0;
        finishPanel.SetActive(false);
    }

    private void OnMontageCompleted()
    {
        finishPanel.SetActive(true);
    }
}
