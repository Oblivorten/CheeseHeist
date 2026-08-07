using UnityEngine;
using CheeseHeist.Core;

namespace CheeseHeist.Adapters
{
    public class CatBody : MonoBehaviour
    {
        public void Sync(CatData data)
        {
            transform.position = new Vector3(data.Position.X, data.Position.Y, data.Position.Z);

            var facing = new Vector3(data.FacingDirection.X, 0f, data.FacingDirection.Z);
            if (facing.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(facing, Vector3.up);
            }
        }
    }
}