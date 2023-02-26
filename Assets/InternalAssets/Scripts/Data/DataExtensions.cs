using UnityEngine;

namespace InternalAssets.Scripts.Data
{
    public static class DataExtensions
    {
        public static Vector3Data AsVectorData(this Vector3 vector) => 
            new Vector3Data(vector.x, vector.y, vector.z);

        public static Vector3 AsUnityVector(this Vector3Data vector3Data) =>
            new Vector3(vector3Data.X, vector3Data.Y, vector3Data.Z);

        public static Vector3 AddY(this Vector3 vector, float y)
        {
            vector.y += y;
            return vector;
        }
        
        public static string ToJson(this object obj) =>
            JsonUtility.ToJson(obj);

        public static T ToDeserialized<T>(this string json) => 
            JsonUtility.FromJson<T>(json);

        public static Vector3 SnapPosition(this Vector3 position, Vector3 step = default)
        {
            if (step == Vector3.zero)
                step = Vector3.one;
            
            int clampedX = Mathf.RoundToInt ( position.x / step.x );
            int clampedY = Mathf.RoundToInt ( position.y / step.y );
            int clampedZ = Mathf.RoundToInt ( position.z / step.z );
            Vector3 snapPosition = new Vector3 (
                clampedX * step.x, 
                clampedY * step.y, 
                clampedZ * step.z);
            return snapPosition;
        }
    }
}