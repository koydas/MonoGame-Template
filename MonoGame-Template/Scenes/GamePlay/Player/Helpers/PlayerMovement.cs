using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame_Template.Common.Enums;
using MonoGame_Template.Common.Helpers;

namespace MonoGame_Template.Scenes.GamePlay.Player.Helpers
{
    public static class PlayerMovement
    {
        private const float JumpForce = 0.003f;
        private const float MovementSpeed = 0.0005f;

        public static void Jump(this Player player)
        {
            if (player.IsGrounded && Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                if (player.Body.LinearVelocity.Y > -JumpForce)
                {
                    player.Body.ApplyForce(Vector2.UnitY * -JumpForce);
                }

                player.IsGrounded = false;
            }
        }

        public static void Move(this Player player)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (player.Body.LinearVelocity.X < MovementSpeed)
                {
                    player.Body.ApplyForce(Vector2.UnitX * MovementSpeed);
                    player.Direction = Direction.Right;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (player.Body.LinearVelocity.X > -MovementSpeed)
                {
                    player.Body.ApplyForce(Vector2.UnitX * -MovementSpeed);
                    player.Direction = Direction.Left;
                }
            }

            // Avoid falling out of the screen
            if (player.Body.Position.X.ToDisplayUnit() < 0)
            {
                player.Body.Position = new Vector2(0, player.Body.Position.Y);
            }
        }

        public static void ApplyGravity(this Player player)
        {
            player.Body.ApplyForce(Vector2.UnitY * 0.0005f);
        }

        public static bool Teleport(this Player player, int deltaTime)
        {
            if (player.TeleportOpacity < 1)
            {
                player.TeleportOpacity += .001f * deltaTime;

                return true;
            }

            return false;
        }
    }
}
