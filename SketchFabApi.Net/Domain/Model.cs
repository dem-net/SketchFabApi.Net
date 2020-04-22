//
// Model.cs
//
// Author:
//       Xavier Fischer 2020-4
//
// Copyright (c) 2020 Xavier Fischer
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SketchFab
{
    public class Warning
    {
    }

    public class Status
    {
        public string processing { get; set; }
        public Warning warning { get; set; }
    }

    public class Model
    {
        public int vertexCount { get; set; }
        public string description { get; set; }
        public License license { get; set; }
        public User user { get; set; }
        public bool isProtected { get; set; }
        public string viewerUrl { get; set; }
        public string uid { get; set; }
        public int soundCount { get; set; }
        public IList<Category> categories { get; set; }
        public int faceCount { get; set; }
        public DateTime createdAt { get; set; }
        public bool isAgeRestricted { get; set; }
        public IList<Tag> tags { get; set; }
        public string name { get; set; }
        public int animationCount { get; set; }
        public int commentCount { get; set; }
        public Thumbnails thumbnails { get; set; }
        public bool isDownloadable { get; set; }
        public string embedUrl { get; set; }
        public int likeCount { get; set; }
        public object price { get; set; }
        public DateTime? publishedAt { get; set; }
        public string uri { get; set; }
        public Status status { get; set; }
        public object staffpickedAt { get; set; }
        public int viewCount { get; set; }
        public string editorUrl { get; set; }
        public int downloadCount { get; set; }
    }

    public static class ModelExtensions
    {
        public static bool IsPending(this Model model, string expectedStatus = "PENDING")
        {
            return model.HasStatus(expectedStatus);
        }

        public static bool IsProcessing(this Model model, string expectedStatus = "PROCESSING")
        {
            return model.HasStatus(expectedStatus);
        }

        public static bool IsReady(this Model model, string expectedStatus = "SUCCEEDED")
        {
            return model.HasStatus(expectedStatus);
        }

        private static bool HasStatus(this Model model, string expectedStatus)
        {
            return model != null && model.status.processing == expectedStatus;
        }
    }


}
