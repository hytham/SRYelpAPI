using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRYelpAPI
{
	/// <summary>
	/// The main Yelp gant that will Perform the Yelp Operation
	/// </summary>
	public abstract class YelpAgent
	{
		/// <summary>
		/// The Childe class must implment this proparty to get result back
		/// </summary>
		public abstract String YelpAction { get ; }

		public abstract void Send(YelpContext contex);

		
	}
}
