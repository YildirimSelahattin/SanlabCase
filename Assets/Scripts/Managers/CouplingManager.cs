using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CouplingManager : Singleton<CouplingManager>
{
    public event Action MontageCompleted;
    public event Action MontageStarted;
    private int currentMontageCount = 0;
    private int lastAMontagePartsCount = 1;
    private int lastBMontagePartsCount = 0;
    public Material correctMat;
    public Material incorrectMat;

    public int LastAMontagePartsCount { get { return lastAMontagePartsCount; } set { lastAMontagePartsCount = value; } }
    public int LastBMontagePartsCount { get { return lastBMontagePartsCount; } set { lastBMontagePartsCount = value; } }
    public int CurrentMontageCount
    {
        get { return currentMontageCount; }
        set
        {
            currentMontageCount = value;
            if (currentMontageCount >= 9)
            {
                UpdateMontageStatus(true);
            }
        }
    }

    public void UpdateMontageStatus(bool montageCompleted)
    {
        if (montageCompleted == true)
        {
            MontageCompleted?.Invoke();
        }
        else
        {
            MontageStarted?.Invoke();
        }
    }
}
