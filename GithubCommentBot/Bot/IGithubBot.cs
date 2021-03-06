﻿using System.Threading.Tasks;
using GithubCommentBot.Dto;
using GithubCommentBot.Models;

namespace GithubCommentBot.Bot
{
    public interface IGithubBot
    {
        void Start();
        void Stop();
        Task AddCommentHook(PrCommentWebHook comment);
        Task AddApproveHook(PrWebHook prWebHook);
        Task AddRejectHook(PrWebHook prWebHook);
    }
}