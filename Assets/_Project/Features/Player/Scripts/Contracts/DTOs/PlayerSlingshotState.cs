namespace _Project.Features.Player.Scripts.Contracts.DTOs
{
    public struct PlayerSlingshotState : IPlayerSlingshotConfig
    {
        public float MaxDragDistanceBody { get; }
        public float MaxDragDistance { get; }
        public float ThrowForce { get; }
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
        public PlayerSlingshotState(
            float maxDragDistanceBody, 
            float maxDragDistance, 
            float throwForce, 
            float angleForForce,
            float dragForce)
        {
            MaxDragDistanceBody = maxDragDistanceBody;
            MaxDragDistance =  maxDragDistance;
            ThrowForce = throwForce;
            AngleForForce = angleForForce;
            DragForce = dragForce;
        }
    }
}