using System;
using Unity.VisualScripting;
public class Users_Data
{
    public string userName;
    public string password;
    public string verificationToken;
    
    public Users_Data()
    {
        this.userName = "";
        this.password = "";
        this.verificationToken = "";
    }

    public Users_Data(string userName, string password, string verificationToken)
    {
        this.userName = userName;
        this.password = password;
        this.verificationToken = verificationToken;
    }
    protected bool Equals(Users_Data other)
    {
        return userName == other.userName && password == other.password && verificationToken == other.verificationToken;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Users_Data)obj);
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
