using System;
using System.Collections.Generic;
using System.Timers;
using System.Diagnostics;

namespace BUAnalytics{

	public class BUID {

		//Singleton
		public static readonly BUID Instance = new BUID();
		private BUID(){}

		//Store identifiers
		private List<string> Identifiers = new List<string>();

		//Check whether identifiers exist
		public bool IsReady{ get{ return Identifiers.Count > 0; } }

		//Refresh timer interval and id cache size
		public double Interval = 4000;
		public double Size = 100;

		//Return first id in cache list and remove
		public string Generate(){

			//Check whether identifiers are depleted
			if (Identifiers.Count <= 0){

				//Log error
				Trace.TraceError("[BUAnalytics] Identifier cache has been depleted, please adjust your BUID cache size or interval");

				//Generate backup identifier
				return Guid.NewGuid().ToString();
			}

			//Grab identifier and remove from cache
			var id = Identifiers[0];
			Identifiers.RemoveAt(0);
			return id;
		}

		//Start caching identifiers
		public void Start(double size = 100){
			Size = size;
			RefreshPerform(null, null);
		}

		//Push documents in all collections to backend server
		private void RefreshPerform(object source, ElapsedEventArgs e){

			//Only refresh if identifier cache is a quarter empty
			if (Identifiers.Count < ((Size / 4) * 3)){
				Refresh();
			}

			//Create timer to refresh every x milliseconds
			if (Interval > 0) {
				var timer = new Timer();
				timer.Elapsed += new ElapsedEventHandler(RefreshPerform);
				timer.Interval = Interval;
				timer.AutoReset = false;
				timer.Start();
			}
		}
		public void Refresh(){

			//Upload data to server using api request
			var count = (Size - Identifiers.Count).ToString();
			BUAPI.Instance.RequestWithPath("/projects/collections/documents/ids/" + count, BUMethod.GET, (code) => {

				//Log error code
				Trace.TraceError("[BUAnalytics] Failed to refresh " + count + " identifiers from server with error code " + code);

			}, (response) => {

				//Cast and add identifiers from response
				foreach(JSONNode id in response["ids"].AsArray){
					Identifiers.Add(id as string);
				}
			});
		}
	}

}