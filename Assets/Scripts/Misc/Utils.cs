using UnityEngine;
using UnityEngine.InputSystem;

namespace Main.Misc
{
    public class Utils
    {
        public static void UpdateBar(Transform bar, float proportion)
        {
            Vector3 barScale = bar.localScale;
            barScale.x = proportion;
            bar.localScale = barScale;
        }


        public static Vector3 GetMouseWorldPosition()
        {
            Vector3 vector = GetMouseWorldPositionWithZ(Mouse.current.position.ReadValue(), Camera.main);
            vector.z = 0f;
            return vector;
        }
        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }

        public static void Log(object message, Object gameObject = null)
        {
#if UNITY_EDITOR
            if (gameObject == null)
                Debug.Log(message.ToString());
            else
                Debug.Log(message.ToString(), gameObject);
#endif
        }

        public static void LogWarning(string message)
        {
#if UNITY_EDITOR
            Debug.LogWarning(message);
#endif
        }

        public static bool PreventSpawnCircle(Vector2 checkingPosition, Vector2[] readyPositions, float size)
        {
            if (readyPositions == null || readyPositions.Length == 0) return true;

            for (int i = 0; i < readyPositions.Length; i++)
            {
                var currentCheckingPosition = readyPositions[i];

                if (Vector2.Distance(checkingPosition, currentCheckingPosition) < size * 2)
                    return false;
            }
            return true;
        }
    }
}
