using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame_Template.Common.Helpers.Enum;
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

                if (mainCollider.Intersects(collider))
                {
                    mainCollider.OnCollision(collider);
                    collider.OnCollision(mainCollider);
                }
            }

            return true;
        }

        public static CollisionSide? GetCollisionType(this ICollider mainCollider, ICollider secondaryCollider)
        {
            var main = mainCollider.GetRect();
            var secondary = secondaryCollider.GetRect();

            var intersectPoint = new Vector2
            {
                X = (main.X - secondary.X) / (float)secondary.Width,
                Y = (main.Y - secondary.Y) / (float)secondary.Height
            };

            if (intersectPoint.Y <= .5)
            {
                return CollisionSide.Top;
            }

            if (intersectPoint.Y > .5)
            {
                return CollisionSide.Bottom;
            }

            if (intersectPoint.X <= .5)
            {
                return CollisionSide.Left;
            }

            if (intersectPoint.X > .5)
            {
                return CollisionSide.Right;
            }
            return null;
        }

        public static bool Intersects(this ICollider mainCollider, ICollider secondaryCollider)
        {
            if (mainCollider.Equals(secondaryCollider))
            {
                return false;
            }

            var main = mainCollider.GetRect();
            var secondary = secondaryCollider.GetRect();

            return main.Intersects(secondary);
        }

        public static Rectangle GetRect(this ICollider collider)
        {
            var size = new Point(collider.CurrentTexture.Width,
                collider.CurrentTexture.Height);

            return new Rectangle(collider.Position.ToPoint(), size);
        }
    }
}