namespace BUAnalytics{

	public class BTScore: BUTemplate{
		
		public string Collection;

		public int? Value;
		public bool? Highest;

		public BTScore(int value, string collection) {
			Value = value;
			Collection = collection;
		}

		public void Upload(){

			//Add optional fields
			if (Value != null){ Add("value", Value); }
			if (Highest != null){ Add("highest", Highest); }

			base.Upload(Collection ?? "Scores");
		}
	}
}