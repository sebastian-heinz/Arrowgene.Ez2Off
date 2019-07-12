/*
 * This file is part of Arrowgene.Ez2Off
 *
 * Arrowgene.Ez2Off is a server implementation for the game "Ez2On".
 * Copyright (C) 2017-2018 Sebastian Heinz
 *
 * Github: https://github.com/Arrowgene/Arrowgene.Ez2Off
 *
 * Arrowgene.Ez2Off is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Arrowgene.Ez2Off is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Arrowgene.Ez2Off. If not, see <https://www.gnu.org/licenses/>.
 */

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Server.Log;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Services.Logging;
using Arrowgene.Services.Networking.Tcp.Server.AsyncEvent;
using Arrowgene.Services.Scripting;
using Microsoft.CodeAnalysis;

namespace Arrowgene.Ez2Off.Server.Scripting
{
    public class EzScriptEngine
    {
        public static EzScriptEngine Instance => _instance;
        private static readonly EzScriptEngine _instance = new EzScriptEngine();

        private ScriptEngine _engine;
        private EzLogger _logger;

        public EzScriptEngine()
        {
            _engine = new ScriptEngine();
            _logger = LogProvider<EzLogger>.GetLogger(this);
            AddReference(Assembly.GetAssembly(typeof(EzServer)));
            AddReference(Assembly.GetAssembly(typeof(AsyncEventServer)));
            AddReference(Assembly.GetAssembly(typeof(Utils)));
        }

        public void AddReference(Assembly assembly)
        {
            _engine.AddReference(assembly);
        }

        /// <summary>
        /// Creates a HandlerId instance for each script found inside the directory.
        /// </summary>
        /// <param name="directoryInfo">Script directory</param>
        /// <param name="server">Intance of EzServer</param>
        public List<IHandler> LoadHandlers(DirectoryInfo directoryInfo, EzServer server)
        {
            List<IHandler> handlers = new List<IHandler>();
            List<FileInfo> scripts = Utils.GetFiles(directoryInfo, new[] {".cs"});
            foreach (FileInfo script in scripts)
            {
                string code = Utils.ReadFileText(script.FullName);
                if (code != null)
                {
                    ScriptTask scriptTask = _engine.CreateTask(script.FullName, code, server);
                    IHandler handler = scriptTask.CreateInstance<IHandler>();
                    if (handler != null)
                    {
                        handlers.Add(handler);
                    }
                    else if (scriptTask.Diagnostics != null && scriptTask.Diagnostics.Count > 0)
                    {
                        foreach (Diagnostic diagnostic in scriptTask.Diagnostics)
                        {
                            _logger.Error("{0}: {1}", scriptTask.Name, diagnostic);
                        }
                    }
                    else
                    {
                        _logger.Error("Failed to load script ({0})", scriptTask.Name);
                    }
                }
            }

            return handlers;
        }
    }
}