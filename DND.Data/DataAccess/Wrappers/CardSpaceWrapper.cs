using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cashew.Data;
using DND.DataAccess;
using DND.Domain;

namespace DND.Data.DataAccess.Wrappers
{
    public class CardSpaceWrapper : CardSpace
    {
        struct Context
        {
            public Context(IImageDataAccess dataAccess, int imageId)
            {
                DataAccess = dataAccess;
                ImageId = imageId;
            }

            public IImageDataAccess DataAccess { get; }
            public int ImageId { get; }
        }

        public CardSpaceWrapper(IImageDataAccess imageDataAccess, int? imageId)
        {
            if (!imageId.HasValue)
                return;

            var context = new Context(imageDataAccess, imageId.Value);
            _backgroundImage =
                new PropWrapper<Image>(context, o => ((Context) o).DataAccess.Get(((Context) o).ImageId));
        }

        readonly PropWrapper<Image> _backgroundImage;

        public override Image BackgroundImage
        {
            get => _backgroundImage.Value;
            set => _backgroundImage.Value = value;
        }
    }
}
