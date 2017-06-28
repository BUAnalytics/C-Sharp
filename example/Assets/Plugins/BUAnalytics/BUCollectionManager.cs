using System;
using System.Timers;
using System.Collections.Generic;
using System.Diagnostics;

namespace BUAnalytics{
	
	public class BUCollectionManager {

		//Singleton
		public static readonly BUCollectionManager Instance = new BUCollectionManager();
		private BUCollectionManager(){ UploadAllPerform(null, null); }

		//Store collections
		public readonly Dictionary<string, BUCollection> Collections = new Dictionary<string, BUCollection>();

		//Upload timer interval
		public double Interval = 2000;

		//Events
		public Action<BUCollection, BUError> Error = null;
		public Action<BUCollection, int> Success = null;

		//Create collections from array of name
		public void Create(string[] names){
			foreach (string name in names){

				//Check for existing collection with name
				if (Collections.ContainsKey(name)) {
					continue;
				}

				//Create new collection
				var collection = new BUCollection(name);
				Collections.Add(name, collection);
			}
		}

		//Push documents in all collections to backend server
		private void UploadAllPerform(object source, ElapsedEventArgs e){
			this.UploadAll ();

			//Create timer to push all collections every x milliseconds
			if (Interval > 0) {
				var timer = new Timer ();
				timer.Elapsed += new ElapsedEventHandler (UploadAllPerform);
				timer.Interval = Interval;
				timer.AutoReset = false;
				timer.Start ();
			}
		}
		public void UploadAll(){
			
			//Upload all collections and forward response to error or success handlers
			foreach (BUCollection collection in Collections.Values){
				collection.Upload((code) => {

					//Notify error
					if (Error != null){
						Error.Invoke(collection, code);
					}

				}, (count) => {

					//Notify succes
					if (Success != null){
						Success.Invoke(collection, count);
					}
				});
			}
		}
	}

}