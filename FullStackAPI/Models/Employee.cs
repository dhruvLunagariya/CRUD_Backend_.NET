using System.ComponentModel.DataAnnotations;

namespace FullStackAPI.Models
{
	public class Employee
	{
		[Key]
		public string Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public long Phone { get; set; }
		public string Department { get; set; }

		public string Country {  get; set; }
		public string State { get; set; }
	}
}
