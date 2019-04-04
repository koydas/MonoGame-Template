using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;

namespace MonoGame_Template.Common.Interfaces
{
    public interface ICollider
    {
        Body Body { get; set; }
        Texture2D CurrentTexture { get; set; }
    }
}
