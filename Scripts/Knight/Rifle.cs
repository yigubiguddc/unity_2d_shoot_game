using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bronya;

public class Rifle : Gun
{

    protected override void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x < transform.position.x)
            transform.localScale = new Vector3(flipY, -flipY, 1);
        else
            transform.localScale = new Vector3(flipY, flipY, 1);
        Shoot();
    }

    protected override void Fire()
    {
        SoundManager.Instance.PlaySound("Assault Shoot");
        animator.SetTrigger("Shoot");
        RaycastHit2D hit2D = Physics2D.Raycast(muzzlePos.position, direction, 30000);

        if(hit2D.transform.GetComponent<IDamagable>() != null)
        {
            hit2D.transform.GetComponent<IDamagable>().TakeDamage(10);
            Popuptext.Creat(hit2D.point,10,true);
        }        
        //…‰œﬂ
        GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
        LineRenderer tracer = bullet.GetComponent<LineRenderer>();
        tracer.SetPosition(0, muzzlePos.position);
        tracer.SetPosition(1, hit2D.point);
        //µØø«
        GameObject shell = ObjectPool.Instance.GetObject(shellPrefab);
        shell.transform.position = shellPos.position;
        shell.transform.rotation = shellPos.rotation;
    }
}
