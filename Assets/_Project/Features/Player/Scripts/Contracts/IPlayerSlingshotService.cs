using System;

namespace _Project.Features.Player.Scripts.Contracts
{
    public interface IPlayerSlingshotService
    {
        event Action<IPlayerSlingshotConfig> OnPlayerSlingshotConfigChange;
        IPlayerSlingshotConfig GetState();
    }
}