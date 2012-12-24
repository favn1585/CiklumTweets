using System.Collections.Generic;
using System.Linq;
using TwitterRepository.Entities;

namespace DAL
{
    public class DbManager
    {
        /// <summary>
        /// Get all favourite tweets
        /// </summary>
        /// <returns>list of tweets</returns>
        public static List<Tweet> GetAllFavourites()
        {
            return DbRepository.GetAllFavourites().ToList();
        }

        /// <summary>
        /// Delete all favourite tweets
        /// </summary>
        public static void DeleteAllFavourites()
        {
            DbRepository.DaleteAllFavourites();
        }

        /// <summary>
        /// Delete tweet from favourites
        /// </summary>
        /// <param name="tweetId">tweet id to delete</param>
        public static void DeletFromFavourites(long tweetId)
        {
            DbRepository.DeletFromFavourites(tweetId);
        }

        /// <summary>
        /// Add tweet to favourites
        /// </summary>
        /// <param name="tweet">tweet entity</param>
        public static void AddFavourites(Tweet tweet)
        {
            DbRepository.AddFavourites(tweet);
        }
    }
}