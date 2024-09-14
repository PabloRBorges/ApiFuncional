using ApiFuncional.Data;
using ApiFuncional.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiFuncional.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly ApiDbContext _db;

        public ProdutosController(ApiDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            if (_db.Produtos == null) return NotFound();

            return await _db.Produtos.ToListAsync();
        }

        //[AllowAnonymous] //apenas o método não precisa de autorização
       // [EnableCors("Production")] // habilita o cors de produção apenas no endpoint
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            if (_db.Produtos == null) return NotFound();

            var produto = await _db.Produtos.FindAsync(id);

            if (produto == null) return NotFound();

            return Ok(produto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PostProduto(Produto produto)
        {
            if (_db.Produtos == null) return Problem("Erro ao criar um produto, contate o suporte");

            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
                //return ValidationProblem(ModelState);

                return ValidationProblem(new ValidationProblemDetails(ModelState)
                {
                    Title = "Um ou mais erros de validação ocorreram!"
                });
            }

            var result = _db.Produtos.AddAsync(produto);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduto), new { id = produto.id }, produto);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PutProduto(int id, Produto produto)
        {
            if (id != produto.id) return BadRequest();

            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            _db.Entry(produto).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            if (_db.Produtos == null) return Problem("Erro ao excluir o produto, contate o suporte");

            if (!ProdutoExists(id)) return NotFound();

            var produto = await _db.Produtos.FindAsync(id);

            if (produto == null) return BadRequest();

            _db.Produtos.Remove(produto);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        private bool ProdutoExists(int id) => (_db.Produtos?.Any(e => e.id == id)).GetValueOrDefault();
    }
}
