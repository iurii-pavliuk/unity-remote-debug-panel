using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.LowLevel;

[Serializable]
public class MySystem
{
    public string name;
    public bool isEnabled;
    public MySystem[] subSystems;

    public static implicit operator MySystem(PlayerLoopSystem loopSystem)
    {
        var result = new MySystem
        {
            name = loopSystem.type == null ? "Unknown" : loopSystem.type.Name,
            isEnabled = true,
            // subSystems = new MySystem[1];//loopSystem.subSystemList.Length]
        };
        // for (var i = 0; i < result.subSystems.Length - 1; i++)
        // {
        //     result.subSystems[i] = loopSystem.subSystemList[i];
        // }

        if (loopSystem.subSystemList is { Length: > 0 })
        {
            result.subSystems = new MySystem[] { loopSystem.subSystemList[0] };
        }

        return result;
    }
}

public class PlayerLoopManager : MonoBehaviour
{
    public string firstName;
    [SerializeField] private int year;

    private PlayerLoopSystem _defaultLoop;

    // what is ProcessRemoteInput?
    // what is VFXUpdate?

    // public MySystem[] systems;
    private Dictionary<string, bool> _systemsStateByName;

    void Start()
    {
        _defaultLoop = PlayerLoop.GetDefaultPlayerLoop();
        _systemsStateByName = new Dictionary<string, bool>();

        foreach (var system in PlayerLoopData.SystemsClusters.SelectMany(cluster => cluster.Value))
        {
            _systemsStateByName.Add(system, true);
        }

        EnableCluster(PlayerLoopData.NotUsed, false);
        UpdatePlayerLoop();
        // var result = TestUnused(PlayerLoop.GetCurrentPlayerLoop());
        // Debug.Log($"Unused test: {result}");
        // LogPlayerLoopSystem(PlayerLoop.GetCurrentPlayerLoop(), "");
    }
   
    private bool TestUnused(PlayerLoopSystem playerLoop)
    {
        if (playerLoop.subSystemList is { Length: > 0 })
        {
            if (playerLoop.subSystemList.Any(subSystem => TestUnused(subSystem) == false))
            {
                return false;
            }
        }
        else
        {
            var sName = playerLoop.type.Name;
            var s = PlayerLoopData.SystemsClusters[PlayerLoopData.NotUsed];
            return !Array.Exists(s, e => e.Equals(sName));
        }

        return true;
    }

    private void UpdatePlayerLoop()
    {
        var newPlayerLoop = _defaultLoop;
        CopyPlayerLoop(ref newPlayerLoop, ref _defaultLoop);
        PlayerLoop.SetPlayerLoop(newPlayerLoop);
    }

    private void CopyPlayerLoop(ref PlayerLoopSystem sourcePlayerLoop, ref PlayerLoopSystem targetPlayerLoop)
    {
        if (sourcePlayerLoop.subSystemList == null || sourcePlayerLoop.subSystemList.Length == 0)
            return;

        targetPlayerLoop.subSystemList = new PlayerLoopSystem[] { };
        for (var i = 0; i < sourcePlayerLoop.subSystemList.Length; i++)
        {
            var system = sourcePlayerLoop.subSystemList[i];
            var systemName = system.type == null ? "Unknown" : system.type.Name;
            var isEnabled = _systemsStateByName.ContainsKey(systemName) == false || _systemsStateByName[systemName];
            if (isEnabled)
            {
                targetPlayerLoop.subSystemList = targetPlayerLoop.subSystemList
                    .Concat(sourcePlayerLoop.subSystemList.AsSpan(i, 1).ToArray()).ToArray();

                CopyPlayerLoop(ref targetPlayerLoop.subSystemList[^1], ref sourcePlayerLoop.subSystemList[i]);
            }
        }
    }

    public void EnableCluster(string clusterName, bool isEnabled)
    {
        foreach (var systemName in PlayerLoopData.SystemsClusters[clusterName])
        {
            _systemsStateByName[systemName] = isEnabled;
        }
    }

    private void LogPlayerLoopSystem(PlayerLoopSystem loop, string prefix)
    {
        Debug.Log($"{prefix} {(loop.type == null ? "Null" : loop.type.Name)}");
        if (loop.subSystemList != null && loop.subSystemList.Length > 0)
        {
            foreach (var subSystem in loop.subSystemList)
            {
                LogPlayerLoopSystem(subSystem, prefix + '\t');
            }
        }
    }
}

public abstract class PlayerLoopData
{
    public const string NotUsed = "NotUsed";
    public const string Animators = "Animators";
    public const string Physics = "Physics";
    public const string Audio = "Audio";
    public const string Animations = "Animations";
    public const string Key = "UI";
    public const string AI = "AI";
    public const string Particles = "Particles";
    public const string Rendering = "Rendering";
    public const string SkinnedMeshes = "SkinnedMeshes";

    public static readonly Dictionary<string, string[]> SystemsClusters = new()
    {
        {
            Animators,
            new[]
            {
                "DirectorSampleTime", "DirectorFixedUpdate", "DirectorFixedSampleTime",
                "DirectorFixedUpdatePostPhysics", "DirectorUpdate",
                "DirectorUpdateAnimationBegin", "DirectorUpdateAnimationEnd", "DirectorDeferredEvaluate",
                "DirectorLateUpdate", "DirectorRenderImage"
            }
        },
        {
            Physics,
            new[]
            {
                "PhysicsResetInterpolatedTransformPosition", "PhysicsFixedUpdate", "Physics2DFixedUpdate",
                "PhysicsClothFixedUpdate", "PhysicsUpdate",
                "Physics2DUpdate",
                "Physics2DLateUpdate", "PhysicsSkinnedClothBeginUpdate", "PhysicsSkinnedClothFinishUpdate"
            }
        },
        { Audio, new[] { "AudioFixedUpdate", "UpdateAudio" } },
        { Animations, new[] { "LegacyFixedAnimationUpdate", "LegacyAnimationUpdate" } },
        {
            NotUsed,
            new[]
            {
                "XREarlyUpdate", "XRUpdate", "UpdateKinect", "ARCoreUpdate", "XRFixedUpdate", "XRPostLateUpdate",
                "XRPreEndFrame", "WindUpdate"
            }
        },
        {
            Key,
            new[]
            {
                "UpdateCanvasRectTransform", "UpdateRectTransform", "PlayerUpdateCanvases", "PlayerEmitCanvasGeometry"
            }
        },
        { AI, new[] { "AIUpdate", "AIUpdatePostScript" } },
        { Particles, new[] { "ParticleSystemBeginUpdateAll", "ParticleSystemEndUpdateAll" } },
        {
            Rendering,
            new[]
            {
                "UpdateAllRenderers", "UpdateLightProbeProxyVolumes", "EnlightenRuntimeUpdate", "SortingGroupsUpdate",
                "FinishFrameRendering", "BatchModeUpdate",
            }
        },
        { SkinnedMeshes, new[] { "UpdateAllSkinnedMeshes" } }
    };
}