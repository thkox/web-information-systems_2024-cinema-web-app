namespace cinema_web_app.Models;

public class Cinema
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string Email { get; set; }

    // Navigation properties
    public ICollection<ScreeningRoom> ScreeningRooms { get; set; }
    public ICollection<Announcement> Announcements { get; set; }
    public ICollection<ContentCinemaAdmin> ContentCinemaAdmins { get; set; }
}