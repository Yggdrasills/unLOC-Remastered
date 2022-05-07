using System.Threading;

using Cysharp.Threading.Tasks;

using UnityEngine;

namespace SevenDays.unLOC.Core.Movement
{
    public interface IMovable
    {
        bool IsActive { get; set; }
        
        bool IsMoving { get; }

        UniTask MoveToPointAsync(Vector3 point, CancellationToken token);

        void ContinueMoving();

        void PauseMoving();
        
        void StopMoving();
    }
}