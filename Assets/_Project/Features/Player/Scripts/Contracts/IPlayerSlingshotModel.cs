namespace _Project.Features.Player.Scripts.Contracts
{
    public interface IPlayerSlingshotModel
    {
        float ThrowForce { get; }
        void AddThrowForcePercent(int percent);
        IPlayerSlingshotConfig GetState();
    }
}