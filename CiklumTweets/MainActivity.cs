using System.Collections.Generic;
using System.Threading;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using CiklumTweets.Adapters;
using CiklumTweets.Utils;
using TwitterRepository;
using TwitterRepository.Entities;

namespace CiklumTweets
{
    [Activity(Label = "CiklumTweets", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private ListView _listView;
        private List<Tweet> _tweets;
        private Button _headerButton;
        private ListAdapter _listAdapter;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            CommonUtils.SetFullScreen(this);
            SetContentView(Resource.Layout.Main);

            _listView = FindViewById<ListView>(Resource.Id.List);
            _listAdapter = new ListAdapter(this, new List<Tweet>());

            var footerView =
                ((LayoutInflater) GetSystemService(LayoutInflaterService)).Inflate(Resource.Layout.FooterView, null);
            _headerButton = footerView.FindViewById<Button>(Resource.Id.buttonMoreFooter);
            _headerButton.Text = "Loading";

            _listView.AddFooterView(footerView);
            _listView.Adapter = _listAdapter;

            ThreadPool.QueueUserWorkItem(lt => LoadTweets());

            FindViewById<Button>(Resource.Id.buttonFavourites).Click +=
                delegate { CommonUtils.StartNewActivity(this, typeof (FavouritesActivity)); };
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (_tweets == null) return;

            _listAdapter = new ListAdapter(this, _tweets);
            _listView.Adapter = _listAdapter;
        }


        /// <summary>
        /// Loading tweets on application start
        /// </summary>
        private void LoadTweets()
        {
            _tweets = TwitterManager.GetTweets(20, null);

            var listAdapter = new ListAdapter(this, _tweets);

            RunOnUiThread(() =>
                              {
                                  _listView.Adapter = listAdapter;
                                  listAdapter.NotifyDataSetChanged();
                                  _listView.InvalidateViews();
                                  _headerButton.Text = "Load older tweets";

                                  _headerButton.Click +=
                                      delegate { ThreadPool.QueueUserWorkItem(lm => LoadMoreTweets()); };
                              });
        }

        /// <summary>
        /// Loading more 10 tweets
        /// </summary>
        private void LoadMoreTweets()
        {
            RunOnUiThread(() => { _headerButton.Text = "Loading"; });

            var newTweets = TwitterManager.GetTweets(10, _tweets[_tweets.Count - 1].Id);

            if (newTweets != null && newTweets.Count != 0)
            {
                _tweets.AddRange(newTweets);

                RunOnUiThread(() =>
                {
                    _listAdapter = new ListAdapter(this, _tweets);
                    _listAdapter.NotifyDataSetChanged();
                    _listView.InvalidateViews();
                });
            }

            RunOnUiThread(() => { _headerButton.Text = "Load older tweets"; });
        }
    }
}

