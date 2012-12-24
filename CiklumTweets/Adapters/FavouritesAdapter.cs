using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using CiklumTweets.Utils;
using DAL;
using TwitterRepository.Entities;

namespace CiklumTweets.Adapters
{
    public class FavouritesAdapter : BaseAdapter<Tweet>
    {
        private readonly List<Tweet> _items;
        private readonly Activity _context;

        public FavouritesAdapter(Activity context)
        {
            _context = context;
            _items = DbManager.GetAllFavourites();
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
            Tweet item = _items[position];
            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.RowFavourite, null);

            view.FindViewById<TextView>(Resource.Id.textViewMessageFavourite).Text = item.Text;
            view.FindViewById<TextView>(Resource.Id.textViewDateFavourite).Text =
                item.PublishingDate.ToString("d MMM yyyy HH:mm");

            var button = view.FindViewById<ImageButton>(Resource.Id.imageButtonDeleteFavourite);

            button.Click +=
                delegate
                    {
                        DbManager.DeletFromFavourites(item.Id);
                        _items.Remove(item);

                        if (_items.Count == 0)
                            CommonUtils.StartNewActivity(_context, typeof (MainActivity));
                        else
                            NotifyDataSetChanged();
                    };

            return view;
        }
    }
}