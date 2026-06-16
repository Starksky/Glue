namespace _Project.Features.Player.Scripts.Contracts.DTOs
{
    public struct PlayerSlingshotState : IPlayerSlingshotConfig
    {
        public float MaxDragDistanceBody { get; }
        public float MaxDragDistance { get; }
        public float ThrowForce { get; set; }
        public float AngleForForce { get; }
        public float DragForce { get; }

        public PlayerSlingshotState(IPlayerSlingshotConfig config)
        {
            MaxDragDistanceBody = config.MaxDragDistanceBody;
            MaxDragDistance = config.MaxDragDistance;
            ThrowForce = config.ThrowForce;
            AngleForForce = config.AngleForForce;
            DragForce = config.DragForce;
        }
    }
}