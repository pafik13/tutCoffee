using System.Collections.Generic;

using Android.App;
using Android.Views;
using Android.Widget;

using iCoffe.Shared;

using UniversalImageLoader.Core;

namespace iCoffe.Droid.Adapters
{
    public class PurchasedOffersAdapter : BaseAdapter<Offer>
    {
        Activity Context;
        IList<Offer> Offers;

        public PurchasedOffersAdapter(Activity context, IList<Offer> offers) : base()
        {
            Context = context;
            Offers = offers;
        }

        public override Offer this[int position]
        {
            get { return Offers[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return Offers.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            // Get our object for position
            var offer = Offers[position];

            var view = (convertView ??
                                Context.LayoutInflater.Inflate(
                                Resource.Layout.PurchasedOfferItem,
                                parent,
                                false)) as RelativeLayout;
            view.FindViewById<TextView>(Resource.Id.poiOfferTitleTV).Text = string.IsNullOrEmpty(offer.Title) ? "<unknow offer>" : offer.Title;

            if (!string.IsNullOrEmpty(offer.LogoUrl))
            {
                ImageLoader.Instance.DisplayImage(offer.LogoUrl, view.FindViewById<ImageView>(Resource.Id.poiOfferLogoIV));
            }

            //Finally return the view
            return view;
        }
    }
}