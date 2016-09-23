using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContosoInsurance.API.Models
{
    public class ImageSeed
    {
        private const string imagePrefix = "vehicle-";
        public Guid ImageKey { get; set; }

        public string GetBlobName()
        {
            return imagePrefix + ImageKey;
        }
        public string BlobUrl { get; set; }


    }
}