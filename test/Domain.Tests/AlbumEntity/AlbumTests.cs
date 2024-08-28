using System.Reflection;
using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.Commands;
using Domain.AlbumDomain.Events;
using Domain.AlbumDomain.Exceptions;
using Domain.AlbumDomain.ImageEntity;
using Domain.CategoryDomain.CategoryEntity;
using Domain.Entity;
using Domain.Shared;
using Domain.Tests.ImageEntity;
using Shouldly;
using static Domain.Tests.AlbumEntity.AlbumTestsHelper;

namespace Domain.Tests.AlbumEntity;

[TestClass]
public class AlbumTests
{
    #region IsImmutable

    [TestMethod]
    public void Return_True_When_Album_Removed()
    {
        Album album = ValidNewAlbum;
        album.SetValue("_isRemoved", true);

        bool isImmutable = album.GetProperty<bool>("IsImmutable");

        isImmutable.ShouldBeTrue();
    }

    [TestMethod]
    public void Return_True_When_Album_Archived()
    {
        Album album = ValidNewAlbum;
        album.SetValue("_isArchived", true);

        bool isImmutable = album.GetProperty<bool>("IsImmutable");

        isImmutable.ShouldBeTrue();
    }

    [TestMethod]
    public void Return_False_When_Album_Not_Archived_Or_Removed()
    {
        Album album = ValidNewAlbum;

        bool isImmutable = album.GetProperty<bool>("IsImmutable");

        isImmutable.ShouldBeFalse();
    }

    #endregion


    #region CreateAlbum

