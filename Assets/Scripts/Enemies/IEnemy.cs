using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    public void Patrol();

    public void Attack();

    public void FollowPlayer();

    public void ReturnToPatrol();
}
