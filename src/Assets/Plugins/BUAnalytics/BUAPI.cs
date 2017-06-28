using System;
using System.Net;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace BUAnalytics{

	public enum BUMethod{ GET, POST, PUT, DELETE, PATCH, OPTIONS, HEAD }

	public class BUAPI{

		public string URL = "https://bu-games.bmth.ac.uk";
		public string Path = "/api/v1";
		public BUAccessKey Auth = null;

		//URL prefix for HTTP requests
		public string HostPrefix{
			get{ return (URL ?? "") + (Path ?? ""); }
		}

		//Singleton
		public static readonly BUAPI Instance = new BUAPI();

		//Setup
		public BUAPI(){
			ServicePointManager.ServerCertificateValidationCallback = ValidateCertificate;
		}

		//Convenience methods for url and path appended requests and for taking a json class or encoding a nested dictionary object
		public void RequestWithPath(string path, BUMethod method, Action<BUError> error, Action<JSONNode> success){
			MakeRequestWithURL(this.HostPrefix + path, method, "", error, success);
		}
		public void RequestWithPath(string path, BUMethod method, object parameters, Action<BUError> error, Action<JSONNode> success){
			if (method == BUMethod.GET || method == BUMethod.DELETE) {
				MakeRequestWithURL(this.HostPrefix + path, method, parameters is IDictionary ? EncodeURL(parameters) : "", error, success);
			} else {
				MakeRequestWithURL(this.HostPrefix + path, method, EncodeJSON(parameters).ToString(), error, success);
			}
		}
		public void RequestWithPath(string path, BUMethod method, JSONClass parameters, Action<BUError> error, Action<JSONNode> success){
			if (method == BUMethod.GET || method == BUMethod.DELETE) {
				MakeRequestWithURL(this.HostPrefix + path, method, "", error, success);
			} else {
				MakeRequestWithURL(this.HostPrefix + path, method, EncodeJSON(parameters).ToString(), error, success);
			}
		}

		//Convenience methods for url appended requests and for taking a json class or encoding a nested dictionary object
		public void RequestWithURL(string url, BUMethod method, Action<BUError> error, Action<JSONNode> success){
			MakeRequestWithURL((this.URL ?? "") + url, method, "", error, success);
		}
		public void RequestWithURL(string url, BUMethod method, object parameters, Action<BUError> error, Action<JSONNode> success){
			if (method == BUMethod.GET || method == BUMethod.DELETE) {
				MakeRequestWithURL((this.URL ?? "") + url, method, parameters is IDictionary ? EncodeURL(parameters) : "", error, success);
			} else {
				MakeRequestWithURL((this.URL ?? "") + url, method, EncodeJSON(parameters).ToString(), error, success);
			}
		}
		public void RequestWithURL(string url, BUMethod method, JSONClass parameters, Action<BUError> error, Action<JSONNode> success){
			if (method == BUMethod.GET || method == BUMethod.DELETE) {
				MakeRequestWithURL((this.URL ?? "") + url, method, "", error, success);
			} else {
				MakeRequestWithURL((this.URL ?? "") + url, method, EncodeJSON(parameters).ToString(), error, success);
			}
		}

		//Make a generic request to the backend server url, response is sent to error and success closures
		private void MakeRequestWithURL(string url, BUMethod method, string body, Action<BUError> error, Action<JSONNode> success){
			
			//Url could not be found
			if (this.URL == null) {
				if (error != null) {
					error.Invoke (BUError.NotFound);
				}
				return;
			}

			//Create http request
			using (var client = new WebClient()){

				//Prepare http headers
				client.Headers[HttpRequestHeader.ContentType] = "application/json; charset=utf-8"; 

				//Authenticate request with access key and secret
				if (this.Auth != null) {
					client.Headers.Add("AuthAccessKey", this.Auth.Key);
					client.Headers.Add("AuthAccessSecret", this.Auth.Secret);
				}

				//Add completion handlers and make http request
				if (method == BUMethod.GET || method == BUMethod.DELETE) {
					client.DownloadStringCompleted += (s, r) => { RequestDidComplete(r.Error, r.Result, error, success); };
					client.DownloadStringAsync(new Uri(url + body), method.ToString());
				} else {
					client.UploadStringCompleted += (s, r) => { RequestDidComplete(r.Error, r.Result, error, success); };
					client.UploadStringAsync(new Uri(url), method.ToString(), body);
				}
			}
		}

		//Read a http response by checking the exception codes and converting it into json format, then calling appropriate callback function
		private void RequestDidComplete(Exception exception, string response, Action<BUError> error, Action<JSONNode> success){

			//Handle any response exceptions
			if (exception != null){
				if (error != null) {
					if (exception is WebException) {

						//Check if we failed to connect to the server
						var webException = exception as WebException;
						if (webException.Response == null) {
							error.Invoke (BUError.Connection);
						} else {

							//Check the http response status codes
							HttpWebResponse httpError = (exception as WebException).Response as HttpWebResponse;
							if (httpError.StatusCode == HttpStatusCode.NotFound) {
								error.Invoke (BUError.NotFound);
							} else if (httpError.StatusCode == HttpStatusCode.InternalServerError) {
								error.Invoke (BUError.Server);
							} else {
								error.Invoke (BUError.Server);
							}
						}

					} else {
						error.Invoke (BUError.Unknown);
					}
				}
				return;
			}

			//Process the received response
			try {

				//Check string response was given
				var result = response.Trim();
				if (result == "") {
					if (error != null) {
						error.Invoke (BUError.Json);
					}
					return;
				}

				//Empty json was returned
				if (result == "{}") {
					if (success != null) {
						success.Invoke (new JSONClass());
					}
					return;
				}

				//Attempt to parse json string from stream
				var json = JSON.Parse (result);

				//Check for server side errors
				if (json ["error"] != null) {
					if (error != null) {
						error.Invoke((BUError)json["error"].AsInt);
					}
					return;
				}

				//Return the successful json object
				if (success != null){
					success.Invoke(json);
				}

			} catch {

				//JSON failed to parse
				if (error != null) {
					error.Invoke (BUError.Json);
				}
			}
		}

		//Encode parameters as key/value pairs in url a format
		private string EncodeURL(object parameters){
			string body = "?";
			foreach (DictionaryEntry entry in (System.Collections.IDictionary)parameters) {
				body += entry.Key + "=" + entry.Value.ToString () + "&";
			}
			return body.TrimEnd ('&');
		}

		//Encode any nested dictionary or list type into a json node object
		private JSONNode EncodeJSON(object parameters){

			//Deal with simple value types first
			if (parameters is string) {
				return new JSONData (parameters as string);
			} else if (parameters is bool) {
				return new JSONData ((bool)parameters);
			} else if (parameters is int) {
				return new JSONData ((int)parameters);
			} else if (parameters is double) {
				return new JSONData ((double)parameters);
			} else if (parameters is float) {
				return new JSONData ((float)parameters);
			} else if (parameters is DateTime) {
				return new JSONData ((double)(((DateTime)parameters).Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
			} else if (parameters.GetType ().IsEnum) {
				return new JSONData ((int)parameters);
			} else if (parameters.GetType ().IsValueType) {
				
				//Encode struct types
				var node = new JSONClass();
				foreach (var field in parameters.GetType().GetFields()) {
					if (!field.IsLiteral) {
						node.Add (field.Name, EncodeJSON(field.GetValue(parameters)));
					}
				}
				return node;
			}

			//Deal with generic data types
			else if (parameters is System.Collections.IEnumerable) {

				//Dictionary type was found
				if (parameters is System.Collections.IDictionary) {
					var node = new JSONClass ();

					//Cycle through each element as a generic item and recursively process them
					foreach (DictionaryEntry entry in (System.Collections.IDictionary)parameters) {
						node.Add((string)entry.Key, EncodeJSON(entry.Value)); 
					}

					return node;

					//List type was found
				} else if (parameters is System.Collections.IList) {
					var array = new JSONArray ();

					//Cycle through each element as a generic item and recursively process them
					foreach (var entry in (System.Collections.IList)parameters) {
						array.Add(EncodeJSON(entry));
					}

					return array;
				}
			}

			return new JSONData(0);
		}

		//Validate third party ssl certificates for https requests
		public bool ValidateCertificate(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
			bool isOk = true;

			// If there are errors in the certificate chain, look at each error to determine the cause.
			if (sslPolicyErrors != SslPolicyErrors.None) {
				for (int i=0; i<chain.ChainStatus.Length; i++) {
					if (chain.ChainStatus [i].Status != X509ChainStatusFlags.RevocationStatusUnknown) {
						chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
						chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
						chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan (0, 1, 0);
						chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
						bool chainIsValid = chain.Build ((X509Certificate2)certificate);
						if (!chainIsValid) {
							isOk = false;
						}
					}
				}
			}

			return isOk;
		}
	}
}