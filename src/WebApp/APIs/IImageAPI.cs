namespace WebApp.APIs;

public interface IImageAPI
{
    public const string Base = "images";

    public static string GetImage(long i) => $"{APIConfigurations.BaseUrl}{Base}/{i}";
}
