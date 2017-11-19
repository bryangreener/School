using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace FinalProject.Models
{
	public class FeedOptions
	{
		public bool Bibble { get; set; }
		public bool Smash { get; set; }
		public bool PDF { get; set; }
		public string CustomText { get; set; }
		public HttpPostedFileBase CustomFile { get; set; }
	}
}