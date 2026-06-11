namespace _Project.Features.Player.Scripts.Contracts
{
    public interface IPlayerSlingshotConfig
    {
        public float MaxDragDistanceBody { get; }
        public float MaxDragDistance { get; }
        public float MaxForce { get; }
        public float AngleForForce { get; }
        public float DragForce { get; }
    }
}