using Domain.AlbumEntity;
using Shouldly;

namespace Domain.Tests.AlbumEntity
{
    [TestClass]
    public class AlbumTitleTests
    {
        [TestMethod]
        public void Return_False_When_Create_From_Empty()
        {
            const string value = "";

            var result = AlbumTitle.TryCreateNew(value, out var _);

            result.ShouldBeFalse();
        }

        [TestMethod]
        public void Return_False_When_Create_From_Whitespace()
        {
            const string value = "    ";

            var result = AlbumTitle.TryCreateNew(value, out var _);

            result.ShouldBeFalse();
        }

        [TestMethod]
        public void Return_False_When_Create_From_Too_Long()
        {
            string value = new('a', AlbumTitle.MaxLength + 1);

            var result = AlbumTitle.TryCreateNew(value, out var _);

            result.ShouldBeFalse();
        }

        [TestMethod]
        public void Return_True_When_Create_From_MaxLength()
        {
            string value = new('a', AlbumTitle.MaxLength);

            var result = AlbumTitle.TryCreateNew(value, out var _);

            result.ShouldBeTrue();
        }

        [TestMethod]
        public void Return_True_When_Create_From_Valid()
        {
            const string value = "album";

            var result = AlbumTitle.TryCreateNew(value, out var _);

            result.ShouldBeTrue();
        }

        [TestMethod]
        public void Should_Trim_Whitespace_When_Create()
        {
            const string input_value = "  album  ";
            const string actual_value = "album";

            AlbumTitle.TryCreateNew(input_value, out var album);

            album.Value.ShouldBe(actual_value);
        }

        [TestMethod]
        public void Should_Set_Value_When_Create_From_Valid()
        {
            const string value = "album";

            AlbumTitle.TryCreateNew(value, out var album);

            album.Value.ShouldBe(value);
        }

        [TestMethod]
        public void Should_Be_Equal_When_Same_Value()
        {
            const string value = "album";

            AlbumTitle.TryCreateNew(value, out var album1);
            AlbumTitle.TryCreateNew(value, out var album2);

            album1.ShouldBe(album2);
        }

        [TestMethod]
        public void Should_Be_Equal_When_Same_Value_With_Whitespace()
        {
            const string value1 = "album";
            const string value2 = "album ";

            AlbumTitle.TryCreateNew(value1, out var album1);
            AlbumTitle.TryCreateNew(value2, out var album2);

            album1.ShouldBe(album2);
        }

        [TestMethod]
        public void Should_Not_Be_Equal_When_Different_Value()
        {
            const string value1 = "album1";
            const string value2 = "album2";

            AlbumTitle.TryCreateNew(value1, out var album1);
            AlbumTitle.TryCreateNew(value2, out var album2);

            album1.ShouldNotBe(album2);
        }
    }
}
