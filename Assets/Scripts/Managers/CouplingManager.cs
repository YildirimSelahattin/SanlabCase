using UnityEngine;
using System;
using System.Linq;

public class CouplingManager : Singleton<CouplingManager>
{
    public event Action AssemblyCompleted;
    public event Action AssemblyStarted;
    private int placedPartCount = 0;
    public Material correctMat;
    public Material incorrectMat;
    private GameObject pistonPartParent;

    public int PlacedPartCount
    {
        get { return placedPartCount; }
        set
        {
            placedPartCount = value;
            if (placedPartCount >= pistonPartParent.transform.childCount - 2)
            {
                UpdateMontageStatus(true);
            }
        }
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
