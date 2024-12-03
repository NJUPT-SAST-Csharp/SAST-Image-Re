namespace WebApp.APIs.Dtos;

public readonly record struct AlbumDto(
    long Id,
    string Title,
    long Author,
    long Category,
    DateTime UpdatedAt
);
