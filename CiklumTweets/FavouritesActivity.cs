using System.Threading;
using Android.App;
using Android.OS;
using Android.Widget;
using CiklumTweets.Adapters;
using CiklumTweets.Utils;
using DAL;

namespace CiklumTweets
{
    [Activity(Label = "Favourites")]
    public class FavouritesActivity : Activity
    {
        private ListView _listView;
        private FavouritesAdapter _favouritesAdapter;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            CommonUtils.SetFullScreen(this);
            SetContentView(Resource.Layout.FavouritesActivity);

            _listView = FindViewById<ListView>(Resource.Id.List);

            ThreadPool.QueueUserWorkItem(lt => LoadFavourites());

            FindViewById<Button>(Resource.Id.buttonClearFavourites).Click +=
                delegate{ ThreadPool.QueueUserWorkItem(lt => DeleteFavourites()); };
        }

        /// <summary>
        /// Load Favourite tweets
        /// </summary>
        private void LoadFavourites()
        {
            _favouritesAdapter = new FavouritesAdapter(this);

            RunOnUiThread(() => { _listView.Adapter = _favouritesAdapter; });
        }

        /// <summary>
        /// Delete all favourite tweets and back to main activity
        /// </summary>
        private void DeleteFavourites()
        {
            DbManager.DeleteAllFavourites();
            CommonUtils.StartNewActivity(this, typeof(MainActivity)); 
        }
    }
}