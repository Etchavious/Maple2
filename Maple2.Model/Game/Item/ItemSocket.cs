﻿using System;
using Maple2.PacketLib.Tools;
using Maple2.Tools;
using Maple2.Tools.Extensions;

namespace Maple2.Model.Game;

public class ItemSocket : IByteSerializable, IByteDeserializable {
    public static readonly ItemSocket Default = new ItemSocket(0, 0);

    public byte MaxSlots;
    public byte UnlockSlots {
        get => (byte) Sockets.Length;
        set => Array.Resize(ref Sockets, value);
    }

    public ItemGemstone?[] Sockets;

    public ItemSocket(byte maxSlots, byte unlocked) {
        MaxSlots = maxSlots;
        Sockets = new ItemGemstone[unlocked];
    }

    public ItemSocket(byte maxSlots, ItemGemstone[] sockets) {
        MaxSlots = maxSlots;
        Sockets = sockets;
    }

    public void WriteTo(IByteWriter writer) {
        writer.WriteByte(MaxSlots);
        writer.WriteByte(UnlockSlots);
        for (int i = 0; i < UnlockSlots; i++) {
            ItemGemstone? gem = Sockets[i];
            writer.WriteBool(gem != null);
            if (gem != null) {
                writer.WriteClass<ItemGemstone>(gem);
            }
        }
    }

    public void ReadFrom(IByteReader reader) {
        MaxSlots = reader.ReadByte();
        UnlockSlots = reader.ReadByte();
        for (int i = 0; i < UnlockSlots; i++) {
            bool hasGem = reader.ReadBool();
            if (hasGem) {
                Sockets[i] = reader.ReadClass<ItemGemstone>();
            }
        }
    }
}

public class ItemGemstone : IByteSerializable, IByteDeserializable {
    public int ItemId;
    public ItemBinding? Binding;
    public bool IsLocked;
    public long UnlockTime;

    public ItemGemstone(int itemId = 0, ItemBinding? binding = null, bool isLocked = false, long unlockTime = 0) {
        ItemId = itemId;
        Binding = binding;
        IsLocked = isLocked;
        UnlockTime = unlockTime;
    }

    public void WriteTo(IByteWriter writer) {
        writer.WriteInt(ItemId);

        writer.WriteBool(Binding != null);
        if (Binding != null) {
            writer.WriteClass<ItemBinding>(Binding);
        }

        writer.WriteBool(IsLocked);
        if (IsLocked) {
            writer.WriteByte();
            writer.WriteLong(UnlockTime);
        }
    }

    public void ReadFrom(IByteReader reader) {
        ItemId = reader.ReadInt();

        bool isBound = reader.ReadBool();
        if (isBound) {
            Binding = reader.ReadClass<ItemBinding>();
        }

        IsLocked = reader.ReadBool();
        if (IsLocked) {
            reader.ReadByte();
            UnlockTime = reader.ReadLong();
        }
    }
}
