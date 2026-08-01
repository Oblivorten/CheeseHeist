using UnityEngine;
using CheeseHeist.Core;

namespace CheeseHeist.Adapters
{
    public class CatBody : MonoBehaviour
    {
        public void Sync(CatData data)
        {
            transform.position = new Vector3(data.Position.X, data.Position.Y, data.Position.Z);

            float velSq = data.Velocity.X * data.Velocity.X + data.Velocity.Z * data.Velocity.Z;
            if (velSq > 0.01f)
            {
                var lookDir = new Vector3(data.Velocity.X, 0f, data.Velocity.Z);
                transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
            }
        }
    }
}