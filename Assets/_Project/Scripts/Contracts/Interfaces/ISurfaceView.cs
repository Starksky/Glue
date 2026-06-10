namespace _Project.Scripts.Contracts.Interfaces
{
    public interface ISurfaceView
    {
        public bool BeginContact(IPhysicBody2D physicBody2D);
        public bool StayContact(IPhysicBody2D physicBody2D);
        public void EndContact(IPhysicBody2D physicBody2D);
    }
}