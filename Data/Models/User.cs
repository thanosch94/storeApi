
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreApi.Data.Models
{

    [Table("AspNetUsers")]
    public class User : IdentityUser<Guid>
    {
        public int? SerialNumber { get; set; }

        [StringLength(25)]
        public string? Code { get; set; }

        public string Name { get; set; }
        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public DateTime DateAdded { get; set; } = DateTime.Now;

        public Guid UserAdded { get; set; }

        public DateTime? DateUpdated { get; set; }

        public Guid? UserUpdated { get; set; }

        //public Guid? UserRole { get; set; } //User Roles are saved in another table

        public string? Country { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Address { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Notes { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? Occupation { get; set; }


    }
}
