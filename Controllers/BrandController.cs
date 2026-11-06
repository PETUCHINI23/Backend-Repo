using Backend.DTOs;
using Backend.Exceptions;
using Backend.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IValidator<BrandInsertDto> _brandInsertValidator;
        private readonly IValidator<BrandUpdateDto> _brandUpdateValidator;
        private readonly IBrandService _brandService;

        public BrandController(
            IValidator<BrandInsertDto> brandInsertValidator,
            IValidator<BrandUpdateDto> brandUpdateValidator,
            [FromKeyedServices("brandService")] IBrandService brandService)
        {
            _brandInsertValidator = brandInsertValidator;
            _brandUpdateValidator = brandUpdateValidator;
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IEnumerable<BrandDto>> Get() =>
            await _brandService.Get();

        [HttpGet("{id}")]
        public async Task<ActionResult<BrandDto>> GetById(int id)
        {
            var brand = await _brandService.GetById(id);
            return brand == null ? NotFound() : Ok(brand);
        }

        [HttpGet("{id}/beers")]
        public async Task<ActionResult<IEnumerable<BeerDto>>> GetBeers(int id)
        {
            var beers = await _brandService.GetBeers(id);
            return beers == null ? NotFound() : Ok(beers);
        }

        [HttpPost]
        public async Task<ActionResult<BrandDto>> Add(BrandInsertDto brandInsertDto)
        {
            var validationResult = await _brandInsertValidator.ValidateAsync(brandInsertDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var brandDto = await _brandService.Add(brandInsertDto);

            return CreatedAtAction(nameof(GetById), new { id = brandDto.Id }, brandDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BrandDto>> Update(int id, BrandUpdateDto brandUpdateDto)
        {
            if (id != brandUpdateDto.Id)
            {
                return BadRequest(new { message = "El Id de la ruta no coincide con el Id del cuerpo" });
            }

            var validationResult = await _brandUpdateValidator.ValidateAsync(brandUpdateDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var brand = await _brandService.Update(id, brandUpdateDto);
            return brand == null ? NotFound() : Ok(brand);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BrandDto>> Delete(int id)
        {
            try
            {
                var brand = await _brandService.Delete(id);
                return brand == null ? NotFound() : Ok(brand);
            }
            catch (ConflictException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
