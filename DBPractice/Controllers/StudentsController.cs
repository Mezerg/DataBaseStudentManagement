﻿using DBPractice.Data;
using DBPractice.Models;
using DBPractice.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace DBPractice.Controllers
{
	public class StudentsController : Controller
	{
		private readonly ApplicationDbContext dbContext;

		public StudentsController(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}
		[HttpGet]
		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddStudentViewModel viewModel)
		{
			var studen = new Student
			{
				Name = viewModel.Name,
				Email = viewModel.Email,
				Phone = viewModel.Phone,
				Subscribed = viewModel.Subscribed,
			};
			await dbContext.AddAsync(studen);
			await dbContext.SaveChangesAsync();
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> List()
		{
			var studens = await dbContext.Students.ToListAsync();
			return View(studens);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(Guid id)
		{
			var student = await dbContext.Students.FindAsync(id);
			return View(student);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(Student viewModel)
		{
			var student = await dbContext.Students.FindAsync(viewModel.Id);
			if (student is not null)
			{
				student.Name = viewModel.Name;
				student.Email = viewModel.Email;
				student.Phone = viewModel.Phone;
				student.Subscribed = viewModel.Subscribed;
				await dbContext.SaveChangesAsync();
			}
			return RedirectToAction("List", "Students");
		}
		[HttpPost]
		public async Task<IActionResult> Delete(Student viewModel)
		{
			var student = await dbContext.Students
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == viewModel.Id);
			if (student is not null)
			{
				dbContext.Students.Remove(viewModel);
				await dbContext.SaveChangesAsync();
			}
			return RedirectToAction("List", "Students");
		}

	}
}
