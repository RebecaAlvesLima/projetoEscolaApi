using Microsoft.AspNetCore.Mvc;
using projetoEscolaAPI.Data;
using projetoEscolaAPI.Models;

namespace projetoEscolaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunoController: ControllerBase
    {
        private EscolaContext _context;
        public AlunoController(EscolaContext context){
            //constructor
            _context = context;
        }
        [HttpGet]
        public ActionResult<List<Aluno>> GetAll(){
            return _context.Aluno.ToList();
        }

        [HttpGet("{AlunoId}")]
        public ActionResult<Aluno>Get(int AlunoId){
            try{
                var result = _context.Aluno.Find(AlunoId);
                if(result == null){
                    return NotFound();
                }
                return Ok(result);
            }
            catch{
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha ao acesso ao banco de dados");
            }
        }

        

        [HttpPost]
        public async Task<ActionResult> post(Aluno model){
            try{
                _context.Aluno.Add(model);
                if(await _context.SaveChangesAsync()==1){
                    //return Ok
                    return Created($"/api/aluno/{model.ra}",model);
                }
            }
            catch{
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha ao acesso do banco de dados");
            }
            //retorna badRequest se não conseguir incluir
            return BadRequest();
        }

        [HttpDelete("{AlunoId}")]
        public async Task<ActionResult> delete(int AlunoId){
            try{
                //verifica se existe aluno a ser excluido
                var aluno = await _context.Aluno.FindAsync(AlunoId);
                if(aluno == null){
                    //metodo ef
                    return NotFound();
                }
                _context.Remove(aluno);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch{
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Falha no acesso ao banco de dados.");
            }
        }

        [HttpPut("{AlunoId}")]
        public async Task<IActionResult> put(int AlunoId, Aluno dadosAlunoAlt){
            try{
                //verifica se existe aluno a ser alterado
                var result = await _context.Aluno.FindAsync(AlunoId);
                if(AlunoId != result.id){
                    return BadRequest();
                }
                result.ra = dadosAlunoAlt.ra;
                result.nome = dadosAlunoAlt.nome;
                result.codCurso = dadosAlunoAlt.codCurso;
                await _context.SaveChangesAsync();
                return Created($"/api/aluno/{dadosAlunoAlt.id}", dadosAlunoAlt);
            }
            catch{
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Falha no acesso ao banco de dados");
            }
        }
    }
}