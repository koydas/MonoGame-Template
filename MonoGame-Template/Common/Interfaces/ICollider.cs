using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Template.Common.Interfaces
{
    public interface ICollider
    {
        Vector2 Position { get; set; }
        Vector2 Velocity { get; set; }
        Vector2 MaxVelocity { get; set; }
        Texture2D CurrentTexture { get; set; }
        //void OnCollision(ICollider collider);
    }
}
