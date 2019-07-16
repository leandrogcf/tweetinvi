﻿using System;
using Tweetinvi.Models.Interfaces;

namespace Tweetinvi.Core.Client
{
    public interface ITwitterExecutionContext : ITweetinviSettings
    {
        Func<ITwitterRequest> RequestFactory { get; set; }
        ITwitterExecutionContext Clone();
    }

    public class TwitterExecutionContext : TweetinviSettings, ITwitterExecutionContext
    {
        public TwitterExecutionContext()
        {
            RequestFactory = () => throw new InvalidOperationException($"You cannot run contextual operations without defining configuring the {nameof(RequestFactory)} of the ExecutionContext");
        }

        public Func<ITwitterRequest> RequestFactory { get; set; }
        public new ITwitterExecutionContext Clone()
        {
            var clone = new TwitterExecutionContext
            {
                RequestFactory = RequestFactory
            };

            clone.InitialiseFrom(this);

            return clone;
        }
    }
}
