namespace DoAnFlutterAPI.DTOs;

public class AdminLoginDto
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class AdminCreateDto
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? FullName { get; set; }
    public bool IsActive { get; set; } = true;
}

public class AdminUpdateDto
{
    public string? Password { get; set; }
    public string? FullName { get; set; }
    public bool? IsActive { get; set; }
}

public class AdminDto
{
    public int AdminId { get; set; }
    public string Username { get; set; } = null!;
    public string? FullName { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}
