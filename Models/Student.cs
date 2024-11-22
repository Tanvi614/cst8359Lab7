using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Lab7.Models
{
	public class Student
	{
		[Key]
		[SwaggerSchema(ReadOnly = true)]
		public Guid ID { get; set; } 

		[Required]
		[StringLength(50)]
		public string FirstName { get; set; }

		[Required]
		[StringLength(50)]
		public string LastName { get; set; }

		[Required]
		[StringLength(50)]
		public string Program { get; set; }
	}
}
