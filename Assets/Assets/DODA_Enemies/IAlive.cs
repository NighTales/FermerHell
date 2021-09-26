using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAlive
{
    float Health { get; set; }
    void GetDamage(float value);
    void PlusHealth(float value);
    void Death();
    
    
}
