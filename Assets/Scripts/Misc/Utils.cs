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
        public static bool PreventSpawnBox(Vector2 CheckingPosition, Vector2[] readyPositions, Vector2 size)
        {
            if (readyPositions == null || readyPositions.Length == 0) return false;

            for (int i = 0; i < readyPositions.Length; i++)
            {
                Vector2 centerPoint = CheckingPosition;
                float width = size.x;
                float height = size.y;

                Vector2 leftUpCorner = new(centerPoint.x - width, centerPoint.y + height);
                Vector2 rightUpCorner = new(centerPoint.x + width, centerPoint.y + height);
                Vector2 leftDownCorner = new(centerPoint.x - width, centerPoint.y - height);
                Vector2 rightDownCorner = new(centerPoint.x + width, centerPoint.y - height);

                Vector2 spawnLeftUpCorner = new(CheckingPosition.x - width, CheckingPosition.y + height);
                Vector2 spawnRightUpCorner = new(CheckingPosition.x + width, CheckingPosition.y + height);
                Vector2 spawnLeftDownCorner = new(CheckingPosition.x - width, CheckingPosition.y - height);
                Vector2 spawnRightDownCorner = new(CheckingPosition.x + width, CheckingPosition.y - height);

                //to ogarnac zeby rogi obliczalo
                if (CheckingPosition.x >= centerPoint.x - width && CheckingPosition.x <= centerPoint.x + width &&
                    CheckingPosition.y >= centerPoint.y - height && CheckingPosition.y <= centerPoint.y + height)
                {
                    return false;
                }
                else if (spawnRightUpCorner.x >= leftDownCorner.x && spawnRightUpCorner.y >= leftDownCorner.y &&
                         spawnLeftDownCorner.x < leftDownCorner.x && spawnLeftDownCorner.y < leftDownCorner.y)
                {
                    return false; //lewy dolny
                }
                else if (spawnRightDownCorner.x >= leftUpCorner.x && spawnRightDownCorner.y <= leftUpCorner.y &&
                         spawnLeftUpCorner.x < leftUpCorner.x && spawnLeftUpCorner.y > leftUpCorner.y)
                {
                    return false; //lewy gorny
                }
                else if (spawnLeftDownCorner.x <= rightUpCorner.x && spawnLeftDownCorner.y <= rightUpCorner.y &&
                         spawnRightUpCorner.x > rightUpCorner.x && spawnRightUpCorner.y > rightUpCorner.y)
                {
                    return false; //prawy gorny
                }
                else if (spawnLeftUpCorner.x <= rightDownCorner.x && spawnLeftUpCorner.y >= rightDownCorner.y &&
                         spawnRightDownCorner.x > rightDownCorner.x && spawnRightDownCorner.y < rightDownCorner.y)
                {
                    return false; //prawy dolny
                }
                else if (spawnRightUpCorner.y == rightUpCorner.y || spawnRightDownCorner.y == rightDownCorner.y ||
                         spawnRightUpCorner.x == rightUpCorner.x || spawnLeftUpCorner.x == leftUpCorner.x)
                {
                    return false; //nachodzace na siebie linie
                }
            }
            return true;
        }
    }
}
