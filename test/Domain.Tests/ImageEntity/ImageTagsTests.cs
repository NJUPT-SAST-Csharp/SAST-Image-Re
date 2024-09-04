using Domain.AlbumDomain.ImageEntity;
using Shouldly;

namespace Domain.Tests.ImageEntity
{
    [TestClass]
    public class ImageTagsTests
    {
        [TestMethod]
        public void Return_False_When_Too_Many_ImageTags()
        {
            var imageTags_more_than_MaxCount = Enumerable
                .Range(default, ImageTags.MaxCount + 1)
                .Select(index => (long)index)
                .ToArray();

            var result = ImageTags.TryCreateNew(imageTags_more_than_MaxCount, out var imageTags);

            result.ShouldBeFalse();
        }

        [TestMethod]
        public void Return_True_When_Create_From_Valid()
        {
            const int collaborator_count = ImageTags.MaxCount - 1;

            var valid_ImageTags = Enumerable
                .Range(default, ImageTags.MaxCount - 1)
                .Select(index => (long)index)
                .ToArray();

            ImageTags.TryCreateNew(valid_ImageTags, out var imageTags);

            imageTags!.Count.ShouldBe(collaborator_count);
        }

        [TestMethod]
        public void Return_True_When_Count_Valid_After_Remove_Duplicate()
        {
            const int duplicate_id = 0;
            const int duplicate_count = 10;
            var imageTags_less_than_MaxCount = Enumerable
                .Range(default, ImageTags.MaxCount - 1)
                .Select(index => (long)index);
            var duplicate_imageTags = Enumerable.Repeat((long)duplicate_id, duplicate_count);
            var total_imageTags = imageTags_less_than_MaxCount
                .Concat(duplicate_imageTags)
                .ToArray();

            var result = ImageTags.TryCreateNew(total_imageTags, out var _);

            result.ShouldBeTrue();
        }

        [TestMethod]
        public void Should_Not_Contain_Duplicate_ImageTags()
        {
            const int duplicate_id = 0;
            const int duplicate_count = ImageTags.MaxCount / 2;
            var imageTags_with_duplicate_ones = Enumerable
                .Repeat((long)duplicate_id, duplicate_count)
                .ToArray();

            _ = ImageTags.TryCreateNew(imageTags_with_duplicate_ones, out var imageTags);

            imageTags!.ShouldBeUnique();
        }

        [TestMethod]
        public void Should_Have_Same_Count_When_Create_From_No_Duplicate()
        {
            const int collaborator_count = ImageTags.MaxCount - 1;

            var valid_ImageTags = Enumerable
                .Range(default, ImageTags.MaxCount - 1)
                .Select(index => (long)index)
                .ToArray();

            ImageTags.TryCreateNew(valid_ImageTags, out var imageTags);

            imageTags!.Count.ShouldBe(collaborator_count);
        }

        [TestMethod]
        public void Return_True_When_Create_From_Empty()
        {
            var empty_imageTags = ImageTags.Empty.Select(i => i.Value).ToArray();

            var result = ImageTags.TryCreateNew(empty_imageTags, out var _);

            result.ShouldBeTrue();
        }

        [TestMethod]
        public void Return_True_When_Create_From_MaxCount()
        {
            var maxcount_imageTags = Enumerable
                .Range(default, ImageTags.MaxCount)
                .Select(index => (long)index)
                .ToArray();

            var result = ImageTags.TryCreateNew(maxcount_imageTags, out var _);

            result.ShouldBeTrue();
        }
    }
}
