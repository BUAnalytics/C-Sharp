using System;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace BUAnalytics{
	
	public class BUCollection{

		public readonly string Name;

		public BUCollection(string name){
			this.Name = name;
		}

		//Document properties, sending data is moved from documents to buffer
		private readonly List<BUDocument> Documents = new List<BUDocument>();
		private readonly List<BUDocument> Buffer = new List<BUDocument>();

		//Check whether any documents exist and if any are being uploaded
		public bool IsUploading{ get{ return Buffer.Count > 0; } }
		public bool IsEmpty{ get{ return Documents.Count <= 0; } }

		//Add single or multiple documents to collection
		public void Add(BUDocument document){
			this.Documents.Add(document);
		}
		public void AddRange(IEnumerable<BUDocument> documents){
			this.Documents.AddRange(documents);
		}

		//Upload pending documents to backend server
		public void Upload(Action<BUError> error = null, Action<int> success = null){

			//Make sure there are documents available and is not already uploading
			if (IsUploading) { return; }
			if (IsEmpty) { return; }

			//Move documents to buffer
			Buffer.AddRange(Documents);
			Documents.Clear();

			//Convert documents to objects list
			var objects = new List<object>();
			foreach (BUDocument document in Buffer){
				objects.Add(document.Contents);
			}

			//Upload data to server using api request
			var body = new Dictionary<string, List<object>>{ { "documents", objects } };
			BUAPI.Instance.RequestWithPath("/projects/collections/" + Name + "/documents", BUMethod.POST, body, (code) => {

				//Log error code
				Trace.TraceError("[BUAnalytics][" + Name + "] Failed to push " + Buffer.Count + " documents to server with error code " + code);

				//Move buffer back to documents list
				Documents.AddRange(Buffer);
				Buffer.Clear();

				//Notify action
				if (error != null){
					error.Invoke(code);
				}

			}, (response) => {

				//Notify
				if (success != null){
					success.Invoke(Buffer.Count);
				}
				
				//Remove buffer contents
				Buffer.Clear();
			});
		}
	}

}