using UnityEditor;
using UnityEngine;

namespace InternalAssets.Scripts.GamePlay.Towers.Editor
{
    [CustomEditor(typeof(TowerSpawnMarker))]
    public class SpawnMarkerEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(TowerSpawnMarker spawner, GizmoType gizmoType)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(spawner.transform.position, 0.2f);
            Gizmos.color = Color.white;
        }
    }
}