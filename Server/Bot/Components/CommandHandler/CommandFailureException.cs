using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CommandFailureException : Exception
{
    public CommandFailureException()
    {
    }

    public CommandFailureException(string message)
        : base(message)
    {
    }

    public CommandFailureException(string message, Exception inner)
        : base(message, inner)
    {
    }
}