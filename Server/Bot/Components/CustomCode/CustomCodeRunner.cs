using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CustomCommandBot.Shared.Models.CustomCode;
using Discord.Commands;
using NLua;

namespace CustomCommandBot.Server.Bot.Components
{
    public class CustomCodeRunner
    {
        private readonly string _basePath = @".\Bot\Components\CustomCode\";

        private string _codeContent;
        private string _sandboxContent;

        public CustomCodeRunner()
        {
            
        }

        public void Run(string code, SocketCommandContext context)
        {
            _codeContent = File.ReadAllText(_basePath + "Code.lua");
            _sandboxContent = File.ReadAllText(_basePath + "Sandbox.lua");

            using (var state = new Lua())
            {
                state["_IMPORT_CODE"] = code;
                state["_IMPORT_ENV"] = new LuaCommandState
                {
                    Context = new(context)
                };

                state.DoString(_sandboxContent);

                var result = state.DoString(_codeContent);
                Console.WriteLine("hi");
            }
        }
    }
}
