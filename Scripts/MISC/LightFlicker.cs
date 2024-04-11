using UnityEngine;


[RequireComponent(typeof(Light))]       //�Զ�������Ҫ�����Էų�ȥ�����ǹ�ѡ

public class LightFlicker : MonoBehaviour
{
    private Light _light;
    public float min = 0.5f;
    public float max = 4.0f;
    private void Awake()
    {
        _light = GetComponent<Light>();

    }
    private void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time, Time.time * 0.5f);
        _light.intensity = Mathf.Lerp(min, max, noise);
    }
}
