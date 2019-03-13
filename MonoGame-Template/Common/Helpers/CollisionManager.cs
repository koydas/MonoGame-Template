using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Template.Common.Helpers.Enum;
using MonoGame_Template.Common.Interfaces;

namespace MonoGame_Template.Common.Helpers
{
    public static class CollisionManager
    {
        public static bool MovementAllowed(this ICollider mainCollider, Vector2 movement, IEnumerable<ICollider> colliders)
        {
            var anticipatedCollider = mainCollider;

            anticipatedCollider.Position = mainCollider.Position + movement;

            foreach (var collider in colliders)
            {
                if (collider?.Position == null || collider?.CurrentTexture == null)
                {
                    continue;
                }

                if (anticipatedCollider.Intersects(collider))
                {
                    //mainCollider.OnCollision(collider);
                    //collider.OnCollision(mainCollider);
                    return false;
                }
            }

            return true;
        }

        //public static bool IsColliding(this ICollider mainCollider, IEnumerable<ICollider> colliders)
        //{
        //    foreach (var collider in colliders)
        //    {
        //        if (collider?.Position == null || collider?.CurrentTexture == null)
        //        {
        //            continue;
        //        }

        //        if (mainCollider.Intersects(collider))
        //        {
        //            mainCollider.OnCollision(collider);
        //            collider.OnCollision(mainCollider);
        //        }
        //    }

        //    return true;
        //}

        //public static CollisionSide? GetCollisionType(this ICollider mainCollider, ICollider secondaryCollider)
        //{
        //    var main = mainCollider.GetRect();
        //    var secondary = secondaryCollider.GetRect();

        //    var intersect = Rectangle.Intersect(main, secondary);

        //    if (intersect.Height < intersect.Width)
        //    {
        //        return main.Center.Y > secondary.Center.Y 
        //            ? CollisionSide.Top 
        //            : CollisionSide.Bottom;
        //    }

        //    if (main.Center.Y < secondary.Center.Y)
        //    {
        //        return main.Center.X < secondary.Center.X 
        //            ? CollisionSide.Left 
        //            : CollisionSide.Right;
        //    }

        //    return null;
        //}

        public static bool Intersects(this ICollider mainCollider, ICollider secondaryCollider)
        {
            if (mainCollider.Equals(secondaryCollider))
            {
                return false;
            }

            var mainRect = mainCollider.GetRect();
            var secondaryRect = secondaryCollider.GetRect();

            if (mainRect.Intersects(secondaryRect))
            {
                Rectangle intersection = GetIntersection(mainCollider, secondaryCollider);

                var mainPixelColor = mainCollider.GetCollisionPixel(intersection);
                var secondaryPixelColor = secondaryCollider.GetCollisionPixel(intersection);

                return mainPixelColor != Color.Transparent && secondaryPixelColor != Color.Transparent;
            }

            return false;
        }

        public static Rectangle GetRect(this ICollider collider)
        {
            var size = new Point(collider.CurrentTexture.Width,
                collider.CurrentTexture.Height);

            return new Rectangle(collider.Position.ToPoint(), size);
        }

        private static Rectangle GetIntersection(ICollider mainCollider, ICollider secondaryCollider)
        {
            var main = mainCollider.GetRect();
            var secondary = secondaryCollider.GetRect();
            var intersect = Rectangle.Intersect(main, secondary);
            return intersect;
        }

        private static Color GetCollisionPixel(this ICollider mainCollider, Rectangle intersect)
        {
            var mainPixels = mainCollider.CurrentTexture.GetPixels();
            var mainPixelColor = mainPixels.GetPixel(intersect.X, intersect.Y, 1);
            return mainPixelColor;
        }
    }
}