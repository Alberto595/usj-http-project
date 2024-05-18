using System;
using Unity.VisualScripting;
public class Users_Data
{
    public string userName;
    public string password;
    public string verificationToken;
    
    protected bool Equals(Users_Data other)
    {
        return userName == other.userName && password == other.password && verificationToken == other.verificationToken;
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
        return HashCode.Combine(userName, password, verificationToken);
    }

    public bool IsEmpty()
    {
        return userName == "" && password == "" && verificationToken == "";
    }
}
