﻿using System.Reflection;
using Domain.AlbumDomain.AlbumEntity;
using Domain.UserDomain.UserEntity;
using Shouldly;

namespace Domain.Tests.AlbumEntity;

[TestClass]
public class CollaboratorsTests
{
    private static UserId NewCollaborator(long id)
    {
        UserId userId = new();
        var valueField = typeof(UserId)
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .First(f => f.Name.Contains(nameof(userId.Value)));

        object boxedUserId = userId;
        valueField.SetValue(boxedUserId, id);
        userId = (UserId)boxedUserId;

        return userId;
    }

    [TestMethod]
    public void Return_False_When_Too_Many_Collaborators()
    {
        long[] collaborators_more_than_MaxCount = Enumerable
            .Range(default, Collaborators.MaxCount + 1)
            .Select(index => (long)index)
            .ToArray();

        bool result = Collaborators.TryCreateNew(
            collaborators_more_than_MaxCount,
            out var collaborators
        );

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void Return_True_When_Create_From_Valid()
    {
        const int collaborator_count = Collaborators.MaxCount - 1;

        long[] valid_Collaborators = Enumerable
            .Range(default, Collaborators.MaxCount - 1)
            .Select(index => (long)index)
            .ToArray();

        Collaborators.TryCreateNew(valid_Collaborators, out var collaborators);

        collaborators!.Count.ShouldBe(collaborator_count);
    }

    [TestMethod]
    public void Return_True_When_Count_Valid_After_Remove_Duplicate()
    {
        const int duplicate_id = 0;
        const int duplicate_count = 10;
        var collaborators_less_than_MaxCount = Enumerable
            .Range(default, Collaborators.MaxCount - 1)
            .Select(index => (long)index);
        var duplicate_collaborators = Enumerable.Repeat((long)duplicate_id, duplicate_count);
        long[] total_collaborators = collaborators_less_than_MaxCount
            .Concat(duplicate_collaborators)
            .ToArray();

        bool result = Collaborators.TryCreateNew(total_collaborators, out var _);

        result.ShouldBeTrue();
    }

    [TestMethod]
    public void Should_Not_Contain_Duplicate_Collaborators()
    {
        const int duplicate_id = 0;
        const int duplicate_count = Collaborators.MaxCount / 2;
        long[] collaborators_with_duplicate_ones = Enumerable
            .Repeat((long)duplicate_id, duplicate_count)
            .ToArray();

        _ = Collaborators.TryCreateNew(collaborators_with_duplicate_ones, out var collaborators);

        collaborators!.ShouldBeUnique();
    }

    [TestMethod]
    public void Should_Have_Same_Count_When_Create_From_No_Duplicate()
    {
        const int collaborator_count = Collaborators.MaxCount - 1;

        long[] valid_Collaborators = Enumerable
            .Range(default, Collaborators.MaxCount - 1)
            .Select(index => (long)index)
            .ToArray();

        Collaborators.TryCreateNew(valid_Collaborators, out var collaborators);

        collaborators!.Count.ShouldBe(collaborator_count);
    }

    [TestMethod]
    public void Return_True_When_Create_From_Empty()
    {
        long[] empty_collaborators = Collaborators.Empty.Select(c => c.Value).ToArray();

        bool result = Collaborators.TryCreateNew(empty_collaborators, out var _);

        result.ShouldBeTrue();
    }

    [TestMethod]
    public void Return_True_When_Create_From_MaxCount()
    {
        long[] maxcount_collaborators = Enumerable
            .Range(default, Collaborators.MaxCount)
            .Select(index => (long)index)
            .ToArray();

        bool result = Collaborators.TryCreateNew(maxcount_collaborators, out var _);

        result.ShouldBeTrue();
    }
}
