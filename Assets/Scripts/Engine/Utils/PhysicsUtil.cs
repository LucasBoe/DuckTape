
using UnityEngine;

namespace SS
{
    public static class PhysicsUtil
    {
        public static void ApplyGravity(this Rigidbody2D rigidbody, Vector2 gravityDirection)
        {
            Vector2 grav = 4f * gravityDirection;
            rigidbody.AddForce(grav * rigidbody.mass);
        }
    }
}
