﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;
using SimpleOAuth;

namespace SRYelpAPI
{
	/// <summary>
	/// The main Yelp Context
	/// </summary>
    public class YelpContext
    {

		Dictionary<String, String> ReqParams = new Dictionary<string, string>();

		

		private const string yelpendpoint = "https://api.yelp.com";
		string AccessToken = System.Configuration.ConfigurationSettings.AppSettings["AccessToken"];
		string AccessTokenSecret = System.Configuration.ConfigurationSettings.AppSettings["AccessTokenSecret"];
		string ConsumerKey = System.Configuration.ConfigurationSettings.AppSettings["ConsumerKey"];
		string ConsumerSecret = System.Configuration.ConfigurationSettings.AppSettings["ConsumerSecret"];
		private string tOKEN;
		private string tOKEN_SECRET;
		private string cONSUMER_KEY;
		private string cONSUMER_SECRET;


		/// <summary>
		/// The Main Constrcutor that will read the yelp configuration from the app.conf/web.conf file
		/// </summary>
		public YelpContext()
		{
			ReqParams.Clear();
		}

		public YelpContext(string tOKEN, string tOKEN_SECRET, string cONSUMER_KEY, string cONSUMER_SECRET)
		{
			this.AccessToken = tOKEN;
			this.AccessTokenSecret = tOKEN_SECRET;
			this.ConsumerKey = cONSUMER_KEY;
			this.ConsumerSecret = cONSUMER_SECRET;
		}

		public  JObject Get(string path)
		{
			String R = "";

			var query = System.Web.HttpUtility.ParseQueryString(String.Empty);
	
			foreach (var queryParam in ReqParams)
			{
				
					query[queryParam.Key] = queryParam.Value;
			}

			var uriBuilder = new UriBuilder(yelpendpoint + path);
			uriBuilder.Query = query.ToString();

			var request = WebRequest.Create(uriBuilder.ToString());
			request.Method = "GET";
			request.SignRequest(
				new Tokens
				{
					ConsumerKey = ConsumerKey,
					ConsumerSecret = ConsumerSecret,
					AccessToken = AccessToken,
					AccessTokenSecret = AccessTokenSecret
				}
			).WithEncryption(EncryptionMethod.HMACSHA1).InHeader();

			var httpResponse = (HttpWebResponse)request.GetResponse();
			using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
			{
				var result = streamReader.ReadToEnd();
				R = result.ToString();
			}
			return JObject.Parse(R);
		}

	

		#region Yelp Paramerters

		/// <summary>
		/// Number of search results braught back
		/// </summary>
		public int Limit
		{
			set
			{
				if (ReqParams.ContainsKey("limit"))
				{
					ReqParams.Add("limit", value.ToString());
				}
				else
				{
					ReqParams["limit"] = value.ToString();
				}
			}
		}

		/// <summary>
		/// Sort the returned result as follow
		/// 0=Best matched (default), 1=Distance, 2=Highest Rated.
		/// For details see yelp documentation
		/// </summary>
		public int Sort
		{
			set
			{
				if (ReqParams.ContainsKey("sort"))
				{
					ReqParams.Add("sort", value.ToString());
				}
				else
				{
					ReqParams["sort"] = value.ToString();
				}
			}
		}

		/// <summary>
		/// The search tearm
		/// </summary>
		public String Term
		{
			set
			{
				if (ReqParams.ContainsKey("term"))
				{
					ReqParams.Add("term", value.ToString());
				}
				else
				{
					ReqParams["term"] = value.ToString();
				}
			}
		}
		/// <summary>
		/// Offset the list of returned business results by this amount
		/// For details see yelp documentation
		/// </summary>
		public String Offset
		{
			set
			{
				if (ReqParams.ContainsKey("offset"))
				{
					ReqParams.Add("offset", value.ToString());
				}
				else
				{
					ReqParams["offset"] = value.ToString();
				}
			}
		}

		/// <summary>
		/// Category to filter search results with
		/// </summary>
		public string categoryFilter
		{
			set
			{
				if (ReqParams.ContainsKey("category_filter"))
				{
					ReqParams.Add("category_filter", value.ToString());
				}
				else
				{
					ReqParams["category_filter"] = value.ToString();
				}
			}
		}

