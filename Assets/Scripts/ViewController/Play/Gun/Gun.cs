using QFramework;
using ShootGame;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IGun
{
    string gunName { get; set; }
    void Shoot(Vector2 shootPoDir);
}

public enum GunState
{
    Idel,
    Shooting,
    CoolDown,
}
public class GunInfo
{
    public BindableProperty<int> bulletCount { get; set; }
    public BindableProperty<string> name { get; set; }
    public BindableProperty<GunState> gunState { get; set; }
}

public abstract class Gun :ViewController,IGun
{
    [SerializeField]private Animator gunAnim;

    public IGunSystem gunSystem;
    public GunInfo gunInfo;

    public float attackRate;//¹¥»÷ÆµÂÊ
    public float curAttackRate;
    public string gunName { get; set; }

    protected virtual void Awake()
    {
        gunSystem = this.GetSystem<IGunSystem>();
        gunName = GetType().Name;

        gunAnim = GetComponent<Animator>();
    }
    private void Update()
    {
    }
    protected abstract void GunShoot(Vector2 shootDir);
    
    public virtual void Shoot(Vector2 shootPoDir)
    {
        
        GunShoot(shootPoDir);
        this.SendCommand(ShootCommand.Single);
    }
    
}

