using System;

namespace WebApp.APIs.Dtos;

public readonly record struct ImageDto(
    long Id,
    long UploaderId,
    long AlbumId,
    string Title,
    long[] Tags,
    DateTime UploadedAt,
    DateTime? RemovedAt
);
