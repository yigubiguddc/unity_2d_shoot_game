public static class GameEvents
{
    //֮���Բ�ʹ��event����Ϊeventֻ���ڱ����е���
    //HealthBar
    public static System.Action<float, float> OnPlayerHealthChange;
    //ExpBar
    public static System.Action<float, float> OnPlayerExpChange;
    //GameOver,to play the sound and show some UI.
    public static System.Action GameOver;
    //Just Pause the whole games;
    public static System.Action GamePause;

    public static System.Action<float> PlayerUpdate;
}

//�Է��ٴ����ǣ�ͬʱ�������ֿռ�Ƚ϶࣬����һ���������ڸ�ʲô��
//����ʹ�õ���c#�е�ί��(delegate)
//System.Action<float,float> ��ʾ���ί�п��Խ�������float���͵Ĳ�����
//ί������������ض��¼�ע�᷽��
//������һ��ί�в��Ҹ����˲���������ζ�Ŷ��ĸ�ί�з���������ƥ���ί�е�ǩ��
//ÿ���漰�����ݸ��µ�ʱ����Ҫ����ί�д��ݵĲ�����~