using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

public class CommandFailureException : Exception
{
    public DateTime Timestamp { get; private set; }

    public Type Type
    {
        get
        {
            return InnerException?.GetType() ?? GetType();
        }
    }

    private void Setup()
    {
        Timestamp = DateTime.Now;
    }

    public CommandFailureException()
    {
        Setup();
    }

    public CommandFailureException(string message)
        : base(message)
    {
        Setup();
    }

    public CommandFailureException(string message, string stackTrace)
        : base(message)
    {
        StackTrace = stackTrace;
        Setup();
    }

    public CommandFailureException(string message, string stackTrace, Exception inner)
        : base(message, inner)
    {
        StackTrace = stackTrace;
        Setup();
    }

    public string GetErrorCode(ulong guildId)
    {
        var guildString = guildId.ToString();

        var typeString = string.Concat(Type.Name.Where(c => char.IsUpper(c)));
        var guildFirst = guildString.Substring(0, 3);
        var guildLast = guildString.Substring(guildString.Length - 3);
        var time = Timestamp.ToString("HH:mm:ss:ffff");

        return $"{typeString}-{guildFirst}:{guildLast}-{Timestamp.Month}/{Timestamp.Day}-{time}";
    }

    public override string ToString()
    {
        return $"{GetType()}: {Message}\n{StackTrace}";
    }

    public override string StackTrace { get; }
}