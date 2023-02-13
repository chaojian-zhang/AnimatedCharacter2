using System.Numerics;

namespace AnimatedCharacter.AnimationManagement
{
    public class AnimationState
    {
        public string Name { get; set; } = "Default";
        public int Width { get; set; }
        public int Height { get; set; }
        public float ChanceWeight { get; set; } = 1.0f;
        public Vector2[] Locations { get; set; }
    }

    public class AnimationSpecification
    {
        public string ImageResourcePath { get; set; }
        public AnimationState[] States { get; set; }
    }
}
