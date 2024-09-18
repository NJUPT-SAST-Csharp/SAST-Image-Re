﻿using Domain.UserDomain.Events;

namespace Application.UserServices;

public sealed class UserModel
{
    private UserModel() { }

    public long Id { get; }
    public string Username { get; private set; } = null!;
    public string Biography { get; private set; } = string.Empty;
    public DateTime RegisteredAt { get; private init; }

    internal UserModel(UserRegisteredEvent e)
    {
        Id = e.Id.Value;
        Username = e.Username.Value;
        RegisteredAt = DateTime.UtcNow;
    }

    internal void ResetUsername(UsernameResetEvent e)
    {
        Username = e.Username.Value;
    }

    internal void UpdateBiography(BiographyUpdatedEvent e)
    {
        Biography = e.Biography.Value;
    }
}
