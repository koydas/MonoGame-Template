using System.Collections;
using System.Collections.Generic;
using MonoGame_Template.Common.Interfaces;

namespace MonoGame_Template.Common.Helpers
{
    public static class CollisionManager
    {
        public static bool IsColliding(this ICollider mainCollider, IEnumerable<ICollider> colliders)
        {
            foreach (var collider in colliders)
            {
                if (collider?.Position == null || collider?.CurrentTexture == null)
                {
                    continue;
                }

                if (
                    // Width
                    mainCollider.Position.X > collider.Position.X
                    && mainCollider.Position.X < collider.Position.X + collider.CurrentTexture.Width

                    // Height
                    && mainCollider.Position.Y > collider.Position.Y
                    && mainCollider.Position.Y < collider.Position.Y + collider.CurrentTexture.Height
                )
                {
                    mainCollider.OnCollision(collider);
                    collider.OnCollision(mainCollider);
                }
            }

            return true;
        }
    }
}