		/// <summary>
		/// Search radius in meters.
		/// </summary>
		public int radiusFilter
		{
			set
			{
				if (ReqParams.ContainsKey("radius_filter"))
				{
					ReqParams.Add("radius_filter", value.ToString());
				}
				else
				{
					ReqParams["radius_filter"] = value.ToString();
				}
			}
		}
		/// <summary>
		/// Whether to exclusively search for businesses with deals
		/// </summary>
		public int dealsFilter
		{
			set
			{
				if (ReqParams.ContainsKey("deals_filter"))
				{
					ReqParams.Add("deals_filter", value.ToString());
				}
				else
				{
					ReqParams["deals_filter"] = value.ToString();
				}
			}
		}
		/// <summary>
		/// The location 
		/// Specifies the combination of "address, neighborhood, city, state or zip, optional country" to be used when searching for businesses.
		/// </summary>
		public string Location
		{
			set
			{
				if (ReqParams.ContainsKey("location"))
				{
					ReqParams.Add("location", value.ToString());
				}
				else
				{
					ReqParams["location"] = value.ToString();
				}
			}
		}

		/// <summary>
		/// Set the geolocation searching criteria
		/// </summary>
		/// <param name="latitude">The Latitude to searchj for</param>
		/// <param name="longitude"> The Longtitude to search for</param>
		public void setGelocation(float latitude,float longitude)
		{
			string p = latitude + "," + longitude;
			if (ReqParams.ContainsKey("cll"))
			{
				ReqParams.Add("cll", p);
			}
			else
			{
				ReqParams["cll"] = p;
			}

		}

		/// <summary>
		/// Set teh Searching Bounding Box
		/// </summary>
		/// <param name="sw_latitude">SW Latitude</param>
		/// <param name="sw_longitude">SW Longtitude</param>
		/// <param name="ne_latitude">NE Latitude</param>
		/// <param name="ne_longitude">NE Longtitude</param>
		public void setGeolocationBound(float sw_latitude, float sw_longitude, float ne_latitude, float ne_longitude)
		{
			string p = sw_latitude + "," + sw_longitude + "|"+ ne_latitude + "," + ne_longitude;
			if (ReqParams.ContainsKey("bounds"))
			{
				ReqParams.Add("bounds", p);
			}
			else
			{
				ReqParams["bounds"] = p;
			}
		}

		/// <summary>
		/// Specify Location by Geographic Coordinate
		/// </summary>
		/// <param name="latitude"></param>
		/// <param name="longitude"></param>
		/// <param name="accuracy"></param>
		/// <param name="altitude"></param>
		/// <param name="altitude_accuracy"></param>
		public void setGeolocationCoordinate(float latitude, float longitude, float accuracy, float altitude, float altitude_accuracy)
		{
			string p = latitude + "," + longitude + "," + accuracy + "," + altitude+","+ altitude_accuracy;
			if (ReqParams.ContainsKey("ll"))
			{
				ReqParams.Add("ll", p);
			}
			else
			{
				ReqParams["ll"] = p;
			}
		}

		/// <summary>
		/// Set the Country code local
		/// </summary>
		public string countryCode
		{
			set
			{
				if (ReqParams.ContainsKey("cc"))
				{
					ReqParams.Add("cc", value.ToString());
				}
				else
				{
					ReqParams["c"] = value.ToString();
				}
			}
		}
		
		
		/// <summary>
		/// The Language Code
		/// </summary>
		public string Language
		{
			set
			{
				if (ReqParams.ContainsKey("lang"))
				{
					ReqParams.Add("lang", value.ToString());
				}
				else
				{
					ReqParams["lang"] = value.ToString();
				}
			}
		}

		/// <summary>
		/// Whether to include links to actionable content if available
		/// </summary>
		public bool actionLinks
		{
			set
			{
				if (ReqParams.ContainsKey("actionlinks"))
				{
					ReqParams.Add("actionlinks", value.ToString());
				}
				else
				{
					ReqParams["actionlinks"] = value.ToString();
				}
			}
		}
		#endregion

	}
}
