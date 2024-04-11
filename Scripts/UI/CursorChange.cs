using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorChange : MonoBehaviour
{
    public Texture2D cursorTexture1; 
    public Texture2D cursorTexture2;


    private void Start()
    {
        SwitchToCursor1();
    }

    private void Update()
    {
        // 举个例子，当按下空格键时，切换鼠标图片
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchToCursor1();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchToCursor2();
        }
    }

    public void SwitchToCursor1()
    {
        Cursor.SetCursor(cursorTexture1, Vector2.zero, CursorMode.Auto);
    }

    public void SwitchToCursor2()
    {
        Cursor.SetCursor(cursorTexture2, Vector2.zero, CursorMode.Auto);
    }
}
