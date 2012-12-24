using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using CiklumTweets.Utils;
using DAL;
using TwitterRepository.Entities;

namespace CiklumTweets.Adapters
{
    public class ListAdapter : BaseAdapter<Tweet>
    {
        private readonly List<Tweet> _items;
        private readonly Activity _context;
        private readonly List<Tweet> _favourites;

        public ListAdapter(Activity context, List<Tweet> items)
        {
            _context = context;
            _items = items;

            _favourites = DbManager.GetAllFavourites();
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Tweet this[int position]
        {
            get { return _items[position]; }
        }

        public override int Count
        {
            get { return _items.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = _items[position];

            var view = _context.LayoutInflater.Inflate(Resource.Layout.RowView, null);

            view.FindViewById<TextView>(Resource.Id.textViewMessage).Text = item.Text;
            view.FindViewById<TextView>(Resource.Id.textViewDate).Text = item.PublishingDate.ToString("d MMM yyyy HH:mm");

            var button = view.FindViewById<ImageButton>(Resource.Id.imageButtonFavourite);

            button.Click += delegate
                                {
                                    if (CommonUtils.IsInList(_favourites, item.Id))
                                    {
                                        DbManager.DeletFromFavourites(item.Id);
                                        _favourites.Remove(item);
                                        button.SetImageResource(Resource.Drawable.favourite_image_nonactive);
                                    }
                                    else
                                    {
                                        DbManager.AddFavourites(new Tweet(item.Id, item.Text, item.PublishingDate));
                                        _favourites.Add(item);
                                        button.SetImageResource(Resource.Drawable.favourite_image);
                                    }

                                };

            button.SetImageResource(CommonUtils.IsInList(_favourites, item.Id)
                                        ? Resource.Drawable.favourite_image
                                        : Resource.Drawable.favourite_image_nonactive);

            return view;
        }
    }
}