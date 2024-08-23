using Domain.AlbumDomain.AlbumEntity;
using Shouldly;

namespace Domain.Tests.AlbumEntity
{
    [TestClass]
    public class AccessibilityTests
    {
        [DataRow(Accessibility.MaxValue + 1)]
        [DataRow(Accessibility.MaxValue + 999)]
        [DataRow(Accessibility.MinValue - 1)]
        [DataRow(Accessibility.MinValue - 999)]
        [TestMethod]
        public void Return_False_When_Create_From_OutOfRange(int value)
        {
            bool result = Accessibility.TryCreateNew(value, out var _);

            result.ShouldBeFalse();
        }

        [DataRow(Accessibility.MaxValue)]
        [TestMethod]
        public void Should_Set_Value_When_Create_From_Valid(int value)
        {
            bool result = Accessibility.TryCreateNew(value, out var accessibility);

            result.ShouldBeTrue();
            accessibility.ShouldNotBe(default);
        }

        [TestMethod]
        public void Should_Be_Equal_When_Same_Value()
        {
            int value = (Accessibility.MinValue + Accessibility.MinValue) / 2;
            Accessibility.TryCreateNew(value, out var accessibility1);
            Accessibility.TryCreateNew(value, out var accessibility2);

            accessibility1.ShouldBe(accessibility2);
        }

        [TestMethod]
        public void Should_Not_Be_Equal_When_Different_Value()
        {
            int value1 = Accessibility.MinValue;
            int value2 = Accessibility.MaxValue;
            Accessibility.TryCreateNew(value1, out var accessibility1);
            Accessibility.TryCreateNew(value2, out var accessibility2);

            accessibility1.ShouldNotBe(accessibility2);
        }
    }
}
