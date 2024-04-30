public class VideoGames_Data
{
    public string name;
    public string releaseYear;
    public string developer;

    public VideoGames_Data()
    {
        this.name = "";
        this.releaseYear = "";
        this.developer = "";
    }

    public VideoGames_Data(string name, string releaseYear, string developer)
    {
        this.name = name;
        this.releaseYear = releaseYear;
        this.developer = developer;
    }
}