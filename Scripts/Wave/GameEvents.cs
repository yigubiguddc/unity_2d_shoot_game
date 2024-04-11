public static class GameEvents
{
    //之所以不使用event是因为event只能在本类中调用
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

//以防再次忘记，同时这里文字空间比较多，介绍一下这里是在干什么。
//这里使用的是c#中的委托(delegate)
//System.Action<float,float> 表示这个委托可以接受两个float类型的参数。
//委托允许代码在特定事件注册方法
//当我由一个委托并且给出了参数，这意味着订阅该委托方法都必须匹配该委托的签名
//每次涉及到数据更新的时候都需要更新委托传递的参数。~