    [TestMethod]
    public void Raise_Event_When_CreateAlbum()
    {
        CreateAlbumCommand command =
            new(NewTitle, NewDescription, default, NewCategory, NewActor(AuthorId));

        var album = new Album(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumCreatedEvent>();
    }

    #endregion

    #region UpdateDescription

    [TestMethod]
    public void Throw_When_UpdateDescription_In_Immutable_Album()
    {
        Album album = ValidNewAlbum;
        album.SetValue("_isArchived", true);
        UpdateAlbumDescriptionCommand command = new(Id, NewDescription, Author);

        Should.Throw<AlbumImmutableException>(() => album.UpdateDescription(command));
    }

    [DataRow(VisitorId)]
    [DataRow(Collaborator1Id)]
    [TestMethod]
    public void Throw_When_UpdateDescription_As_Not_Author_Or_Admin(long actorId)
    {
        Album album = ValidNewAlbum;
        UpdateAlbumDescriptionCommand command = new(Id, NewDescription, NewActor(actorId));

        Should.Throw<NoPermissionException>(() => album.UpdateDescription(command));
    }

    [DataRow(AdminId, true)]
    [DataRow(AuthorId, false)]
    [TestMethod]
    public void Raise_Event_When_Description_Updated(long actorId, bool isAdmin)
    {
        Album album = ValidNewAlbum;
        UpdateAlbumDescriptionCommand command = new(Id, NewDescription, NewActor(actorId, isAdmin));

        album.UpdateDescription(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumDescriptionUpdatedEvent>();
    }

    #endregion

    #region UpdateTitle

    [TestMethod]
    public void Throw_When_UpdateTitle_In_Immutable_Album()
    {
        Album album = ValidNewAlbum;
        album.SetValue("_isArchived", true);
        UpdateAlbumTitleCommand command = new(Id, NewTitle, Author);

        Should.Throw<AlbumImmutableException>(() => album.UpdateTitle(command));
    }

    [DataRow(VisitorId)]
    [DataRow(Collaborator1Id)]
    [TestMethod]
    public void Throw_When_UpdateTitle_As_Not_Author_Or_Admin(long actorId)
    {
        Album album = ValidNewAlbum;
        UpdateAlbumTitleCommand command = new(Id, NewTitle, NewActor(actorId));

        Should.Throw<NoPermissionException>(() => album.UpdateTitle(command));
    }

    [DataRow(AdminId, true)]
    [DataRow(AuthorId, false)]
    [TestMethod]
    public void Raise_Event_When_Title_Updated(long actorId, bool isAdmin)
    {
        Album album = ValidNewAlbum;
        UpdateAlbumTitleCommand command = new(Id, NewTitle, NewActor(actorId, isAdmin));

        album.UpdateTitle(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumTitleUpdatedEvent>();
    }

    #endregion

    #region UpdateCategory

    [TestMethod]
    public void Throw_When_UpdateCategory_In_Immutable_Album()
    {
        Album album = ValidNewAlbum;
        album.SetValue("_isRemoved", true);
        UpdateAlbumCategoryCommand command = new(Id, NewCategory, Author);

        Should.Throw<AlbumImmutableException>(() => album.UpdateCategory(command));
    }

    [DataRow(VisitorId)]
    [DataRow(Collaborator1Id)]
    [TestMethod]
    public void Throw_When_UpdateCategory_As_Not_Author_Or_Admin(long actorId)
    {
        Album album = ValidNewAlbum;
        UpdateAlbumCategoryCommand command = new(Id, NewCategory, NewActor(actorId));

        Should.Throw<NoPermissionException>(() => album.UpdateCategory(command));
    }

    [DataRow(AdminId, true)]
    [DataRow(AuthorId, false)]
    [TestMethod]
    public void Raise_Event_When_Category_Updated(long actorId, bool isAdmin)
    {
        Album album = ValidNewAlbum;
        UpdateAlbumCategoryCommand command = new(Id, NewCategory, NewActor(actorId, isAdmin));

        album.UpdateCategory(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumCategoryUpdatedEvent>();
    }

    #endregion

    #region UpdateAccessibility

    [TestMethod]
    public void Throw_When_UpdateAccessibility_In_Removed_Album()
    {
        Album album = ValidNewAlbum;
        album.SetValue("_isRemoved", true);
        UpdateAccessibilityCommand command = new(Id, NewAccessibility, Author);

        Should.Throw<AlbumImmutableException>(() => album.UpdateAccessibility(command));
    }

    [TestMethod]
    public void Not_Throw_When_UpdateAccessibility_In_Archived_Album()
    {
        Album album = ValidNewAlbum;
        album.SetValue("_isArchived", true);
        UpdateAccessibilityCommand command = new(Id, NewAccessibility, Author);

        Should.NotThrow(() => album.UpdateAccessibility(command));
    }

    [DataRow(VisitorId)]
    [DataRow(Collaborator1Id)]
    [TestMethod]
    public void Throw_When_UpdateAccessibility_As_Not_Author_Or_Admin(long actorId)
    {
        Album album = ValidNewAlbum;
        UpdateAccessibilityCommand command = new(Id, NewAccessibility, NewActor(actorId));

        Should.Throw<NoPermissionException>(() => album.UpdateAccessibility(command));
    }

    [DataRow(AdminId, true)]
    [DataRow(AuthorId, false)]
    [TestMethod]
    public void Raise_Event_When_Accessibility_Updated(long actorId, bool isAdmin)
    {
        Album album = ValidNewAlbum;
        UpdateAccessibilityCommand command = new(Id, NewAccessibility, NewActor(actorId, isAdmin));

        album.UpdateAccessibility(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumAccessibilityUpdatedEvent>();
    }

    #endregion

    #region UpdateCollaborators

    [TestMethod]
    public void Throw_When_UpdateCollaborators_In_Immutable_Album()
    {
        Album album = ValidNewAlbum;
        album.SetValue("_isRemoved", true);
        UpdateCollaboratorsCommand command = new(Id, NewCollaborators, Author);

        Should.Throw<AlbumImmutableException>(() => album.UpdateCollaborators(command));
    }

    [DataRow(VisitorId)]
    [DataRow(Collaborator1Id)]
    [TestMethod]
    public void Throw_When_UpdateCollaborators_As_Not_Author_Or_Admin(long actorId)
    {
        Album album = ValidNewAlbum;
        Collaborators collaborators = NewCollaborators;
        UpdateCollaboratorsCommand command = new(Id, collaborators, NewActor(actorId));

        Should.Throw<NoPermissionException>(() => album.UpdateCollaborators(command));
    }

    [DataRow(AdminId, true)]
    [DataRow(AuthorId, false)]
    [TestMethod]
    public void Raise_Event_When_Collaborators_Updated(long actorId, bool isAdmin)
    {
        Album album = ValidNewAlbum;
        Collaborators collaborators = NewCollaborators;
        UpdateCollaboratorsCommand command = new(Id, collaborators, NewActor(actorId, isAdmin));

        album.UpdateCollaborators(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumCollaboratorsUpdatedEvent>();
    }

    #endregion

    #region UpdateCover

    [TestMethod]
    public void Throw_When_UpdateCover_In_Immutable_Album()
    {
        Album album = ValidNewAlbum;
        album.SetValue("_isRemoved", true);
        UpdateCoverCommand command = new(Id, null, Author);

        Should.Throw<AlbumImmutableException>(() => album.UpdateCover(command));
    }

    [DataRow(VisitorId)]
    [DataRow(Collaborator1Id)]
    [TestMethod]
    public void Throw_When_UpdateCover_As_Not_Author_Or_Admin(long actorId)
    {
        Album album = ValidNewAlbum;
        UpdateCoverCommand command = new(Id, null, NewActor(actorId));

        Should.Throw<NoPermissionException>(() => album.UpdateCover(command));
    }

    [DataRow(AdminId, true)]
    [DataRow(AuthorId, false)]
    [TestMethod]
    public void Raise_Event_When_Cover_Updated(long actorId, bool isAdmin)
    {
        Album album = ValidNewAlbum;
        UpdateCoverCommand command = new(Id, ImageFileStream, NewActor(actorId, isAdmin));

        album.UpdateCover(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumCoverUpdatedEvent>();
    }

    #endregion

    #region AddImage

    [TestMethod]
    public void Throw_When_AddImage_In_Immutable_Album()
    {
        Album album = ValidNewAlbum;
        album.SetValue("_isRemoved", true);
        AddImageCommand command = new(Id, NewImageTitle, NewImageTags, ImageFileStream, Author);

        Should.Throw<AlbumImmutableException>(() => album.AddImage(command));
    }

    [TestMethod]
    public void Throw_When_AddImage_As_Not_Author_Or_Admin_Or_Collaborator()
    {
        Album album = ValidNewAlbum;
        AddImageCommand command =
            new(Id, NewImageTitle, NewImageTags, ImageFileStream, NewActor(VisitorId));

        Should.Throw<NoPermissionException>(() => album.AddImage(command));
    }

    [DataRow(AuthorId, false)]
    [DataRow(AdminId, true)]
    [DataRow(Collaborator1Id, false)]
    [DataRow(Collaborator2Id, true)]
    [TestMethod]
    public void Raise_Event_When_Image_Added(long actorId, bool isAdmin)
    {
        Album album = ValidNewAlbum;
        AddImageCommand command =
            new(Id, NewImageTitle, NewImageTags, ImageFileStream, NewActor(actorId, isAdmin));

        album.AddImage(command);

        album.DomainEvents.Count.ShouldBeGreaterThan(0);
        album.GetValue<List<Image>>().Count.ShouldBe(3);
    }

    [TestMethod]
    public void Raise_AlbumCoverUpdatedEvent_When_IsLatestImage_As_Image_Added()
    {
        Album album = ValidNewAlbum;
        AddImageCommand command = new(Id, NewImageTitle, NewImageTags, ImageFileStream, Author);

        album.AddImage(command);

        album.DomainEvents.Count.ShouldBe(2);
        album.DomainEvents.ShouldBeOfTypes(typeof(ImageAddedEvent), typeof(AlbumCoverUpdatedEvent));
    }

    [TestMethod]
    public void Not_Raise_AlbumCoverUpdatedEvent_When_Not_IsLatestImage_As_Image_Added()
    {
        Album album = ValidNewAlbum;
        album.SetValue(ImmutableCover);
        AddImageCommand command = new(Id, NewImageTitle, NewImageTags, ImageFileStream, Author);

        album.AddImage(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<ImageAddedEvent>();
    }

    #endregion

    #region RemoveImage


    [TestMethod]
    public void Throw_When_RemoveImage_In_Immutable_Album()
    {
        Album album = ValidNewAlbum;
        album.SetValue("_isRemoved", true);
        RemoveImageCommand command = new(Id, Image1.Id, Author);

        Should.Throw<AlbumImmutableException>(() => album.RemoveImage(command));
    }

    [TestMethod]
    public void Throw_When_RemoveImage_As_Not_Author_Or_Admin_Or_Collaborator()
    {
        Album album = ValidNewAlbum;
        RemoveImageCommand command = new(Id, Image1.Id, NewActor(VisitorId));

        Should.Throw<NoPermissionException>(() => album.RemoveImage(command));
    }

    [TestMethod]
    public void Raise_CoverUpdatedEvent_When_Image_As_LatestImage_Removed()
    {
        Album album = ValidNewAlbum;
        album.SetValue(new Cover(Image2.Id, true));
        RemoveImageCommand command = new(Id, Image2.Id, Author);

        album.RemoveImage(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumCoverUpdatedEvent>();
        album.GetValue<Cover>().Id.ShouldBe(Image1.Id);
    }

    [TestMethod]
    public void Not_Raise_CoverUpdatedEvent_When_Image_Not_As_LatestImage_Removed()
    {
        Album album = ValidNewAlbum;
        RemoveImageCommand command = new(Id, Image2.Id, Author);

        album.RemoveImage(command);

        album.DomainEvents.Count.ShouldBe(0);
    }

    #endregion

    #region RestoreImage

    [TestMethod]
    public void Throw_When_RestoreImage_In_Immutable_Album()
    {
        Album album = ValidNewAlbum;
        album.SetValue("_isRemoved", true);
        RestoreImageCommand command = new(Id, Image1.Id, Author);

        Should.Throw<AlbumImmutableException>(() => album.RestoreImage(command));
    }

    [TestMethod]
    public void Throw_When_RestoreImage_As_Not_Author_Or_Admin_Or_Collaborator()
    {
        Album album = ValidNewAlbum;
        RestoreImageCommand command = new(Id, Image1.Id, NewActor(VisitorId));

        Should.Throw<NoPermissionException>(() => album.RestoreImage(command));
    }

    [TestMethod]
    public void Raise_CoverUpdatedEvent_When_Image_As_LatestImage_Restored()
    {
        Album album = ValidNewAlbum;
        Image2.SetValue(true); // _isRemoved = true;
        RestoreImageCommand command = new(Id, Image2.Id, Author);

        album.RestoreImage(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumCoverUpdatedEvent>();
        album.GetValue<Cover>().Id.ShouldBe(Image2.Id);
    }

    [TestMethod]
    public void Not_Raise_CoverUpdatedEvent_When_Image_Not_As_LatestImage_Restored()
    {
        Album album = ValidNewAlbum;
        album.SetValue(new Cover(null, false));
        RestoreImageCommand command = new(Id, Image2.Id, Author);

        album.RestoreImage(command);

        album.DomainEvents.Count.ShouldBe(0);
    }

    #endregion
}

internal static class AlbumTestsHelper
{
    public static readonly AlbumId Id = AlbumId.GenerateNew();

    public static Actor NewActor(long id, bool isAdmin = false) =>
        new()
        {
            Id = new(id),
            IsAdmin = isAdmin,
            IsAuthenticated = true,
        };

    public const long Subscriber1Id = 9;
    public const long Subscriber2Id = 8;
    public const long NewSubscriberId = 7;
    public static readonly Subscribe Subscriber1 = new(Id, new(Subscriber1Id));
    public static readonly Subscribe Subscriber2 = new(Id, new(Subscriber2Id));
    public static readonly Subscribe NewSubscriber = new(Id, new(NewSubscriberId));
    public static List<Subscribe> OriginalSubscribers => [Subscriber1, Subscriber2];

    public static readonly Stream ImageFileStream = new MemoryStream(1);

    public const long Image1Id = 1111111;
    public const long Image2Id = 2222222;
    public static readonly Image Image1 = ImageTestsHelper.CreateNewImage(Image1Id);
    public static readonly Image Image2 = ImageTestsHelper.CreateNewImage(Image2Id);
    public static List<Image> OriginalImages => [Image1, Image2];

    public static readonly ImageTitle NewImageTitle = new("new_title");
    public static readonly ImageTags NewImageTags = new([new(741), new(852)]);

    public static readonly Cover DefaultCover = Cover.Default;
    public static readonly Cover ImmutableCover = Cover.UserCustomCover;

    public const long Collaborator1Id = 1;
    public const long Collaborator2Id = 2;
    public static readonly Collaborators OriginalCollaborators =
        new([new(Collaborator1Id), new(Collaborator2Id)]);
    public static readonly Collaborators NewCollaborators =
        new([new(Collaborator1Id), new(Collaborator2Id), new(3)]);
    public static readonly Collaborators EmptyCollaborators = [];

    public const long AuthorId = 11111;
    public const long AdminId = 99999;
    public const long VisitorId = 55555;
    public static readonly Actor Author = NewActor(AuthorId);
    public static readonly Actor Adm = NewActor(AdminId, true);
    public static readonly Actor Visitor = NewActor(VisitorId);

    public static readonly AlbumTitle OriginalTitle = new("original_title");
    public static readonly AlbumTitle NewTitle = new("new_title");
    public static readonly CategoryId NewCategory = new(2222222222);
    public static readonly AlbumDescription NewDescription = new("new_description");
    public static readonly Accessibility NewAccessibility = Accessibility.AuthOnly;

    public static T GetProperty<T>(this Album album, string propertyName)
    {
        var property = typeof(Album)
            .GetProperties(BindingFlags.NonPublic | BindingFlags.Instance)
            .First(p =>
                p.PropertyType == typeof(T)
                && p.Name.Contains(propertyName, StringComparison.OrdinalIgnoreCase)
            );
        var value = property.GetValue(album);

        Assert.IsNotNull(value);

        return (T)value;
    }

    public static T GetValue<T>(this Album album)
    {
        var field = typeof(Album)
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .First(f => f.FieldType == typeof(T));
        var value = field.GetValue(album);

        Assert.IsNotNull(value);

        return (T)value;
    }

    public static void SetValue<T>(this Album album, string fieldName, T value)
    {
        var field = typeof(Album)
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .First(f =>
                f.FieldType == typeof(T)
                && f.Name.Contains(fieldName, StringComparison.OrdinalIgnoreCase)
            );

        Assert.IsNotNull(field);

        field.SetValue(album, value);
    }

    public static void SetValue<T>(this Album album, T value)
    {
        var field = typeof(Album)
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .First(f => f.FieldType == typeof(T));

        Assert.IsNotNull(field);

        field.SetValue(album, value);
    }

    public static Album ValidNewAlbum => CreateNewAlbumFromReflection();

    private static Album CreateNewAlbumFromReflection()
    {
        var constructor = typeof(Album).GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            Type.EmptyTypes
        );

        Assert.IsNotNull(constructor);

        var album = (Album)constructor.Invoke([]);

        album.SetValue(Author.Id);
        album.SetValue(OriginalTitle);
        album.SetValue(DefaultCover);
        album.SetValue(OriginalSubscribers);
        album.SetValue(OriginalImages);
        album.SetValue(OriginalCollaborators.ToArray());

        typeof(EntityBase<AlbumId>)
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .First(f => f.Name.Contains("id", StringComparison.OrdinalIgnoreCase))
            .SetValue(album, Id);

        return album;
    }
}
