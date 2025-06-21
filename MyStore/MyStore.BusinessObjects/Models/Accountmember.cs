using System.ComponentModel.DataAnnotations;

namespace MyStore.BusinessObjects.Models
{
    public partial class AccountMember
    {
        [Key]
        public int MemberId { get; set; }
        public string MemberPassword { get; set; } = null!;
        public string? FullName { get; set; }
        public string? EmailAddress { get; set; }
        public string? MemberRole { get; set; }
    }
}