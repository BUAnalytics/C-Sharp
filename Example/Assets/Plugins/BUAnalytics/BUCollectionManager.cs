using System;
using System.Timers;
using System.Collections.Generic;

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
				Collections.Add(name, new BUCollection(name));
			}
		}

		//Convenience method for adding a document to a collection and creating the collection if non-existant
		public void Add(string collection, Dictionary<string, object> document){
			this.Add(collection, new BUDocument(document));
		}
		public void Add(string collection, BUDocument document){

			//Check whether document exists and create
			if  (!Collections.ContainsKey(collection)){
				Collections.Add(collection, new BUCollection (collection));
			}

			//Add document to collection
			Collections[collection].Add(document);
		}

		//Push documents in all collections to backend server
		private void UploadAllPerform(object source, ElapsedEventArgs e){
			UploadAll();

			//Create timer to push all collections every x milliseconds
			if (Interval > 0) {
				var timer = new Timer();
				timer.Elapsed += new ElapsedEventHandler(UploadAllPerform);
				timer.Interval = Interval;
				timer.AutoReset = false;
				timer.Start();
			}
		}
		public void UploadAll(){
			
			//Upload all collections and forward response to error or success handlers
			foreach (BUCollection collection in Collections.Values){
				var col = collection;
				collection.Upload((code) => {
					
					//Notify error
					if (Error != null){
						Error.Invoke(col, code);
					}

				}, (count) => {
					
					//Notify succes
					if (Success != null){
						Success.Invoke(col, count);
					}
				});
			}
		}
	}

}