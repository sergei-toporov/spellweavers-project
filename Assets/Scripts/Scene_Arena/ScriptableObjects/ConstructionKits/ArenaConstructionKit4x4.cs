using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 4x4 arena construction kit scriptable object.
/// </summary>
[CreateAssetMenu(fileName = "new_arena_construction_kit", menuName = "Custom Assets/Construction Kits/Prefabs: 4x4", order = 51)]
public class ArenaConstructionKit4x4 : ArenaConstructionKit
{
    [SerializeField] protected List<Transform> walls;
    public List<Transform> Walls { get => walls; }

    [SerializeField] protected List<Transform> floors;
    public List<Transform> Floors { get => floors; }

    [SerializeField] protected List<Transform> obstacles;
    public List<Transform> Obstacles { get => obstacles; }
}
