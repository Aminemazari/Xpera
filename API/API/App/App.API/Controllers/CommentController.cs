﻿using App.API.Models;
using App.API.Models.Post_Models.Comment_Models;
using App.API.Services.Interfaces;
using App.Models.Dtos.Comment;
using App.Models.Dtos.User.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;

namespace App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController(ICommandService commandService, IQueryService queryService) : ControllerBase
    {
        private readonly ICommandService _commandService = commandService;
        private readonly IQueryService _queryService = queryService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ICommentDto>>> GetComments([FromQuery] int? user_id, [FromQuery] int? post_id)
        {
            IEnumerable <ICommentDto> comments = null!;

            if (user_id.HasValue)
            {
                
                comments = await _queryService.ReadCommentsByUserIdAsync(user_id.Value);

            }
            else if (post_id.HasValue)
            {
                comments = await _queryService.ReadCommentsByPostIdAsync(post_id.Value);

            }
            else
            {
                return BadRequest("required to send user_id or post_id");
            }

            return Ok(comments);

        }

        [HttpGet("{id}", Name = "ReadCommentById")]
        public ActionResult<CommentFullReadDto> ReadCommentById(int id)
        {
            return new CommentFullReadDto()
            {
                Id = 1,
                Post = new(),
                Text = "comment",
                User = new(),
                Time = DateTime.Now,
            };
        }

        [HttpPost]
        public async Task<ActionResult<CommentMinReadDto>> CreateComment([FromBody] CommentCreateDto commentCreate)
        {
            CommentMinReadDto user = await _commandService.CreateCommentAsync(commentCreate);

            return CreatedAtRoute(nameof(ReadCommentById), new { Id = user.Id }, user);

        }


    }
}
