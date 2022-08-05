using System.Threading;

using Cysharp.Threading.Tasks;

using UnityEngine;

namespace SevenDays.unLOC.Core.Movement
{
    public interface IMovable
    {
        // review: лишнее свойство. Передвигаемый объект не может быть активным или неактивным.
        // review: Это определяет контроллер, либо модель
        bool IsActive { get; set; }
        
        bool IsMoving { get; }

        UniTask MoveToPointAsync(Vector3 point, CancellationToken token);

        // review: убрать continue moving и pause moving. Или двигаемся или не двигаемся.
        void ContinueMoving();

        void PauseMoving();
        
        void StopMoving();
        // review: direction может быть только лево или право (1 или -1)
        // review: Если значение плавает, условно, от -1 до 1, то это нормаль направления  
        void Move(float horizontalDirection);
    }
}