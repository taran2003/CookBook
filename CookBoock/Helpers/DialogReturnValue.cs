namespace CookBoock.Helpers
{
    public class DialogReturnValue
    {
        public readonly DialogReturnStatuses status;

        public DialogReturnValue(DialogReturnStatuses status)
        {
            this.status = status;
        }
    }
}
