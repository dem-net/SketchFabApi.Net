using System;
using System.Collections.Generic;
using System.Text;

namespace DEM.Net.Extension.SketchFab
{
   
    public class Avatar
    {
        public string uri { get; set; }
        public List<Image> images { get; set; }
        public string uid { get; set; }
    }

    public class User
    {
        public string profileUrl { get; set; }
        public string uid { get; set; }
        public string displayName { get; set; }
        public string uri { get; set; }
        public Avatar avatar { get; set; }
        public string account { get; set; }
        public string username { get; set; }
    }

    public class Image
    {
        public int height { get; set; }
        public string url { get; set; }
        public int width { get; set; }
        public int size { get; set; }
        public string uid { get; set; }
    }

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

    public class PagedResult<T>
    {
        public string next { get; set; }
        public Cursors cursors { get; set; }
        public string previous { get; set; }
        public List<T> results { get; set; }
    }

    public class Cursors
    {
        public string next { get; set; }
        public string previous { get; set; }
    }

}
