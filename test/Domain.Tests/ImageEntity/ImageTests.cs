﻿using System.Reflection;
using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.Commands;
using Domain.AlbumDomain.Events;
using Domain.AlbumDomain.ImageEntity;
using Domain.Entity;
using Domain.Shared;
using Shouldly;
using static Domain.Tests.ImageEntity.ImageTestsHelper;

namespace Domain.Tests.ImageEntity;

[TestClass]
public class ImageTests
{
    [TestMethod]
    public void Raise_Event_When_Image_Removed()
    {
        Image image = ValidNewImage;
        RemoveImageCommand command = new(OuterAlbumId, Id, OuterAuthor);

        image.Remove(in command);

        image.DomainEvents.Count.ShouldBe(1);
        image.DomainEvents.First().ShouldBeOfType<ImageRemovedEvent>();
    }

    [TestMethod]
    public void Raise_Event_When_Image_Restored()
    {
        Image image = ValidNewImage;
        image.SetValue(true); // _isRemoved = true;
        RestoreImageCommand command = new(OuterAlbumId, Id, OuterAuthor);
        image.Restore(in command);

        image.DomainEvents.Count.ShouldBe(1);
        image.DomainEvents.First().ShouldBeOfType<ImageRestoredEvent>();
    }
}

internal static class ImageTestsHelper
{
    public static readonly Actor OuterAuthor = new();
    public static readonly AlbumId OuterAlbumId = new(2333);
    public static readonly ImageId Id = new(1);
    public static Image ValidNewImage => CreateNewImage(Id.Value);
    public static readonly ImageTitle NewImageTitle = new("new_title");
    public static readonly ImageTags NewImageTags = new([new(741), new(852)]);

    public static Image CreateNewImage(long id)
    {
        var image = CreateNewImageFromReflection();

        typeof(EntityBase<ImageId>)
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .First(f => f.Name.Contains("id", StringComparison.OrdinalIgnoreCase))
            .SetValue(image, new ImageId(id));

        return image;
    }

    public static void SetValue<T>(this Image image, T value)
    {
        typeof(Image)
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .First(f => f.FieldType == typeof(T))
            .SetValue(image, value);
    }

    private static Image CreateNewImageFromReflection()
    {
        var constructor = typeof(Image).GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            Type.EmptyTypes
        );
        Assert.IsNotNull(constructor);

        var image = (Image)constructor.Invoke(null);

        image.SetValue(new List<Like>());

        return image;
    }
}
