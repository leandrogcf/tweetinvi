﻿using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Client;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Interfaces;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public interface ITweetsRequester
    {
        Task<ITwitterResult<ITweetDTO, ITweet>> GetTweet(long tweetId);
        Task<ITwitterResult<ITweetDTO[], ITweet[]>> GetTweets(long[] tweetIds);
        Task<ITwitterResult<ITweetDTO, ITweet>> PublishTweet(string text);
        Task<ITwitterResult<ITweetDTO, ITweet>> PublishTweet(IPublishTweetParameters parameters);
        Task<ITwitterResult> DestroyTweet(long tweetId);
        Task<ITwitterResult> DestroyTweet(ITweetDTO tweet);
    }

    public interface IInternalTweetsRequester : ITweetsRequester
    {
        void Initialize(ITwitterClient client);
    }

    public class TweetsRequester : BaseRequester, IInternalTweetsRequester
    {
        private readonly ITweetFactory _tweetFactory;
        private readonly ITweetController _tweetController;
        private ITwitterClient _twitterClient;

        public TweetsRequester(
            ITweetFactory tweetFactory, 
            ITweetController tweetController)
        {
            _tweetFactory = tweetFactory;
            _tweetController = tweetController;
        }

        public void Initialize(ITwitterClient client)
        {
            if (_twitterClient != null)
            {
                throw new InvalidOperationException("createRequest cannot be changed");
            }

            _twitterClient = client;
        }

        public Task<ITwitterResult<ITweetDTO, ITweet>> GetTweet(long tweetId)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _tweetFactory.GetTweet(tweetId, request), request);
        }

        public Task<ITwitterResult<ITweetDTO[], ITweet[]>> GetTweets(long[] tweetIds)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _tweetFactory.GetTweets(tweetIds, request), request);
        }

        public Task<ITwitterResult<ITweetDTO, ITweet>> PublishTweet(string text)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _tweetController.PublishTweet(text, request), request);
        }

        public Task<ITwitterResult<ITweetDTO, ITweet>> PublishTweet(IPublishTweetParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _tweetController.PublishTweet(parameters, request), request);
        }

        public Task<ITwitterResult> DestroyTweet(long tweetId)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _tweetController.DestroyTweet(tweetId, request), request);
        }

        public Task<ITwitterResult> DestroyTweet(ITweetDTO tweet)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _tweetController.DestroyTweet(tweet, request), request);
        }

        // Factories
    }
}
