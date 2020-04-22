using System;
using System.Collections.Generic;
using System.Text;

namespace SketchFab
{

    public class Thumbnails
    {
        public List<Image> images { get; set; }
    }

    public class Collection
    {
        public DateTime updatedAt { get; set; }
        public string slug { get; set; }
        public int subscriberCount { get; set; }
        public int modelCount { get; set; }
        public string name { get; set; }
        public DateTime createdAt { get; set; }
        public User user { get; set; }
        public object description { get; set; }
        public string embedUrl { get; set; }
        public string uid { get; set; }
        public Thumbnails thumbnails { get; set; }
        public string uri { get; set; }
        public string models { get; set; }
        public bool hasRestrictedContent { get; set; }
        public bool isAgeRestricted { get; set; }
    }



}
