using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Actions
{
    public static Action<GameObject> SalvagedWeapon;
    public static Action<EnemyHealth> EnemyKilled;
    public static Action<Skill> ActivatedSkill;
    public static Action ActivatedDodge;
}
