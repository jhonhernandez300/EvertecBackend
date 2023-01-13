using System.ComponentModel.DataAnnotations;

namespace EvertekBackend.Models
{
    public class Employees
    {
        [Key]
        public int IdEmployee { get; set; }
        public string Names { get; set; }
        public string LastNames { get; set; }
        public string PhotoLocation { get; set; }
        public int Married { get; set; }
        public int HasBrothersOrSisters { get; set; }        
        public DateTime DateOfBirth { get; set; }
        
    }
}
