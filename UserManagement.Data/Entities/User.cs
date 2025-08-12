using System;
using System.Collections.Generic;

namespace UserManagement.Models;

public class User
{
    public int Id { get; set; }
    public string Forename { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public string Email { get; set; } = default!;
    public DateOnly DateOfBirth { get; set; } = default!;
    public bool IsActive { get; set; }
    public  IEnumerable<Log> Logs { get; set; } = new List<Log>();
}
