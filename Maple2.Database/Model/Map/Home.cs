﻿using System.Collections.Generic;
using Maple2.Database.Extensions;
using Maple2.Model.Enum;
using Maple2.Model.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maple2.Database.Model;

internal class Home {
    public long AccountId { get; set; }

    public string Message { get; set; } = string.Empty;
    public byte Area { get; set; }
    public byte Height { get; set; }

    public int CurrentArchitectScore { get; set; }
    public int ArchitectScore { get; set; }

    // Interior Settings
    public byte Background { get; set; }
    public byte Lighting { get; set; }
    public byte Camera { get; set; }

    public string? Password { get; set; }
    public IDictionary<HomePermission, HomePermissionSetting> Permissions { get; set; } = new Dictionary<HomePermission, HomePermissionSetting>();

    public UgcMap Indoor { get; set; }

    public long? PlotId { get; set; }
    public UgcMap? Plot { get; set; }

    public static implicit operator Home?(Maple2.Model.Game.Home? other) {
        return other == null ? null : new Home {
            AccountId = other.AccountId,

            Message = other.Message,
            Area = other.Area,
            Height = other.Height,
            CurrentArchitectScore = other.CurrentArchitectScore,
            ArchitectScore = other.ArchitectScore,
            Background = other.Background,
            Lighting = other.Lighting,
            Camera = other.Camera,
            Password = other.Password,
            Permissions = other.Permissions,
            Indoor = new UgcMap {
                MapId = other.MapId,
                Number = other.Number,
                Name = other.Name,
            },
        };
    }

    public static implicit operator Maple2.Model.Game.Home?(Home? other) {
        if (other == null) {
            return null;
        }

        var home = new Maple2.Model.Game.Home {
            AccountId = other.AccountId,
            MapId = other.Indoor.MapId,
            Number = other.Indoor.Number,
            Name = other.Indoor.Name,
            Message = other.Message,
            CurrentArchitectScore = other.CurrentArchitectScore,
            ArchitectScore = other.ArchitectScore,
            Background = other.Background,
            Lighting = other.Lighting,
            Camera = other.Camera,
            Password = other.Password,
        };

        home.SetArea(other.Area);
        home.SetHeight(other.Height);
        foreach ((HomePermission permission, HomePermissionSetting setting) in other.Permissions) {
            home.Permissions[permission] = setting;
        }

        return home;
    }

    public static void Configure(EntityTypeBuilder<Home> builder) {
        builder.HasKey(home => home.AccountId);
        builder.OneToOne<Home, Account>()
            .HasForeignKey<Home>(home => home.AccountId);

        builder.Property(home => home.Area)
            .HasDefaultValue(Constant.MinHomeArea);
        builder.Property(home => home.Height)
            .HasDefaultValue(Constant.MinHomeHeight);

        builder.Property(home => home.Permissions).HasJsonConversion();
    }
}