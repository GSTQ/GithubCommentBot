﻿using System.Threading.Tasks;
using GithubCommentBot.Bot;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GithubCommentBot.Models;
using GithubCommentBot.Dto;

namespace GithubCommentBot.Controllers
{
    [Route("api/[controller]")]
    public class HookController : Controller
    {
        public HookController(IGithubBot bot, ILogger<HookController> logger)
        {
            _bot = bot;
            _logger = logger;
        }

        [HttpPost]
        [Route("comments")]
        public async Task GetCommentHook([FromBody] PrCommentWebHook prCommentWebHook)
        {
            _logger.LogInformation($"Got new comment hook");
            if (prCommentWebHook.Action == "created" || prCommentWebHook.Action == "updated")
            {
                _logger.LogInformation($"Send comment to telegram");
                await _bot.AddCommentHook(prCommentWebHook);
            }
            else
            {
                _logger.LogInformation($"Dont send to telegram beacause action = {prCommentWebHook.Action}");
            }
        }


        [HttpPost]
        [Route("pr")]
        public async Task GetPrHook([FromBody] PrWebHook prWebHook)
        {
            _logger.LogInformation($"Got new prWebHook hook");
            if (prWebHook.Review != null)
            {
                if (prWebHook.Review.State == "approved")
                {
                    _logger.LogInformation($"Send approve message to telegram");
                    await _bot.AddApproveHook(prWebHook);
                }
                else if (prWebHook.Review.State == "changes_requested")
                {
                    _logger.LogInformation($"Send reject message to telegram");
                    await _bot.AddRejectHook(prWebHook);
                }
            }
            else
            {
                _logger.LogInformation($"Pull request review is null");
            }
        }


        private readonly IGithubBot _bot;
        private readonly ILogger<HookController> _logger;
    }
}
