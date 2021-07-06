namespace CustomCommandBot.Shared.Models
{
    public class CommandActionResult
    {
        public bool IsError { get; init; }
        public string Reason { get; init; }

        public CommandActionResult(bool isError, string reason)
        {
            IsError = isError;
            Reason = reason;
        }

        public static CommandActionResult FromError(string reason)
            => new(true, reason);

        public static CommandActionResult FromSuccess()
            => new(false, null);
    }
}
