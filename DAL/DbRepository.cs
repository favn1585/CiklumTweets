using System;
using System.Collections.Generic;
using System.IO;
using Mono.Data.Sqlite;
using TwitterRepository.Entities;

namespace DAL
{
    internal class DbRepository
    {
        private const string DbFileName = "favourites.db3";

        /// <summary>
        /// Get sonnection to Favourites DB
        /// </summary>
        /// <returns>connections</returns>
        private static SqliteConnection GetConnection()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DbFileName);
            var exists = File.Exists(dbPath);

            if (!exists)
                SqliteConnection.CreateFile(dbPath);

            var conn = new SqliteConnection("Data Source=" + dbPath);

            if (!exists)
                CreateDatabase(conn);

            return conn;
        }

        /// <summary>
        /// Create new favourites DB
        /// </summary>
        /// <param name="connection">connection</param>
        private static void CreateDatabase(SqliteConnection connection)
        {
            const string sql =
                "CREATE TABLE TWEETS (Id INTEGER PRIMARY KEY AUTOINCREMENT, TweetId INTEGER, Text ntext, Created datetime);";

            connection.Open();

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }

        /// <summary>
        /// Get all favourites tweets from DB
        /// </summary>
        /// <returns>list of tweets</returns>
        public static IEnumerable<Tweet> GetAllFavourites()
        {
            const string sql = "SELECT * FROM TWEETS;";

            using (var conn = GetConnection())
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            yield return new Tweet(reader.GetInt64(1), reader.GetString(2), reader.GetDateTime(3));
                    }
                }
            }
        }

        /// <summary>
        /// Delete all favourites tweets from DB
        /// </summary>
        public static void DaleteAllFavourites()
        {
            const string sql = "DELETE FROM TWEETS;";

            using (var conn = GetConnection())
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Delete tweet from favourites
        /// </summary>
        /// <param name="tweetId">tweet id to delete</param>
        public static void DeletFromFavourites(long tweetId)
        {
            var sql = string.Format("DELETE FROM TWEETS WHERE TweetId = {0};", tweetId);

            using (var conn = GetConnection())
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Add tweet to favourites
        /// </summary>
        /// <param name="tweet"></param>
        public static void AddFavourites(Tweet tweet)
        {
            using (var conn = GetConnection())
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO TWEETS (TweetId, Text, Created) VALUES (@TweetId, @Text, @Created); ";
                    cmd.Parameters.AddWithValue("@TweetId", tweet.Id);
                    cmd.Parameters.AddWithValue("@Text", tweet.Text);
                    cmd.Parameters.AddWithValue("@Created", tweet.PublishingDate);

                    cmd.ExecuteScalar();
                }
            }
        }
    }
}
