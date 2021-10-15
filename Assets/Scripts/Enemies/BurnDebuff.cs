using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnDebuff : Buff
{
    // Start is called before the first frame update
    public float DOTTime = 1;
    public int DOTDamage = 10;

    public AliveController Controller;
    void Start()
    {
        stateBuff = true;
        Controller = GetComponent<AliveController>();
        StartCoroutine(Timer());
    }
    protected override void ActiveBuff(bool active)
    {
        if (active)
        {
            StartCoroutine(Burn());
        }
    }

    private IEnumerator Burn()//бред
    {
        do
        {
            Controller.Health -= DOTDamage;
            yield return new WaitForSeconds(DOTTime);
        } while (stateBuff );
    }
}
