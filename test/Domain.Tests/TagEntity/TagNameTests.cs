using Domain.TagEntity;
using Shouldly;

namespace Domain.Tests.TagEntity
{
    [TestClass]
    public class TagNameTests
    {
        [TestMethod]
        public void Return_False_When_Create_From_Empty()
        {
            const string value = "";

            var result = TagName.TryCreateNew(value, out var _);

            result.ShouldBeFalse();
        }

        [TestMethod]
        public void Return_False_When_Create_From_Whitespace()
        {
            const string value = "    ";

            var result = TagName.TryCreateNew(value, out var _);

            result.ShouldBeFalse();
        }

        [TestMethod]
        public void Return_False_When_Create_From_Too_Long()
        {
            string value = new('a', TagName.MaxLength + 1);

            var result = TagName.TryCreateNew(value, out var _);

            result.ShouldBeFalse();
        }

        [TestMethod]
        public void Return_True_When_Create_From_MaxLength()
        {
            string value = new('a', TagName.MaxLength);

            var result = TagName.TryCreateNew(value, out var _);

            result.ShouldBeTrue();
        }

        [TestMethod]
        public void Return_True_When_Create_From_Valid()
        {
            const string value = "tag";

            var result = TagName.TryCreateNew(value, out var _);

            result.ShouldBeTrue();
        }

        [TestMethod]
        public void Should_Trim_Whitespace_When_Create()
        {
            const string value = "  tag  ";

            TagName.TryCreateNew(value, out var tag);

            tag.Value.ShouldBe("tag");
        }

        [TestMethod]
        public void Should_Set_Value_When_Create_From_Valid()
        {
            const string value = "tag";

            TagName.TryCreateNew(value, out var tag);

            tag.Value.ShouldBe(value);
        }

        [TestMethod]
        public void Should_Be_Equal_When_Same_Value()
        {
            const string value = "tag";

            TagName.TryCreateNew(value, out var tag1);
            TagName.TryCreateNew(value, out var tag2);

            tag1.ShouldBe(tag2);
        }

        [TestMethod]
        public void Should_Be_Equal_When_Same_Value_With_Whitespace()
        {
            const string value1 = "tag";
            const string value2 = "tag ";

            TagName.TryCreateNew(value1, out var tag1);
            TagName.TryCreateNew(value2, out var tag2);

            tag1.ShouldBe(tag2);
        }

        [TestMethod]
        public void Should_Not_Be_Equal_When_Different_Value()
        {
            const string value1 = "tag1";
            const string value2 = "tag2";

            TagName.TryCreateNew(value1, out var tag1);
            TagName.TryCreateNew(value2, out var tag2);

            tag1.ShouldNotBe(tag2);
        }
    }
}
