//ToDo: Check references
using AutoMapper;
using Companies.Shared.DTOs;
using Domain.Contracts;
using Domain.Models.Responses;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Companies.Presemtation.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeesController : ApiControllerBase
    {
        // private readonly CompaniesContext _context;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork uow;
        private readonly IServiceManager serviceManager;

        public EmployeesController(IMapper mapper, IUnitOfWork uow, IServiceManager serviceManager)
        {
            //  _context = context;
            _mapper = mapper;
            this.uow = uow;
            this.serviceManager = serviceManager;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployee(int companyId)
        {
            ApiBaseResponse response = await serviceManager.EmployeeService.GetEmployeesAsync(companyId);


            return response.Success ?
                Ok(response.GetOkResult<IEnumerable<EmployeeDto>>()) :
                ProcessError(response);
        }

        // GET: api/Employees/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Employee>> GetEmployee(int id)
        //{
        //    var employee = await _context.Employee.FindAsync(id);

        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }

        //    return employee;
        //}

        //// PUT: api/Employees/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutEmployee(int id, Employee employee)
        //{
        //    if (id != employee.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(employee).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!EmployeeExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Employees
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        //{
        //    _context.Employee.Add(employee);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        //}

        //// DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id, int companyId)
        {
            var companyExist = await uow.CompanyRepository.CompanyExistsAsync(companyId);

            if (!companyExist) return NotFound();

            var employee = await uow.EmployeeRepository.GetEmployeeAsync(companyId, id);
            if (employee == null) return NotFound();

            uow.EmployeeRepository.Delete(employee);
            await uow.CompleteASync();

            return NoContent();
        }


        [HttpPatch("{id:int}")]
        public async Task<ActionResult> PatchEmployee(int companyId, int id, JsonPatchDocument<EmployeeUpdateDto> patchDocument)
        {
            if (patchDocument is null) return BadRequest("No patch document");

            //var companyExist = await _context.Companies.AnyAsync(c => c.Id.Equals(companyId));

            //if (!companyExist) return NotFound("Company not exist in database");

            var employeeToPatch = await uow.EmployeeRepository.GetEmployeeAsync(companyId, id, trackChanges: true);

            if (employeeToPatch == null) return NotFound("Employee not found");

            var dto = _mapper.Map<EmployeeUpdateDto>(employeeToPatch);

            patchDocument.ApplyTo(dto, ModelState);
            TryValidateModel(dto);

            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

            _mapper.Map(dto, employeeToPatch);
            await uow.CompleteASync();

            return NoContent();

        }

        //private bool EmployeeExists(int id)
        //{
        //    return _context.Employee.Any(e => e.Id == id);
        //}
    }
}
