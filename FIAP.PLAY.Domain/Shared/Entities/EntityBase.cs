using System.ComponentModel.DataAnnotations;

namespace FIAP.PLAY.Domain.Shared.Entities
{
    public class EntityBase
    {
        [Key]
        public long Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
