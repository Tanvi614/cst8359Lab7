using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab7.Data;
using Lab7.Models;

namespace Lab7.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StudentsController : ControllerBase
	{
		private readonly StudentDbContext _context;

		public StudentsController(StudentDbContext context)
		{
			_context = context;
		}

		// GET: api/Students
		/// <summary>
		/// Retrieves all students from the database.
		/// </summary>
		/// <returns>A collection of students.</returns>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)] // Successfully retrieved the list of students
		[ProducesResponseType(StatusCodes.Status500InternalServerError)] // An error occurred while fetching the data
		public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
		{
			return Ok(await _context.Students.ToListAsync());
		}

		// GET: api/Students/{id}
		/// <summary>
		/// Retrieves a specific student by their ID.
		/// </summary>
		/// <param name="id">The unique ID of the student.</param>
		/// <returns>The requested student.</returns>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)] // Successfully retrieved the student details
		[ProducesResponseType(StatusCodes.Status404NotFound)] // No student found with the provided ID
		[ProducesResponseType(StatusCodes.Status500InternalServerError)] // An error occurred while fetching the student data
		public async Task<ActionResult<Student>> GetStudent(Guid id)
		{
			var student = await _context.Students.FindAsync(id);

			if (student == null)
			{
				return NotFound();
			}

			return Ok(student);
		}

		// PUT: api/Students/{id}
		/// <summary>
		/// Updates an existing student.
		/// </summary>
		/// <param name="id">The unique ID of the student.</param>
		/// <param name="student">The updated student data.</param>
		/// <returns>The updated student.</returns>
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)] // Successfully updated the student
		[ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad request, likely due to mismatched IDs
		[ProducesResponseType(StatusCodes.Status404NotFound)] // The student with the provided ID was not found
		[ProducesResponseType(StatusCodes.Status500InternalServerError)] // An error occurred while updating the student
		public async Task<IActionResult> PutStudent(Guid id, Student student)
		{
			if (id != student.ID)
			{
				return BadRequest();
			}

			_context.Entry(student).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!StudentExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return Ok(student);
		}

		// POST: api/Students
		/// <summary>
		/// Creates a new student.
		/// </summary>
		/// <param name="student">The student object to create.</param>
		/// <returns>The created student object.</returns>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)] // Successfully created the new student
		[ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad request, likely due to invalid data
		[ProducesResponseType(StatusCodes.Status500InternalServerError)] // An error occurred while creating the student
		public async Task<ActionResult<Student>> PostStudent(Student student)
		{
			_context.Students.Add(student);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetStudent", new { id = student.ID }, student);
		}

		// DELETE: api/Students/{id}
		/// <summary>
		/// Deletes a specific student.
		/// </summary>
		/// <param name="id">The unique ID of the student.</param>
		/// <returns>Confirmation of deletion.</returns>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)] // Successfully deleted the student
		[ProducesResponseType(StatusCodes.Status404NotFound)] // No student found with the provided ID
		[ProducesResponseType(StatusCodes.Status500InternalServerError)] // An error occurred while deleting the student
		public async Task<IActionResult> DeleteStudent(Guid id)
		{
			var student = await _context.Students.FindAsync(id);
			if (student == null)
			{
				return NotFound();
			}

			_context.Students.Remove(student);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool StudentExists(Guid id)
		{
			return _context.Students.Any(e => e.ID == id);
		}
	}
}
