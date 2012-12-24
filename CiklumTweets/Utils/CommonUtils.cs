using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Views;
using TwitterRepository.Entities;

namespace CiklumTweets.Utils
{
    public class CommonUtils
    {
        /// <summary>
        /// Set display full screen and portra mode
        /// </summary>
        /// <param name="activity">current activity</param>
        public static void SetFullScreen(Activity activity)
        {
            activity.RequestWindowFeature(WindowFeatures.NoTitle);
            activity.Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            activity.RequestedOrientation = ScreenOrientation.Portrait;
        }

        /// <summary>
        /// Start new activity
        /// </summary>
        /// <param name="currentActivity">current activity</param>
        /// <param name="newActivityType">new activity to start</param>
        public static void StartNewActivity(Activity currentActivity, Type newActivityType)
        {
            var intent = new Intent(currentActivity, newActivityType);
            currentActivity.StartActivity(intent);
        }

        /// <summary>
        /// Check if tweetId exist in favourites list
        /// </summary>
        /// <param name="tweets">favourites list</param>
        /// <param name="id">tweet id</param>
        /// <returns>boolean result</returns>
        public static bool IsInList(List<Tweet> tweets, long id)
        {
            return tweets.Any(x => x.Id == id);
        }
    }
}