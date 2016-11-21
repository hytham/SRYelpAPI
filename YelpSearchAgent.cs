using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SRYelpAPI
{
	public class YelpSearchAgent : YelpAgent
	{
		public override string YelpAction
		{
			get
			{
				return "/v2/search/";
			}
		}

		public override void Send(YelpContext contex)
		{
			JObject res = contex.Get(YelpAction);
			
		}
	}
}
