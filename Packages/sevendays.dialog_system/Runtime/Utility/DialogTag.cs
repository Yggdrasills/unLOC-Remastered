namespace SevenDays.DialogSystem.Runtime
{
    public class DialogTag : Enumeration
    {
        public static DialogTag EngLocalization = new DialogTag(1, "engLocalization");
        public static DialogTag DefaultLocalization = new DialogTag(2, "defaultLocalization");

        public DialogTag(int id, string name) : base(id, name)
        {
        }
    }
}