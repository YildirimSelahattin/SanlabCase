using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RodBoltController : Parts
{
    public override void MoveTargetWithAnimation()
    {
        transform.DOLocalRotate(new Vector3(0,0,360), 2f, RotateMode.LocalAxisAdd);
        base.MoveTargetWithAnimation();
    }
}
