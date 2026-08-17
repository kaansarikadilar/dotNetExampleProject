using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using api_example.DTOs.Comment;
using api_example.Extensions;
using api_example.Helpers;
using api_example.Mappers;
using api_example.Models;
using api_example.Repository;
using api_example.Service.IService;
using api_example.StockRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace api_example.Controllers
{
    [Route("/api_example/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    { 
        private readonly ICommentRepository _CommentRepo;
        private readonly IStockRepository _StockRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFMPService _fmpService;
        public CommentController(ICommentRepository commentRepo,IStockRepository stockRepo,UserManager<AppUser> userManager,IFMPService fmpService)
        {
            _CommentRepo = commentRepo;
            _StockRepo = stockRepo;
            _userManager = userManager;
            _fmpService = fmpService;
        }

        [HttpGet]
        [Authorize]
        public async  Task<IActionResult> GetCommentsAll([FromQuery] CommentQueryObject queryObject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comments = await _CommentRepo.GetCommentsAsync(queryObject);

            var CommentDTO = comments.Select(s => s.toCommentDto());
            return Ok(CommentDTO);
        }
        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comment = await _CommentRepo.GetByIdAsync(id);
            if(comment == null)
            {
                return NotFound();
            }

            return Ok(comment.toCommentDto());
        }
        [HttpPost("{symbol:alpha}")]
         public async Task<IActionResult> Create([FromRoute] string symbol, CreateCommentDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stock = await _StockRepo.GetBySymbolAsync(symbol.ToUpper()); 

            if(stock == null)
            {
                stock = await _fmpService.FindStockBySymbol(symbol.ToUpper());
                if(stock == null)
                {
                    return BadRequest("This stock does not exist");
                }
                else
                {
                    await _StockRepo.CreateAsync(stock);
                }
            }
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            

            var  commentModel = commentDto.toCommentFromCreate(stock.Id); 
            commentModel.AppUserId = appUser?.Id;

            await _CommentRepo.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById),

            new {id = commentModel.Id}, commentModel.toCommentDto());
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult>Update([FromRoute] int id,[FromBody] UpdateCommentRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comment = await _CommentRepo.UpdateAsync(id,requestDto.toCommentFromUpdate());

            if(comment == null)
            {
                return NotFound("Comment not found");
            }
            
            
            return Ok(comment.toCommentDto());
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult>Delete([FromRoute]int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
             var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            
            var commentModel = await _CommentRepo.DeleteAsync(id);
            if(commentModel == null)
            {
                return NotFound("Comment cant be found to delete");
            }
            commentModel.AppUserId = appUser?.Id; // added this line to get the user id

            return Ok(commentModel.toCommentDto()); 
            // turned it into the dto to return only the parameters we want
        }
    }
}