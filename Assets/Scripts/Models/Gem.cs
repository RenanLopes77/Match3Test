using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GemEnum {
  Apple,
  Bread,
  Broccoli,
  Coconut,
  Milk,
  Orange,
  Star,
}

[System.Serializable]
public class Gem {
  public Sprite sprite;
  public GemEnum gemType;
  public float pointsMultiplier;
}