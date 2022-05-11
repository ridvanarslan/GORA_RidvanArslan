using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public static class CinemachineCameraSwitcher
{
    private static List<CinemachineVirtualCamera> _cameras = new List<CinemachineVirtualCamera>();
    private static CinemachineVirtualCamera activeCamera;

    public static bool IsActiveCamera(CinemachineVirtualCamera camera)
    {
        return camera == activeCamera;
    }
    public static void SwitchCamera(CinemachineVirtualCamera camera)
    {
        camera.Priority = 10;
        activeCamera = camera;
        foreach (CinemachineVirtualCamera cam in _cameras)
        {
            Debug.Log(cam.Name + " " + cam.Priority);
            if (cam != camera)
            {
                cam.Priority = 0;
            }
        }
    }

    public static void RegisterCamera(CinemachineVirtualCamera camera) => _cameras.Add(camera);

    public static void UnRegisterCamera(CinemachineVirtualCamera camera) => _cameras.Remove(camera);
}