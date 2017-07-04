using System.Diagnostics;
using System.Collections.Generic;

namespace BUAnalytics{

	public class BUPerformance: BUTemplate{

		public string Collection;

		private Stopwatch Sw = new Stopwatch();

		public string Name;

		public BUPerformance(string name, string collection) {
			Name = name;
			Collection = collection;
		}

		public void Start(){
			Sw.Start();
		}

		public void End(){
			Sw.Stop();
		}

		public void Upload(){

			//Add required fields
			this.AddRange(new Dictionary<string, object>(){
				{ "length", Sw.ElapsedMilliseconds }
			});

			//Add optional fields
			if (Name != null){ Add("name", Name); }

			base.Upload(Collection ?? "Performance");
		}
	}

}