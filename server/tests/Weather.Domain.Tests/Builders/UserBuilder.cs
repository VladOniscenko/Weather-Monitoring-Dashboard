using Weather.Domain.Entities;

public class UserBuilder
{
    private string _email = "test@email.com";
    private string _name = "test";

    public UserBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UserBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public User Build()
    {
        return new User(_name, _email, "password_hash");
    }
}