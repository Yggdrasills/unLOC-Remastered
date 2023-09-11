using SevenDays.unLOC.Activities.Items;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Quests.RobotPainter
{
    public class RobotPainterButtonView : ClickableItem
    {
        [SerializeField]
        private string _code = string.Empty;

        public string Code => _code;
    }
}