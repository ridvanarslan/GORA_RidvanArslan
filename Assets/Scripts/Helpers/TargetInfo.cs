using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TargetInfo 
{
    /// <summary>
    /// If a target is in range, this method will return true.
    /// </summary>
    /// <param name="rayPosition"></param>
    /// <param name="rayDirection"></param>
    /// <param name="hitInfo"></param>
    /// <param name="range"></param>
    /// <param name="mask"></param>
    /// <returns></returns>
    public static bool IsTargetInRange(Vector3 rayPosition, Vector3 rayDirection, out RaycastHit hitInfo, float range, LayerMask mask)
    {
        return Physics.Raycast(rayPosition, rayDirection, out hitInfo, range, mask);
    }
}
