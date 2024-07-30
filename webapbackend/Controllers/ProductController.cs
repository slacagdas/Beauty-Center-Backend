using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using webapbackend.Dto;
using webapbackend.Interface;
using webapbackend.Models;

namespace webapbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProduct _productRepository;

        private readonly IMapper _mapper;

        public ProductController(IProduct productRepository,

            IMapper mapper)
        {
            _productRepository = productRepository;

            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Product>))]
        public IActionResult GetProducts()
        {
            var products = _mapper.Map<List<ProductDto>>(_productRepository.GetProducts());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(products);
        }

        [HttpGet("{ProductId}")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(400)]
        public IActionResult GetProduct(int ProductId)
        {
            if (!_productRepository.ProductExists(ProductId))
                return NotFound();

            var product = _mapper.Map<ProductDto>(_productRepository.GetProduct(ProductId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(product);
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateProduct([FromBody] ProductDto productCreate)
        {
            if (productCreate == null)
                return BadRequest(ModelState);

            var products = _productRepository.GetProducts()
                .Where(c => c.Id == productCreate.Id)
                .FirstOrDefault();

            if (products != null)
            {
                ModelState.AddModelError("", "Product already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productMap = _mapper.Map<Product>(productCreate);


            if (!_productRepository.CreateProduct(productMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{ProductId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProduct(int ProductId, [FromBody] ProductDto updatedProduct)
        {
            if (updatedProduct == null)
                return BadRequest(ModelState);

            if (ProductId != updatedProduct.Id)
                return BadRequest(ModelState);

            if (!_productRepository.ProductExists(ProductId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var ProductMap = _mapper.Map<Product>(updatedProduct);

            if (!_productRepository.UpdateProduct(ProductMap))
            {
                ModelState.AddModelError("", "Something went wrong updating product");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{ProductId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProduct(int ProductId)
        {
            if (!_productRepository.ProductExists(ProductId))
            {
                return NotFound();
            }

            var productToDelete = _productRepository.GetProduct(ProductId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_productRepository.DeleteProduct(productToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting product");
            }

            return NoContent();
        }
    }
}
