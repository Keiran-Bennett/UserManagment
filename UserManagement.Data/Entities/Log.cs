using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Models;

public class Log
{
    public int Id { get; set; }
    public int UserID { get; set; } = default!;
    public DateTime DateofAction { get; set; } = default!;
    public string Details { get; set; } = default!;
}
