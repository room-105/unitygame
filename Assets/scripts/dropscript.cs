using UnityEngine;

[System.Serializable]
public class dropscript : MonoBehaviour
{
    public string itemId;
    public string itemName;
    [Range(0f, 1f)] public float dropRate; // 0.1 = 10% ‚ÌŠm—¦
}

