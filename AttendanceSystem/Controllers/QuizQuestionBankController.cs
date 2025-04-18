using AttendanceSystem.Models;
using AttendanceSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizQuestionBankController : ControllerBase
    {
        private readonly IQuizQuestionBankService _quizQuestionBankService;

        public QuizQuestionBankController(IQuizQuestionBankService quizQuestionBankService)
        {
            _quizQuestionBankService = quizQuestionBankService;
        }

        // GET: api/quizquestionbank
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizQuestionBank>>> GetAllBanks()
        {
            var banks = await _quizQuestionBankService.GetAllBanksAsync();
            return Ok(banks);
        }

        // GET: api/quizquestionbank/{bankId}
        [HttpGet("{bankId}")]
        public async Task<ActionResult<QuizQuestionBank>> GetBankById(int bankId)
        {
            var bank = await _quizQuestionBankService.GetBankByIdAsync(bankId);
            if (bank == null)
                return NotFound();
            return Ok(bank);
        }

        // POST: api/quizquestionbank
        [HttpPost]
        public async Task<ActionResult> CreateBank([FromBody] QuizQuestionBank bank)
        {
            if (bank == null)
                return BadRequest();
            await _quizQuestionBankService.CreateBankAsync(bank);
            return CreatedAtAction(nameof(GetBankById), new { bankId = bank.QuestionBankID }, bank);
        }

        // PUT: api/quizquestionbank/{bankId}
        [HttpPut("{bankId}")]
        public async Task<ActionResult> UpdateBank(int bankId, [FromBody] QuizQuestionBank bank)
        {
            if (bankId != bank.QuestionBankID)
                return BadRequest("ID mismatch.");
            await _quizQuestionBankService.UpdateBankAsync(bank);
            return NoContent();
        }

        // DELETE: api/quizquestionbank/{bankId}
        [HttpDelete("{bankId}")]
        public async Task<ActionResult> DeleteBank(int bankId)
        {
            await _quizQuestionBankService.DeleteBankAsync(bankId);
            return NoContent();
        }
    }
}
