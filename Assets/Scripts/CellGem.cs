﻿using System;
using DG.Tweening;
using UnityEngine;

public class CellGem : MonoBehaviour {

    public float animDuration = 0.5f;

    public void SetParent(Transform parent) {
        transform.SetParent(parent);
        transform.DOMove(parent.transform.position, animDuration).SetEase(Ease.OutQuint);
    }

    public static bool IsGem(string tag) {
        return Enum.IsDefined(typeof(GemEnum), tag);
    }
}