using Domain.Entity;

namespace Domain.UserDomain.UserEntity;

public readonly record struct JwtValue(string Value) : IValueObject<JwtValue, string>;
