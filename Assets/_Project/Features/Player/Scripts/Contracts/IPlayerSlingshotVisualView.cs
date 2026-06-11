namespace _Project.Features.Player.Scripts.Contracts
{
    public interface IPlayerSlingshotVisualView
    {
        public void Show();
        public void Hide();
        public void ResetPosition();
        public void UpdatePosition();
    }
}