using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using webapbackend.Dto;
using webapbackend.Interface;
using webapbackend.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace webapbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointment _appointmentRepository;
        private readonly IUser _userRepository;
        private readonly ICategory _categoryRepository;
        private readonly IProduct _productRepository;
        private readonly IMapper _mapper;

        public AppointmentController(
            IAppointment appointmentRepository,
            IUser userRepository,
            ICategory categoryRepository,
            IProduct productRepository,
            IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AppointmentDto>))]
        public IActionResult GetAppointments()
        {
            var appointments = _mapper.Map<List<AppointmentDto>>(_appointmentRepository.GetAppointments());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(appointments);
        }
        [HttpGet("User/{userId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AppointmentDto>))]
        [ProducesResponseType(404)]
       
        public IActionResult GetAppointmentsByUserId(int userId)
        {
            if (!_userRepository.UserExists(userId))
                return NotFound("User not found");

            var appointments = _mapper.Map<List<AppointmentDto>>(_appointmentRepository.GetAppointmentsByUserId(userId));

            if (!appointments.Any())
                return NotFound("No appointments found for this user");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(appointments);
        }

        [HttpGet("{AppointmentId}")]
        [ProducesResponseType(200, Type = typeof(AppointmentDto))]
        [ProducesResponseType(400)]
        public IActionResult GetAppointment(int AppointmentId)
        {
            if (!_appointmentRepository.AppointmentExists(AppointmentId))
                return NotFound();

            var appointment = _mapper.Map<AppointmentDto>(_appointmentRepository.GetAppointment(AppointmentId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(appointment);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateAppointment([FromBody] AppointmentDto appointmentCreate)
        {
            if (appointmentCreate == null)
                return BadRequest("Invalid appointment data.");

            if (!_userRepository.UserExists(appointmentCreate.UserId))
                return BadRequest("UserId does not exist.");

            if (!_categoryRepository.CategoryExists(appointmentCreate.CategoryId))
                return BadRequest("CategoryId does not exist.");

            if (!_productRepository.ProductExists(appointmentCreate.ProductId))
                return BadRequest("ProductId does not exist.");

            var existingAppointment = _appointmentRepository.GetAppointments()
                .FirstOrDefault(c => c.Id == appointmentCreate.Id);

            if (existingAppointment != null)
            {
                ModelState.AddModelError("", "Appointment already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var appointmentMap = _mapper.Map<Appointment>(appointmentCreate);

            if (!_appointmentRepository.CreateAppointment(appointmentMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving the appointment");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPut("{AppointmentId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateAppointment(int AppointmentId, [FromBody] AppointmentDto updatedAppointment)
        {
            if (updatedAppointment == null)
                return BadRequest("Invalid appointment data.");

            if (AppointmentId != updatedAppointment.Id)
                return BadRequest("Appointment ID mismatch.");

            if (!_appointmentRepository.AppointmentExists(AppointmentId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var appointmentMap = _mapper.Map<Appointment>(updatedAppointment);

            if (!_appointmentRepository.UpdateAppointment(appointmentMap))
            {
                ModelState.AddModelError("", "Something went wrong updating appointment");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{AppointmentId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]


        public IActionResult DeleteAppointment(int AppointmentId)
        {
            if (!_appointmentRepository.AppointmentExists(AppointmentId))
                return NotFound();

            var appointmentToDelete = _appointmentRepository.GetAppointment(AppointmentId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_appointmentRepository.DeleteAppointment(appointmentToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting appointment");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
