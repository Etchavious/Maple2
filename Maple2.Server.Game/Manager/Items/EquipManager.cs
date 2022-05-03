﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Maple2.Model.Enum;
using Maple2.Model.Game;
using Maple2.Server.Game.Session;

namespace Maple2.Server.Game.Manager.Items;

public class EquipManager {
    private readonly GameSession session;

    public readonly ConcurrentDictionary<EquipSlot, Item> Gear;
    public readonly ConcurrentDictionary<EquipSlot, Item> Outfit;
    public readonly ConcurrentDictionary<BadgeType, Item> Badge;

    public EquipManager(GameSession session) {
        this.session = session;

        Gear = new ConcurrentDictionary<EquipSlot, Item>();
        Outfit = new ConcurrentDictionary<EquipSlot, Item>();
        Badge = new ConcurrentDictionary<BadgeType, Item>();
    }

    /// <summary>
    /// Equips an item to its specified slot
    /// </summary>
    /// <param name="item">The item to be equipped</param>
    /// <param name="slot">The slot to be equipped to.
    /// - If not specified, slot from item will be used.
    /// - If item does not have a slot, slot will be determined by metadata.
    /// </param>
    /// <returns>The items if any that were unequipped to equip this item.</returns>
    public IEnumerable<Item> Equip(Item item, EquipSlot slot) {
        if (!ValidEquipSlot(slot)) {
            throw new InvalidOperationException($"Cannot equip item to slot {slot}");
        }

        ConcurrentDictionary<EquipSlot, Item> inventory = item.Metadata.Property.IsSkin ? Outfit : Gear;
        foreach (EquipSlot removeSlot in item.Metadata.SlotNames) {
            if (inventory.Remove(removeSlot, out Item? removed)) {
                yield return removed;
            }
        }
    }

    private static bool ValidEquipSlot(EquipSlot slot) {
        return slot is not (EquipSlot.SK or EquipSlot.OH or EquipSlot.ER or EquipSlot.Unknown);
    }
}