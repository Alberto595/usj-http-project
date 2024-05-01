using System;
using Unity.VisualScripting;

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

    protected bool Equals(VideoGames_Data other)
    {
        return name == other.name && releaseYear == other.releaseYear && developer == other.developer;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((VideoGames_Data)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(name, releaseYear, developer);
    }
}