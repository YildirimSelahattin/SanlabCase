using UnityEngine;
using System;
using System.Linq;
using Unity.VisualScripting;

public class CouplingManager : Singleton<CouplingManager>
{
    public event Action AssemblyCompleted;
    public event Action AssemblyStarted;
    private int placedPartCount = 0;
    public Material correctMat;
    public Material incorrectMat;
    public GameObject pistonPartParent;
    private int totalPartCount;

    public int PlacedPartCount
    {
        get { return placedPartCount; }
        set
        {
            placedPartCount = value;
            if (placedPartCount >= totalPartCount - 1)
            {
                UpdateMontageStatus(true);
            }
        }
    }

    void Start()
    {
        totalPartCount = pistonPartParent.transform.childCount;
    }

    public void UpdateMontageStatus(bool montageCompleted)
    {
         if (montageCompleted == true)
        {
            AssemblyCompleted?.Invoke();
        }
        else
        {
            AssemblyStarted?.Invoke();
        }
    }
}
