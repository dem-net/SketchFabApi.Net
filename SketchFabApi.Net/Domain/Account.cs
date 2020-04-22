//
// Account.cs
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
namespace SketchFab
{
    public class Account
    {
        public bool canProtectModels { get; set; }
        public string account { get; set; }
        public ViewOnlyLimit viewOnlyLimit { get; set; }
        public DateTime joinedAt { get; set; }
        public PrivateModelsLimit privateModelsLimit { get; set; }
        public object renewsAt { get; set; }
        public object billingCycle { get; set; }
        public int uploadSizeLimit { get; set; }
    }

    public class ViewOnlyLimit
    {
        public string type { get; set; }
        public DateTime renews_at { get; set; }
        public int remaining { get; set; }
    }

    public class PrivateModelsLimit
    {
        public object count { get; set; }
        public object since { get; set; }
        public object until { get; set; }
    }


}
