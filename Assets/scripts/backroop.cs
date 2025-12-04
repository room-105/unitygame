using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class backroop : MonoBehaviour
{
    [SerializeField] private Vector2 m_offsetSpeed = new Vector2(0.05f, 0f);

    private Material m_copiedMaterial;//マテリアル：オブジェクトの見た目決めるもの、UIも使う、マテリアルはunity内で使いまわすのでスクリプトで変数宣言してコピーしないと他のUIに支障をきたす
    private const float k_maxLength = 1f;//constは定数＝変わらない値、一度決めたら変更できない、privateはこのクラス内で使うよ、1fはrepeatの上限
    private const string k_propName = "_MainTex";

    void Start()
    {
        var image = GetComponent<Image>();//varは型を勝手に判断してくれる、本来はImaga imageでもいい
        // マテリアルを明示的にコピーして独立させる（共有しない）
        m_copiedMaterial = new Material(image.material);
        image.material = m_copiedMaterial;
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;//Time.timescaleは時間の進む速さ、一時停止は０なのでループは停止

        var x = Mathf.Repeat(Time.time * m_offsetSpeed.x, k_maxLength);
        var y = Mathf.Repeat(Time.time * m_offsetSpeed.y, k_maxLength);
        var offset = new Vector2(x, y);
        m_copiedMaterial.SetTextureOffset(k_propName, offset);
    }

    void OnDestroy()
    {
        if (m_copiedMaterial != null)
        {
            Destroy(m_copiedMaterial);
        }
    }
}
