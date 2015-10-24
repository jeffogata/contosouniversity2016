using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Features.Department
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using AutoMapper;
    using DataAccess;
    using MediatR;
    using Models;

    public class Create
    {
        public class Command : IAsyncRequest
        {
            [StringLength(50, MinimumLength = 3)]
            public string Name { get; set; }

            [DataType(DataType.Currency)]
            [Column(TypeName = "money")]
            public decimal? Budget { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime? StartDate { get; set; }

            public Instructor Administrator { get; set; }
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly ContosoUniversityContext _context;

            public CommandHandler(ContosoUniversityContext context)
            {
                _context = context;
            }

            protected async override Task HandleCore(Command message)
            {
                var department = Mapper.Map<Command, Department>(message);

                _context.Departments.Add(department);

                await _context.SaveChangesAsync();
            }
        }
    }
}
