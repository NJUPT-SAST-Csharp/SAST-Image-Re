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
                .Select(index => index.ToString())
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
                .Select(index => index.ToString())
                .ToArray();

            ImageTags.TryCreateNew(valid_ImageTags, out var imageTags);

            imageTags.Value.Length.ShouldBe(collaborator_count);
        }

        [TestMethod]
        public void Return_True_When_Count_Valid_After_Remove_Duplicate()
        {
            const string duplicate_tag = "1";
            const int duplicate_count = 10;
            var imageTags_less_than_MaxCount = Enumerable
                .Range(default, ImageTags.MaxCount - 1)
                .Select(index => index.ToString());
            var duplicate_imageTags = Enumerable.Repeat(duplicate_tag, duplicate_count);
            var total_imageTags = imageTags_less_than_MaxCount
                .Concat(duplicate_imageTags)
                .ToArray();

            var result = ImageTags.TryCreateNew(total_imageTags, out var _);

            result.ShouldBeTrue();
        }

        [TestMethod]
        public void Should_Not_Contain_Duplicate_ImageTags()
        {
            const string duplicate_tag = "rua";
            const int duplicate_count = ImageTags.MaxCount / 2;
            var imageTags_with_duplicate_ones = Enumerable
                .Repeat(duplicate_tag, duplicate_count)
                .ToArray();

            _ = ImageTags.TryCreateNew(imageTags_with_duplicate_ones, out var imageTags);

            imageTags.Value.ShouldBeUnique();
        }

        [TestMethod]
        public void Should_Have_Same_Count_When_Create_From_No_Duplicate()
        {
            const int collaborator_count = ImageTags.MaxCount - 1;

            var valid_ImageTags = Enumerable
                .Range(default, ImageTags.MaxCount - 1)
                .Select(index => index.ToString())
                .ToArray();

            ImageTags.TryCreateNew(valid_ImageTags, out var imageTags);

            imageTags.Value.Length.ShouldBe(collaborator_count);
        }

        [TestMethod]
        public void Return_True_When_Create_From_Empty()
        {
            string[] empty_imageTags = [];

            var result = ImageTags.TryCreateNew(empty_imageTags, out var _);

            result.ShouldBeTrue();
        }

        [TestMethod]
        public void Return_True_When_Create_From_MaxCount()
        {
            var maxcount_imageTags = Enumerable
                .Range(default, ImageTags.MaxCount)
                .Select(index => index.ToString())
                .ToArray();

            var result = ImageTags.TryCreateNew(maxcount_imageTags, out var _);

            result.ShouldBeTrue();
        }
    }
}
