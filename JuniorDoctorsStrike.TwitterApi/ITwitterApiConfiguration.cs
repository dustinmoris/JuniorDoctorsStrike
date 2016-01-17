namespace JuniorDoctorsStrike.TwitterApi
{
    public interface ITwitterApiConfiguration
    {
        string BaseUrl { get; }
        string AccessToken { get; }
    }
}