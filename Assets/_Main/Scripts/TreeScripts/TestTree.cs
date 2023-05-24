using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTree : MonoBehaviour
{
    public int life;
    public int bullets;
    public bool spotsEnemy;
    ITreeNode _root;

    public void InitializedTree()
    {
        var dead = new TreeAction(Dead);
        var reload = new TreeAction(Reload);
        var shoot = new TreeAction(Shoot);
        var patrol = new TreeAction(Patrol);
        //var dead = new TreeAction(()=>print("Dead"));
        var hasBullet = new TreeQuestion(HasBullet, shoot, reload);
        var hasLoadedGun = new TreeQuestion(HasBullet, patrol, reload);
        var spotsEnemy = new TreeQuestion(SpotsEnemy, hasBullet, hasLoadedGun);
        var hasLife = new TreeQuestion(Haslife,spotsEnemy,dead);
        _root = hasLife;
    }
    private void Awake()
    {
        InitializedTree();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            _root.Execute();
        }
    }
    public bool Haslife()
    {
        return life > 0;
    }
    public bool HasBullet()
    {
        return bullets > 0;
    }
    public bool SpotsEnemy()
    {
        return spotsEnemy;
    }
    public void Dead()
    {
        print("Dead");    
    }
    public void Reload()
    {
        print("Reload");
    }
    public void Shoot()
    {
        print("Shoot");
    }
    public void Patrol()
    {
        print("Patrol");
    }
}
