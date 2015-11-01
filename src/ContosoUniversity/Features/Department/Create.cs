namespace ContosoUniversity.Features.Department
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using DataAccess;
    using Infrastructure;
    using MediatR;
    using Microsoft.AspNet.Mvc.Rendering;
    using Microsoft.Data.Entity;
    using Models;
    using Newtonsoft.Json;

    public class DepartmentCreateQueryResponse
    {
        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Budget { get; set; }
        // need the DataType attribute to have the datepicker rendered
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Administrator")]
        public int? InstructorId { get; set; }
        public List<SelectListItem> Instructors { get; set; }

        public class Instructor
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public string FullName
            {
                get { return $"{LastName}, {FirstName}"; }
            }
        }
    }

    public class CourseCreateQueryHandler : CreateQueryHandler<Department, DepartmentCreateQueryResponse>
    {
        public CourseCreateQueryHandler(ContosoUniversityContext dbContext) : base(dbContext)
        {
        }

        protected override async Task ModifyResponse(DepartmentCreateQueryResponse response)
        {
            /*
                wanted to do:

                    var instructors = await _dbContext.Instructors
                        .OrderBy(i => i.LastName)
                        .ProjectTo<Instructor>()
                        .ToListAsync();

                but this returns instructors and students.  This
                works fine in the HS Contoso example against EF 6.
            */

            var instructors = await DbContext
                .Instructors
                .OrderBy(i => i.LastName)
                .ToListAsync();

            var result = Mapper.Map<List<DepartmentCreateQueryResponse.Instructor>>(instructors);

            var items = result.Select(x =>
                new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.FullName
                }).ToList();

            var nullItem = new SelectListItem
            {
                Value = "",
                Text = "",
                Selected = true
            };

            items.Insert(0, nullItem);

            response.StartDate = DateTime.Now.Date;
            response.Instructors = items;
        }
    }

    public class Create
    {
        public class Command : IAsyncRequest<int>
        {
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("budget")]
            public decimal? Budget { get; set; }
            [JsonProperty("startDate")]
            public DateTime? StartDate { get; set; }
            [JsonProperty("instructorId")]
            public int? InstructorId { get; set; }
        }

        public class CommandHandler : MediatorHandler<Command, int>
        {
            public CommandHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            public override async Task<int> Handle(Command message)
            {
                var department = Mapper.Map<Command, Department>(message);

                DbContext.Departments.Add(department);

                return await DbContext.SaveChangesAsync();
            }
        }
    }
}