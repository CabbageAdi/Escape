using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cabbage.Helpers
{
    public static class ExtensionMethods
    {
        public static Vector3Int ToInt(this Vector3 vector, int offsetX = 0, int offsetY = 0, int offsetZ = 0)
        {
            return new Vector3Int((int) vector.x + offsetX, (int) vector.y + offsetY, (int) vector.z + offsetZ);
        }

        public static bool IsLessThan(this Vector2 one, Vector2 two)
        {
            return (one.x < two.x && one.y < two.y);
        }
        
        public static bool IsMoreThan(this Vector2 one, Vector2 two)
        {
            return (one.x > two.x && one.y > two.y);
        }

        public static float GetAxisDown(this MonoBehaviour hmm, Axis axis)
        {
            switch (axis)
            {
                case Axis.Horizontal:
                    if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                        return -1;
                    if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                        return 1;
                    break;
                case Axis.Vertical:
                    if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                        return -1;
                    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                        return 1;
                    break;
                default:
                    return 0;
            }

            return 0;
        }

        public static T FirstOrDefaultOut<T>(this IEnumerable<T> collection, Func<T, bool> predicate, out T element)
        {
            element = collection.FirstOrDefault(predicate);
            return element;
        }
    }

    public enum Axis
    {
        Horizontal,
        Vertical
    }
}