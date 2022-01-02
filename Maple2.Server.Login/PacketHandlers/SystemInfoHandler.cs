﻿using Maple2.Server.Core.PacketHandlers;
using Maple2.Server.Login.Session;
using Microsoft.Extensions.Logging;

namespace Maple2.Server.Login.PacketHandlers;

public class SystemInfoHandler : SystemInfoHandler<LoginSession> {
    public SystemInfoHandler(ILogger<SystemInfoHandler> logger) : base(logger) { }

    public override string ToString() => $"[0x{OpCode:X4}] Login.{GetType().Name}";
}
