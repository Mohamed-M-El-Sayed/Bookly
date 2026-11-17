namespace Bookly.Domain.Entities.Hotels;

public class ContactInfo
{
    public string Phone { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string? Website { get; private set; }

    private ContactInfo() { }

    public ContactInfo(
        string phone,
        string email,
        string? website = null)
    {
        Phone = phone;
        Email = email;
        Website = website;
    }
}
