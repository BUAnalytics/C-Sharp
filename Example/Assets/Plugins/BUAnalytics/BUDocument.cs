using System.Collections;
using System.Collections.Generic;

namespace BUAnalytics{
	
	public class BUDocument{

		public Dictionary<string, object> Contents;

		public BUDocument(){
			this.Contents = new Dictionary<string, object>();
		}

		public BUDocument(Dictionary<string, object> document){
			this.Contents = document;
		}

		public void Add(string key, object value){
			Contents.Add(key, value);
		}

		public void AddRange(Dictionary<string, object> document){
			foreach (var item in document){
				Contents.Add(item.Key, item.Value);
			}
		}
	}

}